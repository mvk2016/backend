using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;

namespace api.Migrations
{
    public partial class Renames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey("PK_Floor", "Floor");
            migrationBuilder.AddColumn<int>(
                "Id",
                "Floor",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey("PK_Floor", "Floor");
            migrationBuilder.AddColumn<int>(
                "FloorId",
                "Floor",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
        }
    }
}