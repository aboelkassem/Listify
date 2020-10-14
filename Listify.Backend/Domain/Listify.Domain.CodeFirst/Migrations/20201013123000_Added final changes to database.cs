using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Listify.Domain.CodeFirst.Migrations
{
    public partial class Addedfinalchangestodatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUsersRoomsCurrenciesRoom_CurrenciesRoom_CurrencyRoomId",
                schema: "Listify",
                table: "ApplicationUsersRoomsCurrenciesRoom");

            migrationBuilder.DropForeignKey(
                name: "FK_CurrenciesRoom_Currencies_CurrencyId",
                schema: "Listify",
                table: "CurrenciesRoom");

            migrationBuilder.DropForeignKey(
                name: "FK_CurrenciesRoom_Rooms_RoomId",
                schema: "Listify",
                table: "CurrenciesRoom");

            migrationBuilder.DropColumn(
                name: "QuantityChange",
                schema: "Listify",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "QueueCount",
                schema: "Listify",
                table: "ApplicationUsers");

            migrationBuilder.AddColumn<bool>(
                name: "HasBeenRefunded",
                schema: "Listify",
                table: "Transactions",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "QuantityChanged",
                schema: "Listify",
                table: "Transactions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "RefundTimestamp",
                schema: "Listify",
                table: "Transactions",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "YoutubeId",
                schema: "Listify",
                table: "Songs",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ThumbnailHeight",
                schema: "Listify",
                table: "Songs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ThumbnailUrl",
                schema: "Listify",
                table: "Songs",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ThumbnailWidth",
                schema: "Listify",
                table: "Songs",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasBeenSkipped",
                schema: "Listify",
                table: "SongRequests",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TimestampSkipped",
                schema: "Listify",
                table: "SongRequests",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsRoomPlaying",
                schema: "Listify",
                table: "Rooms",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "RoomImageUrl",
                schema: "Listify",
                table: "Rooms",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PlaylistImageUrl",
                schema: "Listify",
                table: "Playlists",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfileDescription",
                schema: "Listify",
                table: "ApplicationUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfileImageUrl",
                schema: "Listify",
                table: "ApplicationUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfileTitle",
                schema: "Listify",
                table: "ApplicationUsers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QueueSongsCount",
                schema: "Listify",
                table: "ApplicationUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Follows",
                schema: "Listify",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    RoomId = table.Column<Guid>(nullable: false),
                    ApplicationUserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Follows", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Follows_ApplicationUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalSchema: "Listify",
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Follows_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalSchema: "Listify",
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RoomsGenres",
                schema: "Listify",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    RoomId = table.Column<Guid>(nullable: false),
                    GenreId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomsGenres", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoomsGenres_Genres_GenreId",
                        column: x => x.GenreId,
                        principalSchema: "Listify",
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoomsGenres_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalSchema: "Listify",
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                column: "TimeStamp",
                value: new DateTime(2020, 10, 13, 12, 30, 0, 150, DateTimeKind.Utc).AddTicks(6895));

            migrationBuilder.CreateIndex(
                name: "IX_Songs_YoutubeId",
                schema: "Listify",
                table: "Songs",
                column: "YoutubeId",
                unique: true,
                filter: "[YoutubeId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Follows_ApplicationUserId",
                schema: "Listify",
                table: "Follows",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Follows_RoomId",
                schema: "Listify",
                table: "Follows",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomsGenres_GenreId",
                schema: "Listify",
                table: "RoomsGenres",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomsGenres_RoomId",
                schema: "Listify",
                table: "RoomsGenres",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUsersRoomsCurrenciesRoom_CurrenciesRoom_CurrencyRoomId",
                schema: "Listify",
                table: "ApplicationUsersRoomsCurrenciesRoom",
                column: "CurrencyRoomId",
                principalSchema: "Listify",
                principalTable: "CurrenciesRoom",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CurrenciesRoom_Currencies_CurrencyId",
                schema: "Listify",
                table: "CurrenciesRoom",
                column: "CurrencyId",
                principalSchema: "Listify",
                principalTable: "Currencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CurrenciesRoom_Rooms_RoomId",
                schema: "Listify",
                table: "CurrenciesRoom",
                column: "RoomId",
                principalSchema: "Listify",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUsersRoomsCurrenciesRoom_CurrenciesRoom_CurrencyRoomId",
                schema: "Listify",
                table: "ApplicationUsersRoomsCurrenciesRoom");

            migrationBuilder.DropForeignKey(
                name: "FK_CurrenciesRoom_Currencies_CurrencyId",
                schema: "Listify",
                table: "CurrenciesRoom");

            migrationBuilder.DropForeignKey(
                name: "FK_CurrenciesRoom_Rooms_RoomId",
                schema: "Listify",
                table: "CurrenciesRoom");

            migrationBuilder.DropTable(
                name: "Follows",
                schema: "Listify");

            migrationBuilder.DropTable(
                name: "RoomsGenres",
                schema: "Listify");

            migrationBuilder.DropIndex(
                name: "IX_Songs_YoutubeId",
                schema: "Listify",
                table: "Songs");

            migrationBuilder.DropColumn(
                name: "HasBeenRefunded",
                schema: "Listify",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "QuantityChanged",
                schema: "Listify",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "RefundTimestamp",
                schema: "Listify",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "ThumbnailHeight",
                schema: "Listify",
                table: "Songs");

            migrationBuilder.DropColumn(
                name: "ThumbnailUrl",
                schema: "Listify",
                table: "Songs");

            migrationBuilder.DropColumn(
                name: "ThumbnailWidth",
                schema: "Listify",
                table: "Songs");

            migrationBuilder.DropColumn(
                name: "HasBeenSkipped",
                schema: "Listify",
                table: "SongRequests");

            migrationBuilder.DropColumn(
                name: "TimestampSkipped",
                schema: "Listify",
                table: "SongRequests");

            migrationBuilder.DropColumn(
                name: "IsRoomPlaying",
                schema: "Listify",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "RoomImageUrl",
                schema: "Listify",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "PlaylistImageUrl",
                schema: "Listify",
                table: "Playlists");

            migrationBuilder.DropColumn(
                name: "ProfileDescription",
                schema: "Listify",
                table: "ApplicationUsers");

            migrationBuilder.DropColumn(
                name: "ProfileImageUrl",
                schema: "Listify",
                table: "ApplicationUsers");

            migrationBuilder.DropColumn(
                name: "ProfileTitle",
                schema: "Listify",
                table: "ApplicationUsers");

            migrationBuilder.DropColumn(
                name: "QueueSongsCount",
                schema: "Listify",
                table: "ApplicationUsers");

            migrationBuilder.AddColumn<int>(
                name: "QuantityChange",
                schema: "Listify",
                table: "Transactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "YoutubeId",
                schema: "Listify",
                table: "Songs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QueueCount",
                schema: "Listify",
                table: "ApplicationUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "Currencies",
                keyColumn: "Id",
                keyValue: new Guid("7385db66-c5d6-4f99-84dc-74cf9695a459"),
                column: "TimeStamp",
                value: new DateTime(2020, 9, 26, 13, 43, 30, 712, DateTimeKind.Utc).AddTicks(3375));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("39d97940-4b5c-49e9-8335-0dcf74d683cd"),
                column: "TimeStamp",
                value: new DateTime(2020, 9, 26, 13, 43, 30, 714, DateTimeKind.Utc).AddTicks(648));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("3d709920-731a-4b67-bce2-fb7373a6cd91"),
                column: "TimeStamp",
                value: new DateTime(2020, 9, 26, 13, 43, 30, 714, DateTimeKind.Utc).AddTicks(584));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("411b13d9-76be-495d-bf8a-41d85133c1f7"),
                column: "TimeStamp",
                value: new DateTime(2020, 9, 26, 13, 43, 30, 714, DateTimeKind.Utc).AddTicks(616));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("42cebf92-a889-49b1-818f-c3a833f398b5"),
                column: "TimeStamp",
                value: new DateTime(2020, 9, 26, 13, 43, 30, 714, DateTimeKind.Utc).AddTicks(633));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("53da2fec-6ed2-4c00-bddf-37ef056a266d"),
                column: "TimeStamp",
                value: new DateTime(2020, 9, 26, 13, 43, 30, 714, DateTimeKind.Utc).AddTicks(622));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("67c40a68-9510-43d0-8c3a-bd24bab7fe74"),
                column: "TimeStamp",
                value: new DateTime(2020, 9, 26, 13, 43, 30, 714, DateTimeKind.Utc).AddTicks(640));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("68653360-b756-4115-ab67-a19003319a1a"),
                column: "TimeStamp",
                value: new DateTime(2020, 9, 26, 13, 43, 30, 714, DateTimeKind.Utc).AddTicks(651));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("70992c6e-b333-4cca-af11-370c639eb890"),
                column: "TimeStamp",
                value: new DateTime(2020, 9, 26, 13, 43, 30, 714, DateTimeKind.Utc).AddTicks(606));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("7933c396-2c1f-4f29-82d2-d5c73d95f377"),
                column: "TimeStamp",
                value: new DateTime(2020, 9, 26, 13, 43, 30, 714, DateTimeKind.Utc).AddTicks(604));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("7d1e9639-ae88-4033-9076-9ed9491e8d7f"),
                column: "TimeStamp",
                value: new DateTime(2020, 9, 26, 13, 43, 30, 714, DateTimeKind.Utc).AddTicks(638));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("80f2c6b5-8be7-444c-b8ad-290af496d6cc"),
                column: "TimeStamp",
                value: new DateTime(2020, 9, 26, 13, 43, 30, 714, DateTimeKind.Utc).AddTicks(635));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("8593f261-6b15-40ff-8dcc-9047bda6ff1e"),
                column: "TimeStamp",
                value: new DateTime(2020, 9, 26, 13, 43, 30, 714, DateTimeKind.Utc).AddTicks(620));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("a3d7343b-1afa-4b82-bad5-6f8e4ec55fc7"),
                column: "TimeStamp",
                value: new DateTime(2020, 9, 26, 13, 43, 30, 714, DateTimeKind.Utc).AddTicks(602));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("adc9b474-5336-45e5-9e95-37487939d2c5"),
                column: "TimeStamp",
                value: new DateTime(2020, 9, 26, 13, 43, 30, 714, DateTimeKind.Utc).AddTicks(614));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("b35340b7-e99c-47c1-bd7e-44113428045b"),
                column: "TimeStamp",
                value: new DateTime(2020, 9, 26, 13, 43, 30, 714, DateTimeKind.Utc).AddTicks(645));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("bc00378e-60da-46f5-ae42-bcd1abebc5eb"),
                column: "TimeStamp",
                value: new DateTime(2020, 9, 26, 13, 43, 30, 714, DateTimeKind.Utc).AddTicks(612));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("d314f793-703a-4387-90ba-d8c420a8176f"),
                column: "TimeStamp",
                value: new DateTime(2020, 9, 26, 13, 43, 30, 714, DateTimeKind.Utc).AddTicks(609));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("d3c3cfba-d1f6-49b2-82ff-3d96b3f4ef5f"),
                column: "TimeStamp",
                value: new DateTime(2020, 9, 26, 13, 43, 30, 714, DateTimeKind.Utc).AddTicks(599));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("d51cca64-3ad4-4026-9bde-9821fa525e95"),
                column: "TimeStamp",
                value: new DateTime(2020, 9, 26, 13, 43, 30, 714, DateTimeKind.Utc).AddTicks(643));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("daf33262-443f-46e5-b12b-9bf8f4e4210d"),
                column: "TimeStamp",
                value: new DateTime(2020, 9, 26, 13, 43, 30, 714, DateTimeKind.Utc).AddTicks(162));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "PurchasableItems",
                keyColumn: "Id",
                keyValue: new Guid("01237025-4ae3-4c73-8ad8-a94c67de8116"),
                column: "TimeStamp",
                value: new DateTime(2020, 9, 26, 13, 43, 30, 713, DateTimeKind.Utc).AddTicks(9249));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "PurchasableItems",
                keyColumn: "Id",
                keyValue: new Guid("018b97a0-c7ea-43e4-9531-3e48dfb3fa6e"),
                column: "TimeStamp",
                value: new DateTime(2020, 9, 26, 13, 43, 30, 713, DateTimeKind.Utc).AddTicks(9241));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "PurchasableItems",
                keyColumn: "Id",
                keyValue: new Guid("2baf4f54-875d-4668-8843-8e765e66eb00"),
                column: "TimeStamp",
                value: new DateTime(2020, 9, 26, 13, 43, 30, 713, DateTimeKind.Utc).AddTicks(8417));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "PurchasableItems",
                keyColumn: "Id",
                keyValue: new Guid("5552e1cc-2a96-4590-b2ca-d3305c229353"),
                column: "TimeStamp",
                value: new DateTime(2020, 9, 26, 13, 43, 30, 713, DateTimeKind.Utc).AddTicks(9238));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "PurchasableItems",
                keyColumn: "Id",
                keyValue: new Guid("58542258-d6b4-490e-835a-3e78ec8c9d2d"),
                column: "TimeStamp",
                value: new DateTime(2020, 9, 26, 13, 43, 30, 713, DateTimeKind.Utc).AddTicks(9209));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "PurchasableItems",
                keyColumn: "Id",
                keyValue: new Guid("5a72bc3c-7494-484d-a30a-5e6a6c698b0d"),
                column: "TimeStamp",
                value: new DateTime(2020, 9, 26, 13, 43, 30, 713, DateTimeKind.Utc).AddTicks(9245));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "PurchasableItems",
                keyColumn: "Id",
                keyValue: new Guid("610dfa85-a0d3-4c36-ab8e-0153ff3742ce"),
                column: "TimeStamp",
                value: new DateTime(2020, 9, 26, 13, 43, 30, 713, DateTimeKind.Utc).AddTicks(9230));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "PurchasableItems",
                keyColumn: "Id",
                keyValue: new Guid("aa147747-3010-4047-8103-b1b50a93bf7f"),
                column: "TimeStamp",
                value: new DateTime(2020, 9, 26, 13, 43, 30, 713, DateTimeKind.Utc).AddTicks(9253));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "PurchasableItems",
                keyColumn: "Id",
                keyValue: new Guid("f217aa5a-a01f-4b7e-891b-b7d9210e8a11"),
                column: "TimeStamp",
                value: new DateTime(2020, 9, 26, 13, 43, 30, 713, DateTimeKind.Utc).AddTicks(9234));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "Songs",
                keyColumn: "Id",
                keyValue: new Guid("24c294bf-2bef-404c-b007-076deea68401"),
                column: "TimeStamp",
                value: new DateTime(2020, 9, 26, 13, 43, 30, 713, DateTimeKind.Utc).AddTicks(6134));

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUsersRoomsCurrenciesRoom_CurrenciesRoom_CurrencyRoomId",
                schema: "Listify",
                table: "ApplicationUsersRoomsCurrenciesRoom",
                column: "CurrencyRoomId",
                principalSchema: "Listify",
                principalTable: "CurrenciesRoom",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CurrenciesRoom_Currencies_CurrencyId",
                schema: "Listify",
                table: "CurrenciesRoom",
                column: "CurrencyId",
                principalSchema: "Listify",
                principalTable: "Currencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CurrenciesRoom_Rooms_RoomId",
                schema: "Listify",
                table: "CurrenciesRoom",
                column: "RoomId",
                principalSchema: "Listify",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
