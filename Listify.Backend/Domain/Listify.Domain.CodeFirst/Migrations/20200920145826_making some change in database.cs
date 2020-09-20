using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Listify.Domain.CodeFirst.Migrations
{
    public partial class makingsomechangeindatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasBeenCharged",
                schema: "Listify",
                table: "Purchases",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "OrderId",
                schema: "Listify",
                table: "Purchases",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PayerId",
                schema: "Listify",
                table: "Purchases",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "WasChargeAccepted",
                schema: "Listify",
                table: "Purchases",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "Currencies",
                keyColumn: "Id",
                keyValue: new Guid("7385db66-c5d6-4f99-84dc-74cf9695a459"),
                columns: new[] { "TimeStamp", "Weight" },
                values: new object[] { new DateTime(2020, 9, 20, 14, 58, 25, 770, DateTimeKind.Utc).AddTicks(7417), 1 });

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "PurchasableItems",
                keyColumn: "Id",
                keyValue: new Guid("01237025-4ae3-4c73-8ad8-a94c67de8116"),
                column: "TimeStamp",
                value: new DateTime(2020, 9, 20, 14, 58, 25, 772, DateTimeKind.Utc).AddTicks(1045));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "PurchasableItems",
                keyColumn: "Id",
                keyValue: new Guid("018b97a0-c7ea-43e4-9531-3e48dfb3fa6e"),
                column: "TimeStamp",
                value: new DateTime(2020, 9, 20, 14, 58, 25, 772, DateTimeKind.Utc).AddTicks(1039));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "PurchasableItems",
                keyColumn: "Id",
                keyValue: new Guid("2baf4f54-875d-4668-8843-8e765e66eb00"),
                column: "TimeStamp",
                value: new DateTime(2020, 9, 20, 14, 58, 25, 772, DateTimeKind.Utc).AddTicks(122));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "PurchasableItems",
                keyColumn: "Id",
                keyValue: new Guid("5552e1cc-2a96-4590-b2ca-d3305c229353"),
                column: "TimeStamp",
                value: new DateTime(2020, 9, 20, 14, 58, 25, 772, DateTimeKind.Utc).AddTicks(1035));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "PurchasableItems",
                keyColumn: "Id",
                keyValue: new Guid("58542258-d6b4-490e-835a-3e78ec8c9d2d"),
                column: "TimeStamp",
                value: new DateTime(2020, 9, 20, 14, 58, 25, 772, DateTimeKind.Utc).AddTicks(1002));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "PurchasableItems",
                keyColumn: "Id",
                keyValue: new Guid("5a72bc3c-7494-484d-a30a-5e6a6c698b0d"),
                column: "TimeStamp",
                value: new DateTime(2020, 9, 20, 14, 58, 25, 772, DateTimeKind.Utc).AddTicks(1042));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "PurchasableItems",
                keyColumn: "Id",
                keyValue: new Guid("610dfa85-a0d3-4c36-ab8e-0153ff3742ce"),
                column: "TimeStamp",
                value: new DateTime(2020, 9, 20, 14, 58, 25, 772, DateTimeKind.Utc).AddTicks(1027));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "PurchasableItems",
                keyColumn: "Id",
                keyValue: new Guid("aa147747-3010-4047-8103-b1b50a93bf7f"),
                column: "TimeStamp",
                value: new DateTime(2020, 9, 20, 14, 58, 25, 772, DateTimeKind.Utc).AddTicks(1049));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "PurchasableItems",
                keyColumn: "Id",
                keyValue: new Guid("f217aa5a-a01f-4b7e-891b-b7d9210e8a11"),
                column: "TimeStamp",
                value: new DateTime(2020, 9, 20, 14, 58, 25, 772, DateTimeKind.Utc).AddTicks(1031));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasBeenCharged",
                schema: "Listify",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "OrderId",
                schema: "Listify",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "PayerId",
                schema: "Listify",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "WasChargeAccepted",
                schema: "Listify",
                table: "Purchases");

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "Currencies",
                keyColumn: "Id",
                keyValue: new Guid("7385db66-c5d6-4f99-84dc-74cf9695a459"),
                columns: new[] { "TimeStamp", "Weight" },
                values: new object[] { new DateTime(2020, 9, 15, 18, 50, 44, 971, DateTimeKind.Utc).AddTicks(4411), 2 });

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "PurchasableItems",
                keyColumn: "Id",
                keyValue: new Guid("01237025-4ae3-4c73-8ad8-a94c67de8116"),
                column: "TimeStamp",
                value: new DateTime(2020, 9, 15, 18, 50, 44, 973, DateTimeKind.Utc).AddTicks(1088));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "PurchasableItems",
                keyColumn: "Id",
                keyValue: new Guid("018b97a0-c7ea-43e4-9531-3e48dfb3fa6e"),
                column: "TimeStamp",
                value: new DateTime(2020, 9, 15, 18, 50, 44, 973, DateTimeKind.Utc).AddTicks(1081));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "PurchasableItems",
                keyColumn: "Id",
                keyValue: new Guid("2baf4f54-875d-4668-8843-8e765e66eb00"),
                column: "TimeStamp",
                value: new DateTime(2020, 9, 15, 18, 50, 44, 973, DateTimeKind.Utc).AddTicks(149));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "PurchasableItems",
                keyColumn: "Id",
                keyValue: new Guid("5552e1cc-2a96-4590-b2ca-d3305c229353"),
                column: "TimeStamp",
                value: new DateTime(2020, 9, 15, 18, 50, 44, 973, DateTimeKind.Utc).AddTicks(1077));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "PurchasableItems",
                keyColumn: "Id",
                keyValue: new Guid("58542258-d6b4-490e-835a-3e78ec8c9d2d"),
                column: "TimeStamp",
                value: new DateTime(2020, 9, 15, 18, 50, 44, 973, DateTimeKind.Utc).AddTicks(1045));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "PurchasableItems",
                keyColumn: "Id",
                keyValue: new Guid("5a72bc3c-7494-484d-a30a-5e6a6c698b0d"),
                column: "TimeStamp",
                value: new DateTime(2020, 9, 15, 18, 50, 44, 973, DateTimeKind.Utc).AddTicks(1085));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "PurchasableItems",
                keyColumn: "Id",
                keyValue: new Guid("610dfa85-a0d3-4c36-ab8e-0153ff3742ce"),
                column: "TimeStamp",
                value: new DateTime(2020, 9, 15, 18, 50, 44, 973, DateTimeKind.Utc).AddTicks(1067));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "PurchasableItems",
                keyColumn: "Id",
                keyValue: new Guid("aa147747-3010-4047-8103-b1b50a93bf7f"),
                column: "TimeStamp",
                value: new DateTime(2020, 9, 15, 18, 50, 44, 973, DateTimeKind.Utc).AddTicks(1092));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "PurchasableItems",
                keyColumn: "Id",
                keyValue: new Guid("f217aa5a-a01f-4b7e-891b-b7d9210e8a11"),
                column: "TimeStamp",
                value: new DateTime(2020, 9, 15, 18, 50, 44, 973, DateTimeKind.Utc).AddTicks(1072));
        }
    }
}
