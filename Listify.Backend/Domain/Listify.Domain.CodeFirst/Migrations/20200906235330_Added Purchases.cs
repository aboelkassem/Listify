using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Listify.Domain.CodeFirst.Migrations
{
    public partial class AddedPurchases : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PurchasableItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    PurchasableItemName = table.Column<string>(nullable: true),
                    PurchasableItemType = table.Column<int>(nullable: false),
                    Quantity = table.Column<float>(nullable: false),
                    UnitCost = table.Column<float>(nullable: false),
                    ImageUri = table.Column<string>(nullable: true),
                    DiscountApplied = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchasableItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Purchases",
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
                name: "PurchasesPurchasableItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    PurchasableItemId = table.Column<Guid>(nullable: false),
                    PurchaseId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchasesPurchasableItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchasesPurchasableItems_PurchasableItems_PurchasableItemId",
                        column: x => x.PurchasableItemId,
                        principalTable: "PurchasableItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchasesPurchasableItems_Purchases_PurchaseId",
                        column: x => x.PurchaseId,
                        principalTable: "Purchases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_ApplicationUserId",
                table: "Purchases",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchasesPurchasableItems_PurchasableItemId",
                table: "PurchasesPurchasableItems",
                column: "PurchasableItemId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchasesPurchasableItems_PurchaseId",
                table: "PurchasesPurchasableItems",
                column: "PurchaseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PurchasesPurchasableItems");

            migrationBuilder.DropTable(
                name: "PurchasableItems");

            migrationBuilder.DropTable(
                name: "Purchases");
        }
    }
}
