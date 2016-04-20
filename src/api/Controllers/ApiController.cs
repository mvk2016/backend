using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using Microsoft.AspNet.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

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
        private readonly JsonSerializerSettings _apiJsonSettings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        /// <summary>
        /// Initialize <see cref="ApiController"/>.
        /// </summary>
        /// 
        /// <param name="context">Database context injected by ASP.NET DI</param>
        /// <seealso cref="Microsoft.Data.Entity.DbContext"/>
        public ApiController(ApiContext context)
        {
            // Inject database context for queries
            _context = context;
        }

        /// <summary>
        /// <code>/api/</code> throws HTTP 400.
        /// </summary>
        [HttpGet]
        public IActionResult DefaultAction()
        {
            return HttpBadRequest();
        }

        // GET: api/buildings        
        /// <summary>
        /// <code>/api/buildings</code> lists all buildings.
        /// 
        /// </summary>
        /// <returns>IActionResult.</returns>
        [HttpGet("buildings")]
        public IActionResult GetBuildings()
        {
            return Json(new
            {
                Buildings = _context.Buildings.Select(b => new Building
                {
                    Id = b.Id,
                    Name = b.Name
                }).ToList()
            }, _apiJsonSettings);
        }

        // GET: api/buildings/1
        [HttpGet("buildings/{buildingId}")]
        public IActionResult GetFloors(int buildingId)
        {
            var buildings = _context.Buildings.Where(b => b.Id == buildingId);
            return buildings.Any() ? (IActionResult) Json(buildings.First()) : HttpNotFound();
        }

        // GET api/buildings/1/1
        [HttpGet("buildings/{buildingId}/floors/{number}")]
        public IActionResult GetRooms(int buildingId, int number)
        {
            var rooms = _context.Rooms;
            var buildings = _context
                .Floors
                .Where(f => f.BuildingId == buildingId && f.Number == number);

            if (!buildings.Any())
                return HttpNotFound();
            var b = buildings.First();

            var floor = buildings
                .Join(rooms, f => f.Id, r => r.FloorId, (f, r) => new
                {
                    type = "Feature",
                    properties = new
                    {
                        roomId = r.Id,
                        name = r.Name,
                    },
                    geometry = new
                    {
                        type = "Polygon",
                        coordinates = JObject.Parse(@"{""f"": [" + r.GeoJson + "]}")["f"]
                    }
                });

            // Always send back building data, set rooms to empty array if floor has no rooms
            return floor.Any()
                ? Json(new
                {
                    type = "FeatureCollection",
                    features = floor
                }, _apiJsonSettings)
                : Json("[]");
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
