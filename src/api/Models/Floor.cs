using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Floor
    {
        public string FloorId { get; set; }

        [Display(Name = "Floor name")]
        public string FloorName { get; set; }

        public virtual ICollection<Room> Rooms { get; set; }
    }
}
