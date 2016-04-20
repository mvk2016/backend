using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;

namespace api.Migrations
{
    public partial class InitialUpdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "RoomName", table: "Room");
            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "SensorData",
                type: "nvarchar(50)",
                nullable: true);
            migrationBuilder.AddColumn<string>(
                name: "GeoJson",
                table: "Room",
                nullable: true);
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Room",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "GeoJson", table: "Room");
            migrationBuilder.DropColumn(name: "Name", table: "Room");
            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "SensorData",
                type: "nvachar(50)",
                nullable: true);
            migrationBuilder.AddColumn<string>(
                name: "RoomName",
                table: "Room",
                nullable: true);
        }
    }
}
