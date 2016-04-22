using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using api.Models;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace api.Controllers
{
    /// <summary>
    /// Provides API endpoints for fetching building metadata, floor plans and sensor data.
    /// All routes begin with <code>/api/</code>
    /// </summary>
    /// 
    /// <seealso cref="Microsoft.AspNet.Mvc.Controller" />
    [Route("api")]
    public class ApiController : Controller
    {
        private readonly ApiContext _context;
        private readonly ILogger<ApiController> _logger;

        /// <summary>
        /// Initialize <see cref="ApiController"/>.
        /// </summary>
        /// 
        /// <param name="context">Database context injected by ASP.NET DI</param>
        /// <param name="logger">Logger injected by ASP.NET DI</param>
        /// <seealso cref="Microsoft.Data.Entity.DbContext"/>
        public ApiController(ApiContext context, ILogger<ApiController> logger)
        {
            // Inject database context for queries, and logging utility
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// <code>GET /api/</code> always throws HTTP 400.
        /// </summary>
        [HttpGet]
        public IActionResult DefaultAction()
        {
            return HttpBadRequest();
        }

        /// <summary>
        /// <code>GET /api/buildings</code> lists all buildings.
        /// 
        /// Example response:
        /// <code>
        /// {
        ///   "buildings": [
        ///     {
        ///       "id": 871073,
        ///       "name": "Keflavik"
        ///     },
        ///     ...
        ///   ]
        /// }
        /// </code>
        /// </summary>
        [HttpGet("buildings")]
        public IActionResult GetBuildings()
        {
            return new ObjectResult(
                new { buildings = _context.Buildings.Select(b => new { b.Id, b.Name }).ToList() });
        }

        /// <summary>
        /// <code>GET /api/buildings/:buildingId</code>
        /// Lists all floors for a given building ID.
        /// 
        /// Example response:
        /// <code>
        /// {
        ///   "floors": [
        ///     {
        ///       "id": 2,
        ///       "buildingId": 871073,
        ///       "number": 5,
        ///       "rooms": null
        ///     },
        ///     ...
        ///   ]
        /// }
        /// </code>
        /// </summary>
        /// 
        /// <param name="buildingId">Building identifier</param>
        /// 
        [HttpGet("buildings/{buildingId}")]
        public IActionResult GetFloors(int buildingId)
        {
            // Make sure the building actually exists
            var building = _context.Buildings.Single(b => b.Id == buildingId);
            if (building == null) return HttpNotFound("Building does not exist");

            // Return floor numbers as a JSON array
            return new ObjectResult(new
            {
                buildingId,
                floors = _context.Floors
                                 .Where(f => f.BuildingId == building.Id)
                                 .Select(f => f.Number)
                                 .ToList()
            });
        }

        /// <summary>
        /// <code>GET /api/buildings/:id/floors/:floor</code>
        /// Provides a GeoJSON FeatureCollection of a given floor in a given building.
        /// Each feature in the collection represents a room on the map and contains
        /// metadata and sensor data properties, as well as geometry object which specifies
        /// the room's geographical location on a world map using polygon coordinates.
        /// 
        /// Example response:
        /// <code>
        /// {
        ///   "type": "FeatureCollection",
        ///   "features": [
        ///     {
        ///       "type": "Feature",
        ///       "properties": {
        ///         "roomId": 1,
        ///         "name": "Conference Room C",
        ///         "data": [
        ///           {
        ///             "collected": "2016-04-19T11:30:00",
        ///             "type": "temperature",
        ///             "value": 296.3
        ///           },
        ///           ...
        ///         ]
        ///       },
        ///       "geometry": {
        ///         "type": "Polygon",
        ///         "coordinates": [
        ///           [
        ///             18.073729835450649,
        ///             59.34676929431086
        ///           ],
        ///           ...
        ///         ]
        ///       }
        ///     }
        ///   ]
        /// }
        /// </code>
        /// </summary>
        /// 
        /// <param name="buildingId">Building identifier</param>
        /// <param name="number">Floor number</param>
        /// 
        [HttpGet("buildings/{buildingId}/floors/{number}")]
        public IActionResult GetRooms(int buildingId, int number)
        {
            var floor = _context.Floors.Single(f => f.BuildingId == buildingId && f.Number == number);
            var roomsOnFloor = _context.Rooms.Where(r => r.FloorId == floor.Id).ToList();
            var types = _context.SensorData.Select(d => d.Type).Distinct().ToList();

            if (floor == null)       return HttpNotFound("Floor does not exist in building");
            if (!roomsOnFloor.Any()) return HttpNotFound("Floor has no rooms");
            
            return new ObjectResult(new
            {
                type = "FeatureCollection",
                features = roomsOnFloor.Select(r => new
                    {
                        type = "Feature",
                        properties = new
                        {
                            roomId = r.Id,
                            name = r.Name,
                            data = types.Select(t => _context.SensorData
                                        .Where(s => s.RoomId == r.Id && s.Type == t)
                                        .Select(s => new { s.Type, s.Value, s.Collected })
                                        .OrderByDescending(s => s.Collected)
                                        .Take(1))
                        },
                        geometry = new
                        {
                            type = "Polygon",
                            coordinates = JArray.Parse(r.GeoJson)
                        }
                    })
            });
        }

        /// <summary>
        /// <code>GET /api/history/:roomId?start=2016-03-22T12:00:00[&end=2016-04-22T12:00:00]</code>
        /// Returns calculated average data for a given room during a given time period.
        /// Timespan is set using ISO 8601 formatted GET query string parameters:
        /// <code>start</code> - Beginning of timespan (mandatory)
        /// <code>end</code> - End of timespan (optional, defaults to DateTime.Now if no value supplied)
        /// </summary>
        /// 
        /// <param name="roomId">Room ID number</param>
        [HttpGet("history/{roomId}")]
        public IActionResult GetHistory(int roomId)
        {
            var start = Request.Query["start"];
            var room = _context.Rooms.Where(rm => rm.Id == roomId);
            if (!room.Any())
                return HttpNotFound();
            var r = room.First();

            var data = _context
                .SensorData
                .Where(d => d.RoomId == roomId)
                .ToList();

            // Always send back room data, set data to empty array if room has no data
            return data.Any()
                ? Json(new { Floor = new { r.Id, r.Name, Data = data } })
                : Json(new { Floor = new { r.Id, r.Name, Data = "[]" } });
        }
    }
}
