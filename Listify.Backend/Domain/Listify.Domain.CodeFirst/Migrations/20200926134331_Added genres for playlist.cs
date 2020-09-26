using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Listify.Domain.CodeFirst.Migrations
{
    public partial class Addedgenresforplaylist : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                schema: "Listify",
                table: "Playlists",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Genres",
                schema: "Listify",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlaylistGenres",
                schema: "Listify",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    PlaylistId = table.Column<Guid>(nullable: false),
                    GenreId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaylistGenres", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlaylistGenres_Genres_GenreId",
                        column: x => x.GenreId,
                        principalSchema: "Listify",
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlaylistGenres_Playlists_PlaylistId",
                        column: x => x.PlaylistId,
                        principalSchema: "Listify",
                        principalTable: "Playlists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "Currencies",
                keyColumn: "Id",
                keyValue: new Guid("7385db66-c5d6-4f99-84dc-74cf9695a459"),
                columns: new[] { "TimeSecBetweenTick", "TimeStamp" },
                values: new object[] { 60f, new DateTime(2020, 9, 26, 13, 43, 30, 712, DateTimeKind.Utc).AddTicks(3375) });

            migrationBuilder.InsertData(
                schema: "Listify",
                table: "Genres",
                columns: new[] { "Id", "Active", "Name", "TimeStamp" },
                values: new object[,]
                {
                    { new Guid("68653360-b756-4115-ab67-a19003319a1a"), true, "Trap", new DateTime(2020, 9, 26, 13, 43, 30, 714, DateTimeKind.Utc).AddTicks(651) },
                    { new Guid("39d97940-4b5c-49e9-8335-0dcf74d683cd"), true, "Trance", new DateTime(2020, 9, 26, 13, 43, 30, 714, DateTimeKind.Utc).AddTicks(648) },
                    { new Guid("b35340b7-e99c-47c1-bd7e-44113428045b"), true, "Techno", new DateTime(2020, 9, 26, 13, 43, 30, 714, DateTimeKind.Utc).AddTicks(645) },
                    { new Guid("d51cca64-3ad4-4026-9bde-9821fa525e95"), true, "R&B", new DateTime(2020, 9, 26, 13, 43, 30, 714, DateTimeKind.Utc).AddTicks(643) },
                    { new Guid("7d1e9639-ae88-4033-9076-9ed9491e8d7f"), true, "Reggae", new DateTime(2020, 9, 26, 13, 43, 30, 714, DateTimeKind.Utc).AddTicks(638) },
                    { new Guid("80f2c6b5-8be7-444c-b8ad-290af496d6cc"), true, "Pop", new DateTime(2020, 9, 26, 13, 43, 30, 714, DateTimeKind.Utc).AddTicks(635) },
                    { new Guid("42cebf92-a889-49b1-818f-c3a833f398b5"), true, "Metal", new DateTime(2020, 9, 26, 13, 43, 30, 714, DateTimeKind.Utc).AddTicks(633) },
                    { new Guid("53da2fec-6ed2-4c00-bddf-37ef056a266d"), true, "Latin", new DateTime(2020, 9, 26, 13, 43, 30, 714, DateTimeKind.Utc).AddTicks(622) },
                    { new Guid("8593f261-6b15-40ff-8dcc-9047bda6ff1e"), true, "Indie", new DateTime(2020, 9, 26, 13, 43, 30, 714, DateTimeKind.Utc).AddTicks(620) },
                    { new Guid("67c40a68-9510-43d0-8c3a-bd24bab7fe74"), true, "Rock", new DateTime(2020, 9, 26, 13, 43, 30, 714, DateTimeKind.Utc).AddTicks(640) },
                    { new Guid("adc9b474-5336-45e5-9e95-37487939d2c5"), true, "Funk", new DateTime(2020, 9, 26, 13, 43, 30, 714, DateTimeKind.Utc).AddTicks(614) },
                    { new Guid("411b13d9-76be-495d-bf8a-41d85133c1f7"), true, "Hip Hop", new DateTime(2020, 9, 26, 13, 43, 30, 714, DateTimeKind.Utc).AddTicks(616) },
                    { new Guid("3d709920-731a-4b67-bce2-fb7373a6cd91"), true, "Blues and Jazz", new DateTime(2020, 9, 26, 13, 43, 30, 714, DateTimeKind.Utc).AddTicks(584) },
                    { new Guid("d3c3cfba-d1f6-49b2-82ff-3d96b3f4ef5f"), true, "Chill", new DateTime(2020, 9, 26, 13, 43, 30, 714, DateTimeKind.Utc).AddTicks(599) },
                    { new Guid("a3d7343b-1afa-4b82-bad5-6f8e4ec55fc7"), true, "Classical", new DateTime(2020, 9, 26, 13, 43, 30, 714, DateTimeKind.Utc).AddTicks(602) },
                    { new Guid("daf33262-443f-46e5-b12b-9bf8f4e4210d"), true, "Ambient", new DateTime(2020, 9, 26, 13, 43, 30, 714, DateTimeKind.Utc).AddTicks(162) },
                    { new Guid("70992c6e-b333-4cca-af11-370c639eb890"), true, "Dubstep", new DateTime(2020, 9, 26, 13, 43, 30, 714, DateTimeKind.Utc).AddTicks(606) },
                    { new Guid("d314f793-703a-4387-90ba-d8c420a8176f"), true, "Electronica", new DateTime(2020, 9, 26, 13, 43, 30, 714, DateTimeKind.Utc).AddTicks(609) },
                    { new Guid("bc00378e-60da-46f5-ae42-bcd1abebc5eb"), true, "Folk", new DateTime(2020, 9, 26, 13, 43, 30, 714, DateTimeKind.Utc).AddTicks(612) },
                    { new Guid("7933c396-2c1f-4f29-82d2-d5c73d95f377"), true, "Country", new DateTime(2020, 9, 26, 13, 43, 30, 714, DateTimeKind.Utc).AddTicks(604) }
                });

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

            migrationBuilder.InsertData(
                schema: "Listify",
                table: "Songs",
                columns: new[] { "Id", "Active", "SongLengthSeconds", "SongName", "TimeStamp", "YoutubeId" },
                values: new object[] { new Guid("24c294bf-2bef-404c-b007-076deea68401"), true, 94, "Spotify Ad Studio", new DateTime(2020, 9, 26, 13, 43, 30, 713, DateTimeKind.Utc).AddTicks(6134), "owQ5YZrleGU" });

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistGenres_GenreId",
                schema: "Listify",
                table: "PlaylistGenres",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistGenres_PlaylistId",
                schema: "Listify",
                table: "PlaylistGenres",
                column: "PlaylistId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlaylistGenres",
                schema: "Listify");

            migrationBuilder.DropTable(
                name: "Genres",
                schema: "Listify");

            migrationBuilder.DeleteData(
                schema: "Listify",
                table: "Songs",
                keyColumn: "Id",
                keyValue: new Guid("24c294bf-2bef-404c-b007-076deea68401"));

            migrationBuilder.DropColumn(
                name: "IsPublic",
                schema: "Listify",
                table: "Playlists");

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "Currencies",
                keyColumn: "Id",
                keyValue: new Guid("7385db66-c5d6-4f99-84dc-74cf9695a459"),
                columns: new[] { "TimeSecBetweenTick", "TimeStamp" },
                values: new object[] { 30f, new DateTime(2020, 9, 21, 16, 11, 23, 16, DateTimeKind.Utc).AddTicks(8284) });

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "PurchasableItems",
                keyColumn: "Id",
                keyValue: new Guid("01237025-4ae3-4c73-8ad8-a94c67de8116"),
                column: "TimeStamp",
                value: new DateTime(2020, 9, 21, 16, 11, 23, 18, DateTimeKind.Utc).AddTicks(1255));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "PurchasableItems",
                keyColumn: "Id",
                keyValue: new Guid("018b97a0-c7ea-43e4-9531-3e48dfb3fa6e"),
                column: "TimeStamp",
                value: new DateTime(2020, 9, 21, 16, 11, 23, 18, DateTimeKind.Utc).AddTicks(1247));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "PurchasableItems",
                keyColumn: "Id",
                keyValue: new Guid("2baf4f54-875d-4668-8843-8e765e66eb00"),
                column: "TimeStamp",
                value: new DateTime(2020, 9, 21, 16, 11, 23, 18, DateTimeKind.Utc).AddTicks(397));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "PurchasableItems",
                keyColumn: "Id",
                keyValue: new Guid("5552e1cc-2a96-4590-b2ca-d3305c229353"),
                column: "TimeStamp",
                value: new DateTime(2020, 9, 21, 16, 11, 23, 18, DateTimeKind.Utc).AddTicks(1243));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "PurchasableItems",
                keyColumn: "Id",
                keyValue: new Guid("58542258-d6b4-490e-835a-3e78ec8c9d2d"),
                column: "TimeStamp",
                value: new DateTime(2020, 9, 21, 16, 11, 23, 18, DateTimeKind.Utc).AddTicks(1217));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "PurchasableItems",
                keyColumn: "Id",
                keyValue: new Guid("5a72bc3c-7494-484d-a30a-5e6a6c698b0d"),
                column: "TimeStamp",
                value: new DateTime(2020, 9, 21, 16, 11, 23, 18, DateTimeKind.Utc).AddTicks(1250));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "PurchasableItems",
                keyColumn: "Id",
                keyValue: new Guid("610dfa85-a0d3-4c36-ab8e-0153ff3742ce"),
                column: "TimeStamp",
                value: new DateTime(2020, 9, 21, 16, 11, 23, 18, DateTimeKind.Utc).AddTicks(1237));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "PurchasableItems",
                keyColumn: "Id",
                keyValue: new Guid("aa147747-3010-4047-8103-b1b50a93bf7f"),
                column: "TimeStamp",
                value: new DateTime(2020, 9, 21, 16, 11, 23, 18, DateTimeKind.Utc).AddTicks(1259));

            migrationBuilder.UpdateData(
                schema: "Listify",
                table: "PurchasableItems",
                keyColumn: "Id",
                keyValue: new Guid("f217aa5a-a01f-4b7e-891b-b7d9210e8a11"),
                column: "TimeStamp",
                value: new DateTime(2020, 9, 21, 16, 11, 23, 18, DateTimeKind.Utc).AddTicks(1240));
        }
    }
}
