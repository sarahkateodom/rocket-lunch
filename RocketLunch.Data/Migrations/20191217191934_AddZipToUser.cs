using Microsoft.EntityFrameworkCore.Migrations;

namespace RocketLunch.data.Migrations
{
    public partial class AddZipToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "zip",
                table: "users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "zip",
                table: "users");
        }
    }
}
