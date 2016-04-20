﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Building
    {
        [Required]
        public int Id { get; set; }

        [Display(Name = "Building Name")]
        [Required]
        public string Name { get; set; }

        public virtual ICollection<Floor> Floors { get; set; }
    }
}
