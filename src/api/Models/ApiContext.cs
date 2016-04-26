using Microsoft.Data.Entity;

namespace api.Models
{
    public class ApiContext : DbContext
    {
        public DbSet<Building> Buildings { get; set; }
        public DbSet<Floor> Floors { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<SensorData> SensorData { get; set; }
    }
}