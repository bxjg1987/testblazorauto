using Microsoft.EntityFrameworkCore.Migrations;

namespace ZLJ.Migrations
{
    public partial class dagengxin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BXJGShopItems_BXJGShopDictionaries_BrandId",
                table: "BXJGShopItems");

            migrationBuilder.DropForeignKey(
                name: "FK_BXJGShopItems_BXJGShopDictionaries_CategoryId",
                table: "BXJGShopItems");

            migrationBuilder.DropForeignKey(
                name: "FK_BXJGShopOrders_BXJGShopDictionaries_DistributionMethodId",
                table: "BXJGShopOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_BXJGShopOrders_BXJGShopDictionaries_PaymentMethodId",
                table: "BXJGShopOrders");

            migrationBuilder.AlterColumn<string>(
                name: "Images",
                table: "BXJGShopItems",
                type: "varchar(5000)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldMaxLength: 5000,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BXJGShopItems_BXJGGeneralTrees_BrandId",
                table: "BXJGShopItems",
                column: "BrandId",
                principalTable: "BXJGGeneralTrees",
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
                name: "FK_BXJGShopOrders_BXJGGeneralTrees_DistributionMethodId",
                table: "BXJGShopOrders",
                column: "DistributionMethodId",
                principalTable: "BXJGGeneralTrees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BXJGShopOrders_BXJGGeneralTrees_PaymentMethodId",
                table: "BXJGShopOrders",
                column: "PaymentMethodId",
                principalTable: "BXJGGeneralTrees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BXJGShopItems_BXJGGeneralTrees_BrandId",
                table: "BXJGShopItems");

            migrationBuilder.DropForeignKey(
                name: "FK_BXJGShopItems_BXJGShopItemCategories_CategoryId",
                table: "BXJGShopItems");

            migrationBuilder.DropForeignKey(
                name: "FK_BXJGShopOrders_BXJGGeneralTrees_DistributionMethodId",
                table: "BXJGShopOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_BXJGShopOrders_BXJGGeneralTrees_PaymentMethodId",
                table: "BXJGShopOrders");

            migrationBuilder.AlterColumn<string>(
                name: "Images",
                table: "BXJGShopItems",
                type: "nvarchar(max)",
                maxLength: 5000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(5000)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BXJGShopItems_BXJGShopDictionaries_BrandId",
                table: "BXJGShopItems",
                column: "BrandId",
                principalTable: "BXJGShopDictionaries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BXJGShopItems_BXJGShopDictionaries_CategoryId",
                table: "BXJGShopItems",
                column: "CategoryId",
                principalTable: "BXJGShopDictionaries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BXJGShopOrders_BXJGShopDictionaries_DistributionMethodId",
                table: "BXJGShopOrders",
                column: "DistributionMethodId",
                principalTable: "BXJGShopDictionaries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BXJGShopOrders_BXJGShopDictionaries_PaymentMethodId",
                table: "BXJGShopOrders",
                column: "PaymentMethodId",
                principalTable: "BXJGShopDictionaries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
