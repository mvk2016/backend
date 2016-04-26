using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace api.Models
{
    public class Floor
    {
        public int Id { get; set; }
        public int BuildingId { get; set; }

        [Display(Name = "Floor name")]
        public int Number { get; set; }

        public virtual ICollection<Room> Rooms { get; set; }
    }
}