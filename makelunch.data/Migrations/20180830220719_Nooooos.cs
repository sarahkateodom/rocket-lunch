using Microsoft.EntityFrameworkCore.Migrations;

namespace makelunch.data.Migrations
{
    public partial class Nooooos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "avatar_url",
                table: "users",
                newName: "nopes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "nopes",
                table: "users",
                newName: "avatar_url");
        }
    }
}
