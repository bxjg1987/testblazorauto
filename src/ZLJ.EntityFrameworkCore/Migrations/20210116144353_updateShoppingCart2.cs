using Microsoft.EntityFrameworkCore.Migrations;

namespace ZLJ.Migrations
{
    public partial class updateShoppingCart2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCartEntity_BXJGShopCustomer_CustomerId",
                table: "ShoppingCartEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCartItemEntity_BXJGShopProduct_ProductId",
                table: "ShoppingCartItemEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCartItemEntity_BXJGShopSku_SkuId",
                table: "ShoppingCartItemEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCartItemEntity_ShoppingCartEntity_ShoppingCartId",
                table: "ShoppingCartItemEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShoppingCartItemEntity",
                table: "ShoppingCartItemEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShoppingCartEntity",
                table: "ShoppingCartEntity");

            migrationBuilder.RenameTable(
                name: "ShoppingCartItemEntity",
                newName: "BXJGShopShoppingCartItem");

            migrationBuilder.RenameTable(
                name: "ShoppingCartEntity",
                newName: "BXJGShopShoppingCart");

            migrationBuilder.RenameIndex(
                name: "IX_ShoppingCartItemEntity_SkuId",
                table: "BXJGShopShoppingCartItem",
                newName: "IX_BXJGShopShoppingCartItem_SkuId");

            migrationBuilder.RenameIndex(
                name: "IX_ShoppingCartItemEntity_ShoppingCartId",
                table: "BXJGShopShoppingCartItem",
                newName: "IX_BXJGShopShoppingCartItem_ShoppingCartId");

            migrationBuilder.RenameIndex(
                name: "IX_ShoppingCartItemEntity_ProductId",
                table: "BXJGShopShoppingCartItem",
                newName: "IX_BXJGShopShoppingCartItem_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ShoppingCartEntity_CustomerId",
                table: "BXJGShopShoppingCart",
                newName: "IX_BXJGShopShoppingCart_CustomerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BXJGShopShoppingCartItem",
                table: "BXJGShopShoppingCartItem",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BXJGShopShoppingCart",
                table: "BXJGShopShoppingCart",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BXJGShopShoppingCart_BXJGShopCustomer_CustomerId",
                table: "BXJGShopShoppingCart",
                column: "CustomerId",
                principalTable: "BXJGShopCustomer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BXJGShopShoppingCartItem_BXJGShopProduct_ProductId",
                table: "BXJGShopShoppingCartItem",
                column: "ProductId",
                principalTable: "BXJGShopProduct",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BXJGShopShoppingCartItem_BXJGShopShoppingCart_ShoppingCartId",
                table: "BXJGShopShoppingCartItem",
                column: "ShoppingCartId",
                principalTable: "BXJGShopShoppingCart",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BXJGShopShoppingCartItem_BXJGShopSku_SkuId",
                table: "BXJGShopShoppingCartItem",
                column: "SkuId",
                principalTable: "BXJGShopSku",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BXJGShopShoppingCart_BXJGShopCustomer_CustomerId",
                table: "BXJGShopShoppingCart");

            migrationBuilder.DropForeignKey(
                name: "FK_BXJGShopShoppingCartItem_BXJGShopProduct_ProductId",
                table: "BXJGShopShoppingCartItem");

            migrationBuilder.DropForeignKey(
                name: "FK_BXJGShopShoppingCartItem_BXJGShopShoppingCart_ShoppingCartId",
                table: "BXJGShopShoppingCartItem");

            migrationBuilder.DropForeignKey(
                name: "FK_BXJGShopShoppingCartItem_BXJGShopSku_SkuId",
                table: "BXJGShopShoppingCartItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BXJGShopShoppingCartItem",
                table: "BXJGShopShoppingCartItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BXJGShopShoppingCart",
                table: "BXJGShopShoppingCart");

            migrationBuilder.RenameTable(
                name: "BXJGShopShoppingCartItem",
                newName: "ShoppingCartItemEntity");

            migrationBuilder.RenameTable(
                name: "BXJGShopShoppingCart",
                newName: "ShoppingCartEntity");

            migrationBuilder.RenameIndex(
                name: "IX_BXJGShopShoppingCartItem_SkuId",
                table: "ShoppingCartItemEntity",
                newName: "IX_ShoppingCartItemEntity_SkuId");

            migrationBuilder.RenameIndex(
                name: "IX_BXJGShopShoppingCartItem_ShoppingCartId",
                table: "ShoppingCartItemEntity",
                newName: "IX_ShoppingCartItemEntity_ShoppingCartId");

            migrationBuilder.RenameIndex(
                name: "IX_BXJGShopShoppingCartItem_ProductId",
                table: "ShoppingCartItemEntity",
                newName: "IX_ShoppingCartItemEntity_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_BXJGShopShoppingCart_CustomerId",
                table: "ShoppingCartEntity",
                newName: "IX_ShoppingCartEntity_CustomerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShoppingCartItemEntity",
                table: "ShoppingCartItemEntity",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShoppingCartEntity",
                table: "ShoppingCartEntity",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCartEntity_BXJGShopCustomer_CustomerId",
                table: "ShoppingCartEntity",
                column: "CustomerId",
                principalTable: "BXJGShopCustomer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCartItemEntity_BXJGShopProduct_ProductId",
                table: "ShoppingCartItemEntity",
                column: "ProductId",
                principalTable: "BXJGShopProduct",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCartItemEntity_BXJGShopSku_SkuId",
                table: "ShoppingCartItemEntity",
                column: "SkuId",
                principalTable: "BXJGShopSku",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCartItemEntity_ShoppingCartEntity_ShoppingCartId",
                table: "ShoppingCartItemEntity",
                column: "ShoppingCartId",
                principalTable: "ShoppingCartEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
