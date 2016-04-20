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
        /// 
        /// <returns>IActionResult.</returns>
        [HttpGet("buildings")]
        public IActionResult GetBuildings()
        {
            return new ObjectResult(new
            {
                buildings = _context.Buildings.Select(b => new { b.Id, b.Name }).ToList()
            });
        }

        // GET: api/buildings/1
        [HttpGet("buildings/{buildingId}")]
        public IActionResult GetFloors(int buildingId)
        {
            var buildings = _context.Buildings.Where(b => b.Id == buildingId);

            if (!buildings.Any()) return HttpNotFound();
            return new ObjectResult(buildings.First());
        }

        // GET api/buildings/1/1
        [HttpGet("buildings/{buildingId}/floors/{number}")]
        public IActionResult GetRooms(int buildingId, int number)
        {
            var floor = _context.Floors.Single(f => f.BuildingId == buildingId && f.Number == number);
            var roomsOnFloor = _context.Rooms.Where(r => r.FloorId == floor.Id).ToList();

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
                            data = _context.SensorData.Where(s => s.RoomId == r.Id)
                        },
                        geometry = new
                        {
                            type = "Polygon",
                            coordinates = JArray.Parse(r.GeoJson)
                        }
                    })
            });
        }

        // GET api/floors/floor2/rooms/room2
        [HttpGet("rooms/{roomId}")]
        public IActionResult GetRoom(int roomId)
        {
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
