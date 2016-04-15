using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace api.Controllers
{
    [Route("api/[controller]")]
    public class FloorsController : Controller
    {
        // GET: api/floors
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "floor1", "floor2" };
        }

        // GET api/floors/floor2
        [HttpGet("{id}")]
        public string Get(int floorid)
        {
            return "{geojson for floor2}";
        }

        // GET api/floors/floor2/rooms/room2
        [HttpGet("{floorid}/rooms/{roomid}")]
        public string Get(int floorid, int roomid)
        {
            return "{fancy history object}";
        }
    }
}
