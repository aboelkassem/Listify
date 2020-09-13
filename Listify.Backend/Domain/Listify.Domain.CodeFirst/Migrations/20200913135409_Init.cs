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
                    Active = table.Column<bool>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    AspNetUserId = table.Column<string>(nullable: true),
                    Username = table.Column<string>(nullable: true),
                    PlaylistCountMax = table.Column<int>(nullable: false),
                    PlaylistSongCount = table.Column<int>(nullable: false),
                    QueueCount = table.Column<int>(nullable: false),
                    ChatColor = table.Column<string>(nullable: true),
                    DateJoined = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Currencies",
                schema: "Listify",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    CurrencyName = table.Column<string>(nullable: true),
                    Weight = table.Column<int>(nullable: false),
                    QuantityIncreasePerTick = table.Column<int>(nullable: false),
                    TimeSecBetweenTick = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PurchasableItems",
                schema: "Listify",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    PurchasableItemName = table.Column<string>(nullable: true),
                    PurchasableItemType = table.Column<int>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    UnitCost = table.Column<float>(nullable: false),
                    ImageUri = table.Column<string>(nullable: true),
                    DiscountApplied = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchasableItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Songs",
                schema: "Listify",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
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
                name: "LogErrors",
                schema: "Listify",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    Exception = table.Column<string>(nullable: true),
                    IPAddress = table.Column<string>(nullable: true),
                    ApplicationUserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogErrors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LogErrors_ApplicationUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalSchema: "Listify",
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LogsAPI",
                schema: "Listify",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    EndPointType = table.Column<int>(nullable: false),
                    ResponseCode = table.Column<int>(nullable: false),
                    IPAddress = table.Column<string>(nullable: true),
                    ApplicationUserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogsAPI", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LogsAPI_ApplicationUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalSchema: "Listify",
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Playlists",
                schema: "Listify",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
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
                name: "Purchases",
                schema: "Listify",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    PurchaseMethod = table.Column<int>(nullable: false),
                    Subtotal = table.Column<float>(nullable: false),
                    AmountCharged = table.Column<float>(nullable: false),
                    ApplicationUserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Purchases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Purchases_ApplicationUsers_ApplicationUserId",
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
                    Active = table.Column<bool>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    RoomCode = table.Column<string>(nullable: true),
                    RoomTitle = table.Column<string>(nullable: true),
                    RoomKey = table.Column<string>(nullable: true),
                    AllowRequests = table.Column<bool>(nullable: false),
                    IsRoomLocked = table.Column<bool>(nullable: false),
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
                name: "PurchaseLineItems",
                schema: "Listify",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    OrderQuantity = table.Column<int>(nullable: false),
                    PurchasableItemId = table.Column<Guid>(nullable: false),
                    PurchaseId = table.Column<Guid>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    ApplicationUserRoomCurrencyId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseLineItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseLineItems_PurchasableItems_PurchasableItemId",
                        column: x => x.PurchasableItemId,
                        principalSchema: "Listify",
                        principalTable: "PurchasableItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseLineItems_Purchases_PurchaseId",
                        column: x => x.PurchaseId,
                        principalSchema: "Listify",
                        principalTable: "Purchases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUsersRooms",
                schema: "Listify",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    IsOnline = table.Column<bool>(nullable: false),
                    IsOwner = table.Column<bool>(nullable: false),
                    ApplicationUserId = table.Column<Guid>(nullable: false),
                    RoomId = table.Column<Guid>(nullable: false)
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
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUsersRooms_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalSchema: "Listify",
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CurrenciesRoom",
                schema: "Listify",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    RoomId = table.Column<Guid>(nullable: false),
                    CurrencyId = table.Column<Guid>(nullable: false),
                    CurrencyName = table.Column<string>(nullable: true),
                    TimestampLastUpdate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrenciesRoom", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CurrenciesRoom_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalSchema: "Listify",
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CurrenciesRoom_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalSchema: "Listify",
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SongRequests",
                schema: "Listify",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    SongRequestType = table.Column<int>(nullable: false),
                    SongId = table.Column<Guid>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    PlayCount = table.Column<int>(nullable: true),
                    PlaylistId = table.Column<Guid>(nullable: true),
                    WeightedValue = table.Column<int>(nullable: true),
                    HasBeenPlayed = table.Column<bool>(nullable: true),
                    TimestampPlayed = table.Column<DateTime>(nullable: true),
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
                    Active = table.Column<bool>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    ConnectionId = table.Column<string>(nullable: true),
                    IsOnline = table.Column<bool>(nullable: false),
                    HasPingBeenSent = table.Column<bool>(nullable: false),
                    ConnectionType = table.Column<int>(nullable: false),
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
                    Active = table.Column<bool>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    Message = table.Column<string>(nullable: true),
                    ApplicationUserRoomId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatMessages_ApplicationUsersRooms_ApplicationUserRoomId",
                        column: x => x.ApplicationUserRoomId,
                        principalSchema: "Listify",
                        principalTable: "ApplicationUsersRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUsersRoomsCurrenciesRoom",
                schema: "Listify",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    Quantity = table.Column<float>(nullable: false),
                    ApplicationUserRoomId = table.Column<Guid>(nullable: false),
                    CurrencyRoomId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUsersRoomsCurrenciesRoom", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationUsersRoomsCurrenciesRoom_ApplicationUsersRooms_ApplicationUserRoomId",
                        column: x => x.ApplicationUserRoomId,
                        principalSchema: "Listify",
                        principalTable: "ApplicationUsersRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUsersRoomsCurrenciesRoom_CurrenciesRoom_CurrencyRoomId",
                        column: x => x.CurrencyRoomId,
                        principalSchema: "Listify",
                        principalTable: "CurrenciesRoom",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                schema: "Listify",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    TransactionType = table.Column<int>(nullable: false),
                    QuantityChange = table.Column<int>(nullable: false),
                    ApplicationUserRoomCurrencyId = table.Column<Guid>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    SongQueuedId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_ApplicationUsersRoomsCurrenciesRoom_ApplicationUserRoomCurrencyId",
                        column: x => x.ApplicationUserRoomCurrencyId,
                        principalSchema: "Listify",
                        principalTable: "ApplicationUsersRoomsCurrenciesRoom",
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

            migrationBuilder.InsertData(
                schema: "Listify",
                table: "Currencies",
                columns: new[] { "Id", "Active", "CurrencyName", "QuantityIncreasePerTick", "TimeSecBetweenTick", "TimeStamp", "Weight" },
                values: new object[] { new Guid("7385db66-c5d6-4f99-84dc-74cf9695a459"), true, "Tokens", 1, 30f, new DateTime(2020, 9, 13, 13, 54, 8, 318, DateTimeKind.Utc).AddTicks(6131), 1 });

            migrationBuilder.InsertData(
                schema: "Listify",
                table: "PurchasableItems",
                columns: new[] { "Id", "Active", "DiscountApplied", "ImageUri", "PurchasableItemName", "PurchasableItemType", "Quantity", "TimeStamp", "UnitCost" },
                values: new object[,]
                {
                    { new Guid("2baf4f54-875d-4668-8843-8e765e66eb00"), true, 0f, "https://res.cloudinary.com/dvdcninhs/image/upload/v1600001888/Listify%20Photos/1-playlist_gzqfcr.jpg", "1 Playlist", 0, 1, new DateTime(2020, 9, 13, 13, 54, 8, 320, DateTimeKind.Utc).AddTicks(331), 1f },
                    { new Guid("58542258-d6b4-490e-835a-3e78ec8c9d2d"), true, 0f, "https://res.cloudinary.com/dvdcninhs/image/upload/v1600001883/Listify%20Photos/3-playlist_p0sg3o.jpg", "Pack of 3 Playlist", 0, 3, new DateTime(2020, 9, 13, 13, 54, 8, 320, DateTimeKind.Utc).AddTicks(1284), 2f },
                    { new Guid("610dfa85-a0d3-4c36-ab8e-0153ff3742ce"), true, 0f, "https://res.cloudinary.com/dvdcninhs/image/upload/v1600001889/Listify%20Photos/5-playlist_bqmufv.jpg", "Pack of 5 Playlist", 0, 5, new DateTime(2020, 9, 13, 13, 54, 8, 320, DateTimeKind.Utc).AddTicks(1299), 3f },
                    { new Guid("f217aa5a-a01f-4b7e-891b-b7d9210e8a11"), true, 0f, "https://res.cloudinary.com/dvdcninhs/image/upload/v1600001889/Listify%20Photos/10-playlist_myaf2g.jpg", "Pack of 10 Playlist", 0, 10, new DateTime(2020, 9, 13, 13, 54, 8, 320, DateTimeKind.Utc).AddTicks(1307), 5f },
                    { new Guid("5552e1cc-2a96-4590-b2ca-d3305c229353"), true, 0f, "https://res.cloudinary.com/dvdcninhs/image/upload/v1600001886/Listify%20Photos/15-songs_ut0thz.jpg", "15 Additional Songs Per Playlist", 1, 15, new DateTime(2020, 9, 13, 13, 54, 8, 320, DateTimeKind.Utc).AddTicks(1310), 1f },
                    { new Guid("018b97a0-c7ea-43e4-9531-3e48dfb3fa6e"), true, 0f, "https://res.cloudinary.com/dvdcninhs/image/upload/v1600001886/Listify%20Photos/40-songs_xx4al5.jpg", "40 Additional Songs Per Playlist", 1, 40, new DateTime(2020, 9, 13, 13, 54, 8, 320, DateTimeKind.Utc).AddTicks(1313), 2f },
                    { new Guid("5a72bc3c-7494-484d-a30a-5e6a6c698b0d"), true, 0f, "https://res.cloudinary.com/dvdcninhs/image/upload/v1600001884/Listify%20Photos/80-songs_gdpufy.jpg", "80 Additional Songs Per Playlist", 1, 80, new DateTime(2020, 9, 13, 13, 54, 8, 320, DateTimeKind.Utc).AddTicks(1322), 3f },
                    { new Guid("01237025-4ae3-4c73-8ad8-a94c67de8116"), true, 0f, "https://res.cloudinary.com/dvdcninhs/image/upload/v1600001888/Listify%20Photos/160-songs_ztgjsd.jpg", "160 Additional Songs Per Playlist", 1, 160, new DateTime(2020, 9, 13, 13, 54, 8, 320, DateTimeKind.Utc).AddTicks(1325), 5f },
                    { new Guid("aa147747-3010-4047-8103-b1b50a93bf7f"), true, 0f, "https://res.cloudinary.com/dvdcninhs/image/upload/v1600001885/Listify%20Photos/40-tokens_ppx2qi.jpg", "40 Currencies Per Room", 2, 40, new DateTime(2020, 9, 13, 13, 54, 8, 320, DateTimeKind.Utc).AddTicks(1340), 1f }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUsers_AspNetUserId",
                schema: "Listify",
                table: "ApplicationUsers",
                column: "AspNetUserId",
                unique: true,
                filter: "[AspNetUserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUsers_Username",
                schema: "Listify",
                table: "ApplicationUsers",
                column: "Username",
                unique: true,
                filter: "[Username] IS NOT NULL");

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
                name: "IX_ApplicationUsersRoomsConnections_ConnectionId",
                schema: "Listify",
                table: "ApplicationUsersRoomsConnections",
                column: "ConnectionId",
                unique: true,
                filter: "[ConnectionId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUsersRoomsCurrenciesRoom_ApplicationUserRoomId",
                schema: "Listify",
                table: "ApplicationUsersRoomsCurrenciesRoom",
                column: "ApplicationUserRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUsersRoomsCurrenciesRoom_CurrencyRoomId",
                schema: "Listify",
                table: "ApplicationUsersRoomsCurrenciesRoom",
                column: "CurrencyRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_ApplicationUserRoomId",
                schema: "Listify",
                table: "ChatMessages",
                column: "ApplicationUserRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_CurrenciesRoom_CurrencyId",
                schema: "Listify",
                table: "CurrenciesRoom",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_CurrenciesRoom_RoomId",
                schema: "Listify",
                table: "CurrenciesRoom",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_LogErrors_ApplicationUserId",
                schema: "Listify",
                table: "LogErrors",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_LogsAPI_ApplicationUserId",
                schema: "Listify",
                table: "LogsAPI",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Playlists_ApplicationUserId",
                schema: "Listify",
                table: "Playlists",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseLineItems_PurchasableItemId",
                schema: "Listify",
                table: "PurchaseLineItems",
                column: "PurchasableItemId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseLineItems_PurchaseId",
                schema: "Listify",
                table: "PurchaseLineItems",
                column: "PurchaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_ApplicationUserId",
                schema: "Listify",
                table: "Purchases",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_ApplicationUserId",
                schema: "Listify",
                table: "Rooms",
                column: "ApplicationUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_RoomCode",
                schema: "Listify",
                table: "Rooms",
                column: "RoomCode",
                unique: true,
                filter: "[RoomCode] IS NOT NULL");

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
                name: "LogErrors",
                schema: "Listify");

            migrationBuilder.DropTable(
                name: "LogsAPI",
                schema: "Listify");

            migrationBuilder.DropTable(
                name: "PurchaseLineItems",
                schema: "Listify");

            migrationBuilder.DropTable(
                name: "Transactions",
                schema: "Listify");

            migrationBuilder.DropTable(
                name: "PurchasableItems",
                schema: "Listify");

            migrationBuilder.DropTable(
                name: "Purchases",
                schema: "Listify");

            migrationBuilder.DropTable(
                name: "ApplicationUsersRoomsCurrenciesRoom",
                schema: "Listify");

            migrationBuilder.DropTable(
                name: "SongRequests",
                schema: "Listify");

            migrationBuilder.DropTable(
                name: "ApplicationUsersRooms",
                schema: "Listify");

            migrationBuilder.DropTable(
                name: "CurrenciesRoom",
                schema: "Listify");

            migrationBuilder.DropTable(
                name: "Playlists",
                schema: "Listify");

            migrationBuilder.DropTable(
                name: "Songs",
                schema: "Listify");

            migrationBuilder.DropTable(
                name: "Currencies",
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
