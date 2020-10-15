using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Listify.Domain.CodeFirst.Migrations
{
    public partial class AddinglistifyschemaforpurchasableLineItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "PurchaseLineItems",
                newName: "PurchaseLineItems",
                newSchema: "Listify");

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "Currencies",
                keyColumn: "Id",
                keyValue: new Guid("7385db66-c5d6-4f99-84dc-74cf9695a459"),
                column: "TimeStamp",
                value: new DateTime(2020, 10, 15, 14, 4, 53, 686, DateTimeKind.Utc).AddTicks(3909));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("39d97940-4b5c-49e9-8335-0dcf74d683cd"),
                column: "TimeStamp",
                value: new DateTime(2020, 10, 15, 14, 4, 53, 688, DateTimeKind.Utc).AddTicks(3728));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("3d709920-731a-4b67-bce2-fb7373a6cd91"),
                column: "TimeStamp",
                value: new DateTime(2020, 10, 15, 14, 4, 53, 688, DateTimeKind.Utc).AddTicks(3658));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("411b13d9-76be-495d-bf8a-41d85133c1f7"),
                column: "TimeStamp",
                value: new DateTime(2020, 10, 15, 14, 4, 53, 688, DateTimeKind.Utc).AddTicks(3704));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("42cebf92-a889-49b1-818f-c3a833f398b5"),
                column: "TimeStamp",
                value: new DateTime(2020, 10, 15, 14, 4, 53, 688, DateTimeKind.Utc).AddTicks(3712));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("53da2fec-6ed2-4c00-bddf-37ef056a266d"),
                column: "TimeStamp",
                value: new DateTime(2020, 10, 15, 14, 4, 53, 688, DateTimeKind.Utc).AddTicks(3709));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("67c40a68-9510-43d0-8c3a-bd24bab7fe74"),
                column: "TimeStamp",
                value: new DateTime(2020, 10, 15, 14, 4, 53, 688, DateTimeKind.Utc).AddTicks(3720));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("68653360-b756-4115-ab67-a19003319a1a"),
                column: "TimeStamp",
                value: new DateTime(2020, 10, 15, 14, 4, 53, 688, DateTimeKind.Utc).AddTicks(3731));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("70992c6e-b333-4cca-af11-370c639eb890"),
                column: "TimeStamp",
                value: new DateTime(2020, 10, 15, 14, 4, 53, 688, DateTimeKind.Utc).AddTicks(3692));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("7933c396-2c1f-4f29-82d2-d5c73d95f377"),
                column: "TimeStamp",
                value: new DateTime(2020, 10, 15, 14, 4, 53, 688, DateTimeKind.Utc).AddTicks(3689));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("7d1e9639-ae88-4033-9076-9ed9491e8d7f"),
                column: "TimeStamp",
                value: new DateTime(2020, 10, 15, 14, 4, 53, 688, DateTimeKind.Utc).AddTicks(3717));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("80f2c6b5-8be7-444c-b8ad-290af496d6cc"),
                column: "TimeStamp",
                value: new DateTime(2020, 10, 15, 14, 4, 53, 688, DateTimeKind.Utc).AddTicks(3715));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("8593f261-6b15-40ff-8dcc-9047bda6ff1e"),
                column: "TimeStamp",
                value: new DateTime(2020, 10, 15, 14, 4, 53, 688, DateTimeKind.Utc).AddTicks(3706));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("a3d7343b-1afa-4b82-bad5-6f8e4ec55fc7"),
                column: "TimeStamp",
                value: new DateTime(2020, 10, 15, 14, 4, 53, 688, DateTimeKind.Utc).AddTicks(3687));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("adc9b474-5336-45e5-9e95-37487939d2c5"),
                column: "TimeStamp",
                value: new DateTime(2020, 10, 15, 14, 4, 53, 688, DateTimeKind.Utc).AddTicks(3702));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("b35340b7-e99c-47c1-bd7e-44113428045b"),
                column: "TimeStamp",
                value: new DateTime(2020, 10, 15, 14, 4, 53, 688, DateTimeKind.Utc).AddTicks(3726));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("bc00378e-60da-46f5-ae42-bcd1abebc5eb"),
                column: "TimeStamp",
                value: new DateTime(2020, 10, 15, 14, 4, 53, 688, DateTimeKind.Utc).AddTicks(3698));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("d314f793-703a-4387-90ba-d8c420a8176f"),
                column: "TimeStamp",
                value: new DateTime(2020, 10, 15, 14, 4, 53, 688, DateTimeKind.Utc).AddTicks(3695));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("d3c3cfba-d1f6-49b2-82ff-3d96b3f4ef5f"),
                column: "TimeStamp",
                value: new DateTime(2020, 10, 15, 14, 4, 53, 688, DateTimeKind.Utc).AddTicks(3683));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("d51cca64-3ad4-4026-9bde-9821fa525e95"),
                column: "TimeStamp",
                value: new DateTime(2020, 10, 15, 14, 4, 53, 688, DateTimeKind.Utc).AddTicks(3723));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("daf33262-443f-46e5-b12b-9bf8f4e4210d"),
                column: "TimeStamp",
                value: new DateTime(2020, 10, 15, 14, 4, 53, 688, DateTimeKind.Utc).AddTicks(3188));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "PurchasableItems",
                keyColumn: "Id",
                keyValue: new Guid("01237025-4ae3-4c73-8ad8-a94c67de8116"),
                column: "TimeStamp",
                value: new DateTime(2020, 10, 15, 14, 4, 53, 688, DateTimeKind.Utc).AddTicks(2042));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "PurchasableItems",
                keyColumn: "Id",
                keyValue: new Guid("018b97a0-c7ea-43e4-9531-3e48dfb3fa6e"),
                column: "TimeStamp",
                value: new DateTime(2020, 10, 15, 14, 4, 53, 688, DateTimeKind.Utc).AddTicks(2034));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "PurchasableItems",
                keyColumn: "Id",
                keyValue: new Guid("2baf4f54-875d-4668-8843-8e765e66eb00"),
                column: "TimeStamp",
                value: new DateTime(2020, 10, 15, 14, 4, 53, 688, DateTimeKind.Utc).AddTicks(1169));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "PurchasableItems",
                keyColumn: "Id",
                keyValue: new Guid("5552e1cc-2a96-4590-b2ca-d3305c229353"),
                column: "TimeStamp",
                value: new DateTime(2020, 10, 15, 14, 4, 53, 688, DateTimeKind.Utc).AddTicks(2030));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "PurchasableItems",
                keyColumn: "Id",
                keyValue: new Guid("58542258-d6b4-490e-835a-3e78ec8c9d2d"),
                column: "TimeStamp",
                value: new DateTime(2020, 10, 15, 14, 4, 53, 688, DateTimeKind.Utc).AddTicks(2002));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "PurchasableItems",
                keyColumn: "Id",
                keyValue: new Guid("5a72bc3c-7494-484d-a30a-5e6a6c698b0d"),
                column: "TimeStamp",
                value: new DateTime(2020, 10, 15, 14, 4, 53, 688, DateTimeKind.Utc).AddTicks(2039));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "PurchasableItems",
                keyColumn: "Id",
                keyValue: new Guid("610dfa85-a0d3-4c36-ab8e-0153ff3742ce"),
                column: "TimeStamp",
                value: new DateTime(2020, 10, 15, 14, 4, 53, 688, DateTimeKind.Utc).AddTicks(2021));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "PurchasableItems",
                keyColumn: "Id",
                keyValue: new Guid("aa147747-3010-4047-8103-b1b50a93bf7f"),
                column: "TimeStamp",
                value: new DateTime(2020, 10, 15, 14, 4, 53, 688, DateTimeKind.Utc).AddTicks(2046));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "PurchasableItems",
                keyColumn: "Id",
                keyValue: new Guid("f217aa5a-a01f-4b7e-891b-b7d9210e8a11"),
                column: "TimeStamp",
                value: new DateTime(2020, 10, 15, 14, 4, 53, 688, DateTimeKind.Utc).AddTicks(2026));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "Songs",
                keyColumn: "Id",
                keyValue: new Guid("24c294bf-2bef-404c-b007-076deea68401"),
                columns: new[] { "ThumbnailHeight", "ThumbnailUrl", "ThumbnailWidth", "TimeStamp" },
                values: new object[] { 90L, "https://i.ytimg.com/vi/owQ5YZrleGU/default.jpg", 120L, new DateTime(2020, 10, 15, 14, 4, 53, 687, DateTimeKind.Utc).AddTicks(7425) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "PurchaseLineItems",
                schema: "Listify",
                newName: "PurchaseLineItems");

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "Currencies",
                keyColumn: "Id",
                keyValue: new Guid("7385db66-c5d6-4f99-84dc-74cf9695a459"),
                column: "TimeStamp",
                value: new DateTime(2020, 10, 13, 12, 30, 0, 149, DateTimeKind.Utc).AddTicks(3323));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("39d97940-4b5c-49e9-8335-0dcf74d683cd"),
                column: "TimeStamp",
                value: new DateTime(2020, 10, 13, 12, 30, 0, 151, DateTimeKind.Utc).AddTicks(1967));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("3d709920-731a-4b67-bce2-fb7373a6cd91"),
                column: "TimeStamp",
                value: new DateTime(2020, 10, 13, 12, 30, 0, 151, DateTimeKind.Utc).AddTicks(1898));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("411b13d9-76be-495d-bf8a-41d85133c1f7"),
                column: "TimeStamp",
                value: new DateTime(2020, 10, 13, 12, 30, 0, 151, DateTimeKind.Utc).AddTicks(1943));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("42cebf92-a889-49b1-818f-c3a833f398b5"),
                column: "TimeStamp",
                value: new DateTime(2020, 10, 13, 12, 30, 0, 151, DateTimeKind.Utc).AddTicks(1950));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("53da2fec-6ed2-4c00-bddf-37ef056a266d"),
                column: "TimeStamp",
                value: new DateTime(2020, 10, 13, 12, 30, 0, 151, DateTimeKind.Utc).AddTicks(1948));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("67c40a68-9510-43d0-8c3a-bd24bab7fe74"),
                column: "TimeStamp",
                value: new DateTime(2020, 10, 13, 12, 30, 0, 151, DateTimeKind.Utc).AddTicks(1959));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("68653360-b756-4115-ab67-a19003319a1a"),
                column: "TimeStamp",
                value: new DateTime(2020, 10, 13, 12, 30, 0, 151, DateTimeKind.Utc).AddTicks(1970));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("70992c6e-b333-4cca-af11-370c639eb890"),
                column: "TimeStamp",
                value: new DateTime(2020, 10, 13, 12, 30, 0, 151, DateTimeKind.Utc).AddTicks(1930));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("7933c396-2c1f-4f29-82d2-d5c73d95f377"),
                column: "TimeStamp",
                value: new DateTime(2020, 10, 13, 12, 30, 0, 151, DateTimeKind.Utc).AddTicks(1928));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("7d1e9639-ae88-4033-9076-9ed9491e8d7f"),
                column: "TimeStamp",
                value: new DateTime(2020, 10, 13, 12, 30, 0, 151, DateTimeKind.Utc).AddTicks(1957));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("80f2c6b5-8be7-444c-b8ad-290af496d6cc"),
                column: "TimeStamp",
                value: new DateTime(2020, 10, 13, 12, 30, 0, 151, DateTimeKind.Utc).AddTicks(1954));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("8593f261-6b15-40ff-8dcc-9047bda6ff1e"),
                column: "TimeStamp",
                value: new DateTime(2020, 10, 13, 12, 30, 0, 151, DateTimeKind.Utc).AddTicks(1945));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("a3d7343b-1afa-4b82-bad5-6f8e4ec55fc7"),
                column: "TimeStamp",
                value: new DateTime(2020, 10, 13, 12, 30, 0, 151, DateTimeKind.Utc).AddTicks(1925));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("adc9b474-5336-45e5-9e95-37487939d2c5"),
                column: "TimeStamp",
                value: new DateTime(2020, 10, 13, 12, 30, 0, 151, DateTimeKind.Utc).AddTicks(1940));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("b35340b7-e99c-47c1-bd7e-44113428045b"),
                column: "TimeStamp",
                value: new DateTime(2020, 10, 13, 12, 30, 0, 151, DateTimeKind.Utc).AddTicks(1964));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("bc00378e-60da-46f5-ae42-bcd1abebc5eb"),
                column: "TimeStamp",
                value: new DateTime(2020, 10, 13, 12, 30, 0, 151, DateTimeKind.Utc).AddTicks(1938));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("d314f793-703a-4387-90ba-d8c420a8176f"),
                column: "TimeStamp",
                value: new DateTime(2020, 10, 13, 12, 30, 0, 151, DateTimeKind.Utc).AddTicks(1935));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("d3c3cfba-d1f6-49b2-82ff-3d96b3f4ef5f"),
                column: "TimeStamp",
                value: new DateTime(2020, 10, 13, 12, 30, 0, 151, DateTimeKind.Utc).AddTicks(1922));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("d51cca64-3ad4-4026-9bde-9821fa525e95"),
                column: "TimeStamp",
                value: new DateTime(2020, 10, 13, 12, 30, 0, 151, DateTimeKind.Utc).AddTicks(1962));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("daf33262-443f-46e5-b12b-9bf8f4e4210d"),
                column: "TimeStamp",
                value: new DateTime(2020, 10, 13, 12, 30, 0, 151, DateTimeKind.Utc).AddTicks(1416));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "PurchasableItems",
                keyColumn: "Id",
                keyValue: new Guid("01237025-4ae3-4c73-8ad8-a94c67de8116"),
                column: "TimeStamp",
                value: new DateTime(2020, 10, 13, 12, 30, 0, 151, DateTimeKind.Utc).AddTicks(152));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "PurchasableItems",
                keyColumn: "Id",
                keyValue: new Guid("018b97a0-c7ea-43e4-9531-3e48dfb3fa6e"),
                column: "TimeStamp",
                value: new DateTime(2020, 10, 13, 12, 30, 0, 151, DateTimeKind.Utc).AddTicks(144));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "PurchasableItems",
                keyColumn: "Id",
                keyValue: new Guid("2baf4f54-875d-4668-8843-8e765e66eb00"),
                column: "TimeStamp",
                value: new DateTime(2020, 10, 13, 12, 30, 0, 150, DateTimeKind.Utc).AddTicks(9269));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "PurchasableItems",
                keyColumn: "Id",
                keyValue: new Guid("5552e1cc-2a96-4590-b2ca-d3305c229353"),
                column: "TimeStamp",
                value: new DateTime(2020, 10, 13, 12, 30, 0, 151, DateTimeKind.Utc).AddTicks(141));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "PurchasableItems",
                keyColumn: "Id",
                keyValue: new Guid("58542258-d6b4-490e-835a-3e78ec8c9d2d"),
                column: "TimeStamp",
                value: new DateTime(2020, 10, 13, 12, 30, 0, 151, DateTimeKind.Utc).AddTicks(112));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "PurchasableItems",
                keyColumn: "Id",
                keyValue: new Guid("5a72bc3c-7494-484d-a30a-5e6a6c698b0d"),
                column: "TimeStamp",
                value: new DateTime(2020, 10, 13, 12, 30, 0, 151, DateTimeKind.Utc).AddTicks(148));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "PurchasableItems",
                keyColumn: "Id",
                keyValue: new Guid("610dfa85-a0d3-4c36-ab8e-0153ff3742ce"),
                column: "TimeStamp",
                value: new DateTime(2020, 10, 13, 12, 30, 0, 151, DateTimeKind.Utc).AddTicks(132));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "PurchasableItems",
                keyColumn: "Id",
                keyValue: new Guid("aa147747-3010-4047-8103-b1b50a93bf7f"),
                column: "TimeStamp",
                value: new DateTime(2020, 10, 13, 12, 30, 0, 151, DateTimeKind.Utc).AddTicks(155));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "PurchasableItems",
                keyColumn: "Id",
                keyValue: new Guid("f217aa5a-a01f-4b7e-891b-b7d9210e8a11"),
                column: "TimeStamp",
                value: new DateTime(2020, 10, 13, 12, 30, 0, 151, DateTimeKind.Utc).AddTicks(136));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "Songs",
                keyColumn: "Id",
                keyValue: new Guid("24c294bf-2bef-404c-b007-076deea68401"),
                columns: new[] { "ThumbnailHeight", "ThumbnailUrl", "ThumbnailWidth", "TimeStamp" },
                values: new object[] { null, null, null, new DateTime(2020, 10, 13, 12, 30, 0, 150, DateTimeKind.Utc).AddTicks(6895) });
        }
    }
}
