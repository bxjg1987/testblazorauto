using Microsoft.EntityFrameworkCore.Migrations;

namespace ZLJ.Migrations
{
    public partial class updateShopItemAddUnitSpecixxx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Specification",
                table: "BXJGShopItems",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UnitId",
                table: "BXJGShopItems",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BXJGShopItems_UnitId",
                table: "BXJGShopItems",
                column: "UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_BXJGShopItems_BXJGGeneralTrees_UnitId",
                table: "BXJGShopItems",
                column: "UnitId",
                principalTable: "BXJGGeneralTrees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BXJGShopItems_BXJGGeneralTrees_UnitId",
                table: "BXJGShopItems");

            migrationBuilder.DropIndex(
                name: "IX_BXJGShopItems_UnitId",
                table: "BXJGShopItems");

            migrationBuilder.DropColumn(
                name: "Specification",
                table: "BXJGShopItems");

            migrationBuilder.DropColumn(
                name: "UnitId",
                table: "BXJGShopItems");
        }
    }
}
