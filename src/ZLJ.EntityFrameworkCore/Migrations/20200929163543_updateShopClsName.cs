using Microsoft.EntityFrameworkCore.Migrations;

namespace ZLJ.Migrations
{
    public partial class updateShopClsName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BXJGShopItemCategories_BXJGShopItemCategories_ParentId",
                table: "BXJGShopItemCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_BXJGShopItems_BXJGShopItemCategories_CategoryId",
                table: "BXJGShopItems");

            migrationBuilder.DropForeignKey(
                name: "FK_BXJGShopProducts_BXJGShopItemCategories_CategoryId",
                table: "BXJGShopProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BXJGShopItemCategories",
                table: "BXJGShopItemCategories");

            migrationBuilder.RenameTable(
                name: "BXJGShopItemCategories",
                newName: "BXJGShopProductCategories");

            migrationBuilder.RenameIndex(
                name: "IX_BXJGShopItemCategories_ParentId",
                table: "BXJGShopProductCategories",
                newName: "IX_BXJGShopProductCategories_ParentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BXJGShopProductCategories",
                table: "BXJGShopProductCategories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BXJGShopItems_BXJGShopProductCategories_CategoryId",
                table: "BXJGShopItems",
                column: "CategoryId",
                principalTable: "BXJGShopProductCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BXJGShopProductCategories_BXJGShopProductCategories_ParentId",
                table: "BXJGShopProductCategories",
                column: "ParentId",
                principalTable: "BXJGShopProductCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BXJGShopProducts_BXJGShopProductCategories_CategoryId",
                table: "BXJGShopProducts",
                column: "CategoryId",
                principalTable: "BXJGShopProductCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BXJGShopItems_BXJGShopProductCategories_CategoryId",
                table: "BXJGShopItems");

            migrationBuilder.DropForeignKey(
                name: "FK_BXJGShopProductCategories_BXJGShopProductCategories_ParentId",
                table: "BXJGShopProductCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_BXJGShopProducts_BXJGShopProductCategories_CategoryId",
                table: "BXJGShopProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BXJGShopProductCategories",
                table: "BXJGShopProductCategories");

            migrationBuilder.RenameTable(
                name: "BXJGShopProductCategories",
                newName: "BXJGShopItemCategories");

            migrationBuilder.RenameIndex(
                name: "IX_BXJGShopProductCategories_ParentId",
                table: "BXJGShopItemCategories",
                newName: "IX_BXJGShopItemCategories_ParentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BXJGShopItemCategories",
                table: "BXJGShopItemCategories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BXJGShopItemCategories_BXJGShopItemCategories_ParentId",
                table: "BXJGShopItemCategories",
                column: "ParentId",
                principalTable: "BXJGShopItemCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BXJGShopItems_BXJGShopItemCategories_CategoryId",
                table: "BXJGShopItems",
                column: "CategoryId",
                principalTable: "BXJGShopItemCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BXJGShopProducts_BXJGShopItemCategories_CategoryId",
                table: "BXJGShopProducts",
                column: "CategoryId",
                principalTable: "BXJGShopItemCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
