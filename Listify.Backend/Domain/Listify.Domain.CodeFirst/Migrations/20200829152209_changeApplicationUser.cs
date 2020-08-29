using Microsoft.EntityFrameworkCore.Migrations;

namespace Listify.Domain.CodeFirst.Migrations
{
    public partial class changeApplicationUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SongPoolCountSongsMax",
                schema: "Listify",
                table: "ApplicationUsers");

            migrationBuilder.AddColumn<int>(
                name: "PlaylistSongCount",
                schema: "Listify",
                table: "ApplicationUsers",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlaylistSongCount",
                schema: "Listify",
                table: "ApplicationUsers");

            migrationBuilder.AddColumn<int>(
                name: "SongPoolCountSongsMax",
                schema: "Listify",
                table: "ApplicationUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
