using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Room
    {
        [ScaffoldColumn(false)]
        public int RoomId { get; set; }

        [Display(Name = "Room Name")]
        public string RoomName { get; set; }

        public virtual ICollection<SensorData> SensorData { get; set; }
    }
}
