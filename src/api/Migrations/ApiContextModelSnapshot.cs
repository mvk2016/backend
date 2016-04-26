using System;
using api.Models;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;

namespace api.Migrations
{
    [DbContext(typeof(ApiContext))]
    internal class ApiContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-rc1-16348")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("api.Models.Building", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<string>("Name")
                    .IsRequired();

                b.HasKey("Id");
            });

            modelBuilder.Entity("api.Models.Floor", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<int?>("BuildingId");

                b.Property<int>("Number");

                b.HasKey("Id");
            });

            modelBuilder.Entity("api.Models.Room", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<int?>("FloorId");

                b.Property<string>("GeoJson");

                b.Property<string>("Name");

                b.HasKey("Id");
            });

            modelBuilder.Entity("api.Models.SensorData", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<DateTime>("Collected");

                b.Property<int?>("RoomId");

                b.Property<string>("SensorId")
                    .HasAnnotation("Relational:ColumnType", "nvarchar(50)");

                b.Property<string>("Type")
                    .HasAnnotation("Relational:ColumnType", "nvarchar(50)");

                b.Property<double>("Value")
                    .HasAnnotation("Relational:ColumnType", "float");

                b.HasKey("Id");
            });

            modelBuilder.Entity("api.Models.Floor", b =>
            {
                b.HasOne("api.Models.Building")
                    .WithMany()
                    .HasForeignKey("BuildingId");
            });

            modelBuilder.Entity("api.Models.Room", b =>
            {
                b.HasOne("api.Models.Floor")
                    .WithMany()
                    .HasForeignKey("FloorId");
            });

            modelBuilder.Entity("api.Models.SensorData", b =>
            {
                b.HasOne("api.Models.Room")
                    .WithMany()
                    .HasForeignKey("RoomId");
            });
        }
    }
}