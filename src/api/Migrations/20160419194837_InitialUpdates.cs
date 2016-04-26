using Microsoft.Data.Entity.Migrations;

namespace api.Migrations
{
    public partial class InitialUpdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn("RoomName", "Room");
            migrationBuilder.AlterColumn<string>(
                "Type",
                "SensorData",
                "nvarchar(50)",
                nullable: true);
            migrationBuilder.AddColumn<string>(
                "GeoJson",
                "Room",
                nullable: true);
            migrationBuilder.AddColumn<string>(
                "Name",
                "Room",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn("GeoJson", "Room");
            migrationBuilder.DropColumn("Name", "Room");
            migrationBuilder.AlterColumn<string>(
                "Type",
                "SensorData",
                "nvachar(50)",
                nullable: true);
            migrationBuilder.AddColumn<string>(
                "RoomName",
                "Room",
                nullable: true);
        }
    }
}