using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Listify.Domain.CodeFirst.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Listify");

            migrationBuilder.CreateTable(
                name: "ApplicationUsers",
                schema: "Listify",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    AspNetUserId = table.Column<string>(nullable: true),
                    Username = table.Column<string>(nullable: true),
                    PlaylistCountMax = table.Column<int>(nullable: false),
                    SongPoolCountSongsMax = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Songs",
                schema: "Listify",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    SongName = table.Column<string>(nullable: true),
                    YoutubeId = table.Column<string>(nullable: true),
                    SongLengthSeconds = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Songs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Playlists",
                schema: "Listify",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    PlaylistName = table.Column<string>(nullable: true),
                    IsSelected = table.Column<bool>(nullable: false),
                    ApplicationUserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Playlists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Playlists_ApplicationUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalSchema: "Listify",
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                schema: "Listify",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    RoomCode = table.Column<string>(nullable: true),
                    IsRoomPublic = table.Column<bool>(nullable: false),
                    IsRoomOnline = table.Column<bool>(nullable: false),
                    ApplicationUserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rooms_ApplicationUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalSchema: "Listify",
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUsersRooms",
                schema: "Listify",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    ApplciationUserId = table.Column<Guid>(nullable: false),
                    RoomId = table.Column<Guid>(nullable: false),
                    ApplicationUserId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUsersRooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationUsersRooms_ApplicationUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalSchema: "Listify",
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApplicationUsersRooms_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalSchema: "Listify",
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Currencies",
                schema: "Listify",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    CurrencyName = table.Column<string>(nullable: true),
                    Weight = table.Column<int>(nullable: false),
                    QuantityIncreasePerTick = table.Column<int>(nullable: false),
                    TimeSecBetweenTick = table.Column<int>(nullable: false),
                    TimestampLastUpdated = table.Column<DateTime>(nullable: false),
                    RoomId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Currencies_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalSchema: "Listify",
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SongRequests",
                schema: "Listify",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    SongId = table.Column<Guid>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    PlayCount = table.Column<int>(nullable: true),
                    PlaylistId = table.Column<Guid>(nullable: true),
                    WeightedCurrentValue = table.Column<int>(nullable: true),
                    ApplicationUserId = table.Column<Guid>(nullable: true),
                    RoomId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SongRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SongRequests_Playlists_PlaylistId",
                        column: x => x.PlaylistId,
                        principalSchema: "Listify",
                        principalTable: "Playlists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SongRequests_ApplicationUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalSchema: "Listify",
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SongRequests_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalSchema: "Listify",
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SongRequests_Songs_SongId",
                        column: x => x.SongId,
                        principalSchema: "Listify",
                        principalTable: "Songs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUsersRoomsConnections",
                schema: "Listify",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    ConnectionId = table.Column<string>(nullable: true),
                    IsOnline = table.Column<bool>(nullable: false),
                    HasPingBeenSent = table.Column<bool>(nullable: false),
                    ApplicationUserRoomId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUsersRoomsConnections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationUsersRoomsConnections_ApplicationUsersRooms_ApplicationUserRoomId",
                        column: x => x.ApplicationUserRoomId,
                        principalSchema: "Listify",
                        principalTable: "ApplicationUsersRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChatMessages",
                schema: "Listify",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    Message = table.Column<string>(nullable: true),
                    ApplicationUserRoomId = table.Column<Guid>(nullable: false),
                    ApplicationUserId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatMessages_ApplicationUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalSchema: "Listify",
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChatMessages_ApplicationUsersRooms_ApplicationUserRoomId",
                        column: x => x.ApplicationUserRoomId,
                        principalSchema: "Listify",
                        principalTable: "ApplicationUsersRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUsersRoomsCurrencies",
                schema: "Listify",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    ApplicationUserRoomId = table.Column<Guid>(nullable: false),
                    CurrencyId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUsersRoomsCurrencies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationUsersRoomsCurrencies_ApplicationUsersRooms_ApplicationUserRoomId",
                        column: x => x.ApplicationUserRoomId,
                        principalSchema: "Listify",
                        principalTable: "ApplicationUsersRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUsersRoomsCurrencies_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalSchema: "Listify",
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                schema: "Listify",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    TransactionType = table.Column<int>(nullable: false),
                    QuantityChange = table.Column<int>(nullable: false),
                    ApplicationUserRoomId = table.Column<Guid>(nullable: false),
                    ApplicationUserRoomCurrencyId = table.Column<Guid>(nullable: true),
                    Discriminator = table.Column<string>(nullable: false),
                    SongQueuedId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_ApplicationUsersRoomsCurrencies_ApplicationUserRoomCurrencyId",
                        column: x => x.ApplicationUserRoomCurrencyId,
                        principalSchema: "Listify",
                        principalTable: "ApplicationUsersRoomsCurrencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transactions_ApplicationUsersRooms_ApplicationUserRoomId",
                        column: x => x.ApplicationUserRoomId,
                        principalSchema: "Listify",
                        principalTable: "ApplicationUsersRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transactions_SongRequests_SongQueuedId",
                        column: x => x.SongQueuedId,
                        principalSchema: "Listify",
                        principalTable: "SongRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUsersRooms_ApplicationUserId",
                schema: "Listify",
                table: "ApplicationUsersRooms",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUsersRooms_RoomId",
                schema: "Listify",
                table: "ApplicationUsersRooms",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUsersRoomsConnections_ApplicationUserRoomId",
                schema: "Listify",
                table: "ApplicationUsersRoomsConnections",
                column: "ApplicationUserRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUsersRoomsCurrencies_ApplicationUserRoomId",
                schema: "Listify",
                table: "ApplicationUsersRoomsCurrencies",
                column: "ApplicationUserRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUsersRoomsCurrencies_CurrencyId",
                schema: "Listify",
                table: "ApplicationUsersRoomsCurrencies",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_ApplicationUserId",
                schema: "Listify",
                table: "ChatMessages",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_ApplicationUserRoomId",
                schema: "Listify",
                table: "ChatMessages",
                column: "ApplicationUserRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Currencies_RoomId",
                schema: "Listify",
                table: "Currencies",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Playlists_ApplicationUserId",
                schema: "Listify",
                table: "Playlists",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_ApplicationUserId",
                schema: "Listify",
                table: "Rooms",
                column: "ApplicationUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SongRequests_PlaylistId",
                schema: "Listify",
                table: "SongRequests",
                column: "PlaylistId");

            migrationBuilder.CreateIndex(
                name: "IX_SongRequests_ApplicationUserId",
                schema: "Listify",
                table: "SongRequests",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SongRequests_RoomId",
                schema: "Listify",
                table: "SongRequests",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_SongRequests_SongId",
                schema: "Listify",
                table: "SongRequests",
                column: "SongId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_ApplicationUserRoomCurrencyId",
                schema: "Listify",
                table: "Transactions",
                column: "ApplicationUserRoomCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_ApplicationUserRoomId",
                schema: "Listify",
                table: "Transactions",
                column: "ApplicationUserRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_SongQueuedId",
                schema: "Listify",
                table: "Transactions",
                column: "SongQueuedId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUsersRoomsConnections",
                schema: "Listify");

            migrationBuilder.DropTable(
                name: "ChatMessages",
                schema: "Listify");

            migrationBuilder.DropTable(
                name: "Transactions",
                schema: "Listify");

            migrationBuilder.DropTable(
                name: "ApplicationUsersRoomsCurrencies",
                schema: "Listify");

            migrationBuilder.DropTable(
                name: "SongRequests",
                schema: "Listify");

            migrationBuilder.DropTable(
                name: "ApplicationUsersRooms",
                schema: "Listify");

            migrationBuilder.DropTable(
                name: "Currencies",
                schema: "Listify");

            migrationBuilder.DropTable(
                name: "Playlists",
                schema: "Listify");

            migrationBuilder.DropTable(
                name: "Songs",
                schema: "Listify");

            migrationBuilder.DropTable(
                name: "Rooms",
                schema: "Listify");

            migrationBuilder.DropTable(
                name: "ApplicationUsers",
                schema: "Listify");
        }
    }
}
