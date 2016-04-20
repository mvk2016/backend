using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using api.Models;

namespace api.Migrations
{
    [DbContext(typeof(ApiContext))]
    [Migration("20160419194837_InitialUpdates")]
    partial class InitialUpdates
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-rc1-16348")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("api.Models.Building", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BuildingName")
                        .IsRequired();

                    b.HasKey("Id");
                });

            modelBuilder.Entity("api.Models.Floor", b =>
                {
                    b.Property<string>("FloorId");

                    b.Property<int?>("BuildingBuildingId");

                    b.Property<string>("FloorName");

                    b.HasKey("FloorId");
                });

            modelBuilder.Entity("api.Models.Room", b =>
                {
                    b.Property<int>("RoomId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("FloorFloorId");

                    b.Property<string>("GeoJson");

                    b.Property<string>("Name");

                    b.HasKey("RoomId");
                });

            modelBuilder.Entity("api.Models.SensorData", b =>
                {
                    b.Property<int>("SensorDataId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Collected");

                    b.Property<int?>("RoomRoomId");

                    b.Property<string>("SensorId")
                        .HasAnnotation("Relational:ColumnType", "nvarchar(50)");

                    b.Property<string>("Type")
                        .HasAnnotation("Relational:ColumnType", "nvarchar(50)");

                    b.Property<double>("Value")
                        .HasAnnotation("Relational:ColumnType", "float");

                    b.HasKey("SensorDataId");
                });

            modelBuilder.Entity("api.Models.Floor", b =>
                {
                    b.HasOne("api.Models.Building")
                        .WithMany()
                        .HasForeignKey("BuildingBuildingId");
                });

            modelBuilder.Entity("api.Models.Room", b =>
                {
                    b.HasOne("api.Models.Floor")
                        .WithMany()
                        .HasForeignKey("FloorFloorId");
                });

            modelBuilder.Entity("api.Models.SensorData", b =>
                {
                    b.HasOne("api.Models.Room")
                        .WithMany()
                        .HasForeignKey("RoomRoomId");
                });
        }
    }
}
