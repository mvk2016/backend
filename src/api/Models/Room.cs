using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.SqlServer.Types;

namespace api.Models
{
    public class Room
    {
        public int Id { get; set; }
        public int FloorId { get; set; }

        [Display(Name = "Room Name")]
        public string Name { get; set; }
        public SqlGeometry GeoJson { get; set; }

        public virtual ICollection<SensorData> SensorData { get; set; }
    }
}
