using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;
using Microsoft.Data.Entity.Metadata;

namespace api.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Building",
                columns: table => new
                {
                    BuildingId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BuildingName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Building", x => x.BuildingId);
                });
            migrationBuilder.CreateTable(
                name: "Floor",
                columns: table => new
                {
                    FloorId = table.Column<string>(nullable: false),
                    BuildingBuildingId = table.Column<int>(nullable: true),
                    FloorName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Floor", x => x.FloorId);
                    table.ForeignKey(
                        name: "FK_Floor_Building_BuildingBuildingId",
                        column: x => x.BuildingBuildingId,
                        principalTable: "Building",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });
            migrationBuilder.CreateTable(
                name: "Room",
                columns: table => new
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
                        name: "FK_Room_Floor_FloorFloorId",
                        column: x => x.FloorFloorId,
                        principalTable: "Floor",
                        principalColumn: "FloorId",
                        onDelete: ReferentialAction.Restrict);
                });
            migrationBuilder.CreateTable(
                name: "SensorData",
                columns: table => new
                {
                    SensorDataId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Collected = table.Column<DateTime>(nullable: false),
                    RoomRoomId = table.Column<int>(nullable: true),
                    SensorId = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Value = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SensorData", x => x.SensorDataId);
                    table.ForeignKey(
                        name: "FK_SensorData_Room_RoomRoomId",
                        column: x => x.RoomRoomId,
                        principalTable: "Room",
                        principalColumn: "RoomId",
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
