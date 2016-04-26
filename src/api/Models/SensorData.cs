using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    /// <summary>
    ///     Defines the SensorData data model.
    /// </summary>
    public class SensorData
    {
        public int Id { get; set; }
        public int RoomId { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string SensorId { get; set; }

        public DateTime Collected { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string Type { get; set; }

        [Column(TypeName = "float")]
        public double Value { get; set; }
    }
}