using Microsoft.EntityFrameworkCore.Migrations;

namespace Listify.Domain.CodeFirst.Migrations
{
    public partial class addingloginFixedmigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchasesPurchasableItems_PurchasableItems_PurchasableItemId",
                table: "PurchasesPurchasableItems");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchasesPurchasableItems_Purchases_PurchaseId",
                table: "PurchasesPurchasableItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PurchasesPurchasableItems",
                table: "PurchasesPurchasableItems");

            migrationBuilder.RenameTable(
                name: "Purchases",
                newName: "Purchases",
                newSchema: "Listify");

            migrationBuilder.RenameTable(
                name: "PurchasableItems",
                newName: "PurchasableItems",
                newSchema: "Listify");

            migrationBuilder.RenameTable(
                name: "PurchasesPurchasableItems",
                newName: "PurchasePurchasableItems",
                newSchema: "Listify");

            migrationBuilder.RenameIndex(
                name: "IX_PurchasesPurchasableItems_PurchaseId",
                schema: "Listify",
                table: "PurchasePurchasableItems",
                newName: "IX_PurchasePurchasableItems_PurchaseId");

            migrationBuilder.RenameIndex(
                name: "IX_PurchasesPurchasableItems_PurchasableItemId",
                schema: "Listify",
                table: "PurchasePurchasableItems",
                newName: "IX_PurchasePurchasableItems_PurchasableItemId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PurchasePurchasableItems",
                schema: "Listify",
                table: "PurchasePurchasableItems",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchasePurchasableItems_PurchasableItems_PurchasableItemId",
                schema: "Listify",
                table: "PurchasePurchasableItems",
                column: "PurchasableItemId",
                principalSchema: "Listify",
                principalTable: "PurchasableItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchasePurchasableItems_Purchases_PurchaseId",
                schema: "Listify",
                table: "PurchasePurchasableItems",
                column: "PurchaseId",
                principalSchema: "Listify",
                principalTable: "Purchases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchasePurchasableItems_PurchasableItems_PurchasableItemId",
                schema: "Listify",
                table: "PurchasePurchasableItems");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchasePurchasableItems_Purchases_PurchaseId",
                schema: "Listify",
                table: "PurchasePurchasableItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PurchasePurchasableItems",
                schema: "Listify",
                table: "PurchasePurchasableItems");

            migrationBuilder.RenameTable(
                name: "Purchases",
                schema: "Listify",
                newName: "Purchases");

            migrationBuilder.RenameTable(
                name: "PurchasableItems",
                schema: "Listify",
                newName: "PurchasableItems");

            migrationBuilder.RenameTable(
                name: "PurchasePurchasableItems",
                schema: "Listify",
                newName: "PurchasesPurchasableItems");

            migrationBuilder.RenameIndex(
                name: "IX_PurchasePurchasableItems_PurchaseId",
                table: "PurchasesPurchasableItems",
                newName: "IX_PurchasesPurchasableItems_PurchaseId");

            migrationBuilder.RenameIndex(
                name: "IX_PurchasePurchasableItems_PurchasableItemId",
                table: "PurchasesPurchasableItems",
                newName: "IX_PurchasesPurchasableItems_PurchasableItemId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PurchasesPurchasableItems",
                table: "PurchasesPurchasableItems",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchasesPurchasableItems_PurchasableItems_PurchasableItemId",
                table: "PurchasesPurchasableItems",
                column: "PurchasableItemId",
                principalTable: "PurchasableItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchasesPurchasableItems_Purchases_PurchaseId",
                table: "PurchasesPurchasableItems",
                column: "PurchaseId",
                principalTable: "Purchases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
