using System;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;

namespace api.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "Building",
                table => new
                {
                    BuildingId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BuildingName = table.Column<string>(nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_Building", x => x.BuildingId); });
            migrationBuilder.CreateTable(
                "Floor",
                table => new
                {
                    FloorId = table.Column<string>(nullable: false),
                    BuildingBuildingId = table.Column<int>(nullable: true),
                    FloorName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Floor", x => x.FloorId);
                    table.ForeignKey(
                        "FK_Floor_Building_BuildingBuildingId",
                        x => x.BuildingBuildingId,
                        "Building",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });
            migrationBuilder.CreateTable(
                "Room",
                table => new
                {
                    RoomId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FloorFloorId = table.Column<string>(nullable: true),
                    RoomName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Room", x => x.RoomId);
                    table.ForeignKey(
                        "FK_Room_Floor_FloorFloorId",
                        x => x.FloorFloorId,
                        "Floor",
                        "FloorId",
                        onDelete: ReferentialAction.Restrict);
                });
            migrationBuilder.CreateTable(
                "SensorData",
                table => new
                {
                    SensorDataId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Collected = table.Column<DateTime>(nullable: false),
                    RoomRoomId = table.Column<int>(nullable: true),
                    SensorId = table.Column<string>("nvarchar(50)", nullable: true),
                    Type = table.Column<string>("nvarchar(50)", nullable: true),
                    Value = table.Column<double>("float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SensorData", x => x.SensorDataId);
                    table.ForeignKey(
                        "FK_SensorData_Room_RoomRoomId",
                        x => x.RoomRoomId,
                        "Room",
                        "RoomId",
                        onDelete: ReferentialAction.Restrict);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("Room");
            migrationBuilder.DropTable("Floor");
            migrationBuilder.DropTable("Building");
        }
    }
}