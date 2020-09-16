using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Listify.Domain.CodeFirst.Migrations
{
    public partial class Includedmaturecontent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "MatureContent",
                schema: "Listify",
                table: "Rooms",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "Currencies",
                keyColumn: "Id",
                keyValue: new Guid("7385db66-c5d6-4f99-84dc-74cf9695a459"),
                columns: new[] { "QuantityIncreasePerTick", "TimeStamp", "Weight" },
                values: new object[] { 2, new DateTime(2020, 9, 15, 17, 16, 0, 628, DateTimeKind.Utc).AddTicks(276), 2 });

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "PurchasableItems",
                keyColumn: "Id",
                keyValue: new Guid("01237025-4ae3-4c73-8ad8-a94c67de8116"),
                column: "TimeStamp",
                value: new DateTime(2020, 9, 15, 17, 16, 0, 629, DateTimeKind.Utc).AddTicks(3388));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "PurchasableItems",
                keyColumn: "Id",
                keyValue: new Guid("018b97a0-c7ea-43e4-9531-3e48dfb3fa6e"),
                column: "TimeStamp",
                value: new DateTime(2020, 9, 15, 17, 16, 0, 629, DateTimeKind.Utc).AddTicks(3381));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "PurchasableItems",
                keyColumn: "Id",
                keyValue: new Guid("2baf4f54-875d-4668-8843-8e765e66eb00"),
                column: "TimeStamp",
                value: new DateTime(2020, 9, 15, 17, 16, 0, 629, DateTimeKind.Utc).AddTicks(2484));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "PurchasableItems",
                keyColumn: "Id",
                keyValue: new Guid("5552e1cc-2a96-4590-b2ca-d3305c229353"),
                column: "TimeStamp",
                value: new DateTime(2020, 9, 15, 17, 16, 0, 629, DateTimeKind.Utc).AddTicks(3376));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "PurchasableItems",
                keyColumn: "Id",
                keyValue: new Guid("58542258-d6b4-490e-835a-3e78ec8c9d2d"),
                column: "TimeStamp",
                value: new DateTime(2020, 9, 15, 17, 16, 0, 629, DateTimeKind.Utc).AddTicks(3348));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "PurchasableItems",
                keyColumn: "Id",
                keyValue: new Guid("5a72bc3c-7494-484d-a30a-5e6a6c698b0d"),
                column: "TimeStamp",
                value: new DateTime(2020, 9, 15, 17, 16, 0, 629, DateTimeKind.Utc).AddTicks(3384));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "PurchasableItems",
                keyColumn: "Id",
                keyValue: new Guid("610dfa85-a0d3-4c36-ab8e-0153ff3742ce"),
                column: "TimeStamp",
                value: new DateTime(2020, 9, 15, 17, 16, 0, 629, DateTimeKind.Utc).AddTicks(3369));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "PurchasableItems",
                keyColumn: "Id",
                keyValue: new Guid("aa147747-3010-4047-8103-b1b50a93bf7f"),
                column: "TimeStamp",
                value: new DateTime(2020, 9, 15, 17, 16, 0, 629, DateTimeKind.Utc).AddTicks(3391));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "PurchasableItems",
                keyColumn: "Id",
                keyValue: new Guid("f217aa5a-a01f-4b7e-891b-b7d9210e8a11"),
                column: "TimeStamp",
                value: new DateTime(2020, 9, 15, 17, 16, 0, 629, DateTimeKind.Utc).AddTicks(3373));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MatureContent",
                schema: "Listify",
                table: "Rooms");

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "Currencies",
                keyColumn: "Id",
                keyValue: new Guid("7385db66-c5d6-4f99-84dc-74cf9695a459"),
                columns: new[] { "QuantityIncreasePerTick", "TimeStamp", "Weight" },
                values: new object[] { 1, new DateTime(2020, 9, 13, 13, 54, 8, 318, DateTimeKind.Utc).AddTicks(6131), 1 });

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "PurchasableItems",
                keyColumn: "Id",
                keyValue: new Guid("01237025-4ae3-4c73-8ad8-a94c67de8116"),
                column: "TimeStamp",
                value: new DateTime(2020, 9, 13, 13, 54, 8, 320, DateTimeKind.Utc).AddTicks(1325));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "PurchasableItems",
                keyColumn: "Id",
                keyValue: new Guid("018b97a0-c7ea-43e4-9531-3e48dfb3fa6e"),
                column: "TimeStamp",
                value: new DateTime(2020, 9, 13, 13, 54, 8, 320, DateTimeKind.Utc).AddTicks(1313));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "PurchasableItems",
                keyColumn: "Id",
                keyValue: new Guid("2baf4f54-875d-4668-8843-8e765e66eb00"),
                column: "TimeStamp",
                value: new DateTime(2020, 9, 13, 13, 54, 8, 320, DateTimeKind.Utc).AddTicks(331));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "PurchasableItems",
                keyColumn: "Id",
                keyValue: new Guid("5552e1cc-2a96-4590-b2ca-d3305c229353"),
                column: "TimeStamp",
                value: new DateTime(2020, 9, 13, 13, 54, 8, 320, DateTimeKind.Utc).AddTicks(1310));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "PurchasableItems",
                keyColumn: "Id",
                keyValue: new Guid("58542258-d6b4-490e-835a-3e78ec8c9d2d"),
                column: "TimeStamp",
                value: new DateTime(2020, 9, 13, 13, 54, 8, 320, DateTimeKind.Utc).AddTicks(1284));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "PurchasableItems",
                keyColumn: "Id",
                keyValue: new Guid("5a72bc3c-7494-484d-a30a-5e6a6c698b0d"),
                column: "TimeStamp",
                value: new DateTime(2020, 9, 13, 13, 54, 8, 320, DateTimeKind.Utc).AddTicks(1322));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "PurchasableItems",
                keyColumn: "Id",
                keyValue: new Guid("610dfa85-a0d3-4c36-ab8e-0153ff3742ce"),
                column: "TimeStamp",
                value: new DateTime(2020, 9, 13, 13, 54, 8, 320, DateTimeKind.Utc).AddTicks(1299));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "PurchasableItems",
                keyColumn: "Id",
                keyValue: new Guid("aa147747-3010-4047-8103-b1b50a93bf7f"),
                column: "TimeStamp",
                value: new DateTime(2020, 9, 13, 13, 54, 8, 320, DateTimeKind.Utc).AddTicks(1340));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "PurchasableItems",
                keyColumn: "Id",
                keyValue: new Guid("f217aa5a-a01f-4b7e-891b-b7d9210e8a11"),
                column: "TimeStamp",
                value: new DateTime(2020, 9, 13, 13, 54, 8, 320, DateTimeKind.Utc).AddTicks(1307));
        }
    }
}
