using Microsoft.EntityFrameworkCore.Migrations;

namespace ZLJ.Migrations
{
    public partial class addItembrand : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "BrandId",
                table: "BXJGShopItems",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BXJGShopItems_BrandId",
                table: "BXJGShopItems",
                column: "BrandId");

            migrationBuilder.AddForeignKey(
                name: "FK_BXJGShopItems_BXJGShopDictionaries_BrandId",
                table: "BXJGShopItems",
                column: "BrandId",
                principalTable: "BXJGShopDictionaries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BXJGShopItems_BXJGShopDictionaries_BrandId",
                table: "BXJGShopItems");

            migrationBuilder.DropIndex(
                name: "IX_BXJGShopItems_BrandId",
                table: "BXJGShopItems");

            migrationBuilder.DropColumn(
                name: "BrandId",
                table: "BXJGShopItems");
        }
    }
}
