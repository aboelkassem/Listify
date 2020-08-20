using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Listify.Domain.CodeFirst.Migrations
{
    public partial class Adjustedcurrency : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Currencies_Rooms_RoomId",
                schema: "Listify",
                table: "Currencies");

            migrationBuilder.DropIndex(
                name: "IX_Currencies_RoomId",
                schema: "Listify",
                table: "Currencies");

            migrationBuilder.DropColumn(
                name: "RoomId",
                schema: "Listify",
                table: "Currencies");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RoomId",
                schema: "Listify",
                table: "Currencies",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Currencies_RoomId",
                schema: "Listify",
                table: "Currencies",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Currencies_Rooms_RoomId",
                schema: "Listify",
                table: "Currencies",
                column: "RoomId",
                principalSchema: "Listify",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
