﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace api.Models
{
    public class Room
    {
        public int Id { get; set; }
        public int FloorId { get; set; }

        [Display(Name = "Room Name")]
        public string Name { get; set; }

        public string GeoJson { get; set; }

        public virtual ICollection<SensorData> SensorData { get; set; }
    }
}