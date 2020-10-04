using Microsoft.EntityFrameworkCore.Migrations;

namespace ZLJ.Migrations
{
    public partial class updateitemtoproduct2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SkuEntity_BXJGShopItems_ProductId",
                table: "SkuEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SkuEntity",
                table: "SkuEntity");

            migrationBuilder.RenameTable(
                name: "SkuEntity",
                newName: "BXJGShopSku");

            migrationBuilder.RenameIndex(
                name: "IX_SkuEntity_ProductId",
                table: "BXJGShopSku",
                newName: "IX_BXJGShopSku_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BXJGShopSku",
                table: "BXJGShopSku",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BXJGShopSku_BXJGShopItems_ProductId",
                table: "BXJGShopSku",
                column: "ProductId",
                principalTable: "BXJGShopItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BXJGShopSku_BXJGShopItems_ProductId",
                table: "BXJGShopSku");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BXJGShopSku",
                table: "BXJGShopSku");

            migrationBuilder.RenameTable(
                name: "BXJGShopSku",
                newName: "SkuEntity");

            migrationBuilder.RenameIndex(
                name: "IX_BXJGShopSku_ProductId",
                table: "SkuEntity",
                newName: "IX_SkuEntity_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SkuEntity",
                table: "SkuEntity",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SkuEntity_BXJGShopItems_ProductId",
                table: "SkuEntity",
                column: "ProductId",
                principalTable: "BXJGShopItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
