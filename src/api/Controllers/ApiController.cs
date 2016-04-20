using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using Microsoft.AspNet.Mvc;

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

        [HttpGet]
        public IActionResult DefaultAction()
        {
            return HttpBadRequest();
        }

        // GET: api/buildings
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
            });
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
                .Join(rooms, f => f.Id, r => r.FloorId, (f, r) => new { r.Name, r.GeoJson });

            return floor.Any() ? Json(new { Floor = new
            {
                BuildingId = b.BuildingId,
                Number = b.Number,
                Rooms = floor
            } }) : Json("[]");
        }

        // GET api/floors/floor2/rooms/room2
        [HttpGet("{buildingId}/{floorId}/{roomId}")]
        public string GetRoom(int buildingId, int floorId, int roomId)
        {
            return "{fancy history object}";
        }
    }
}
