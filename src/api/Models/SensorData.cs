using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class SensorData
    {
        public int SensorDataId { get; set; }

        [Display(Name = "Sensor Type")]
        public string SensorType { get; set; }

        public double SensorValue { get; set; }
    }
}
