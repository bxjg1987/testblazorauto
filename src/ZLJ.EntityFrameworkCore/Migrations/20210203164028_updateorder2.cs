using Microsoft.EntityFrameworkCore.Migrations;

namespace ZLJ.Migrations
{
    public partial class updateorder2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BXJGShopOrder_BXJGBaseInfoAdministratives_AreaId",
                table: "BXJGShopOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_BXJGShopOrder_BXJGGeneralTrees_DistributionMethodId",
                table: "BXJGShopOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_BXJGShopOrder_BXJGGeneralTrees_PaymentMethodId",
                table: "BXJGShopOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_BXJGShopOrder_BXJGShopCustomer_CustomerId",
                table: "BXJGShopOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_BXJGShopOrderItem_BXJGShopProduct_ProductId",
                table: "BXJGShopOrderItem");

            migrationBuilder.DropForeignKey(
                name: "FK_BXJGShopOrderItem_BXJGShopSku_SkuId",
                table: "BXJGShopOrderItem");

            migrationBuilder.DropIndex(
                name: "IX_BXJGShopOrderItem_ProductId",
                table: "BXJGShopOrderItem");

            migrationBuilder.DropIndex(
                name: "IX_BXJGShopOrderItem_SkuId",
                table: "BXJGShopOrderItem");

            migrationBuilder.DropIndex(
                name: "IX_BXJGShopOrder_AreaId",
                table: "BXJGShopOrder");

            migrationBuilder.DropIndex(
                name: "IX_BXJGShopOrder_CustomerId",
                table: "BXJGShopOrder");

            migrationBuilder.DropIndex(
                name: "IX_BXJGShopOrder_DistributionMethodId",
                table: "BXJGShopOrder");

            migrationBuilder.DropIndex(
                name: "IX_BXJGShopOrder_PaymentMethodId",
                table: "BXJGShopOrder");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_BXJGShopOrderItem_ProductId",
                table: "BXJGShopOrderItem",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_BXJGShopOrderItem_SkuId",
                table: "BXJGShopOrderItem",
                column: "SkuId");

            migrationBuilder.CreateIndex(
                name: "IX_BXJGShopOrder_AreaId",
                table: "BXJGShopOrder",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_BXJGShopOrder_CustomerId",
                table: "BXJGShopOrder",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_BXJGShopOrder_DistributionMethodId",
                table: "BXJGShopOrder",
                column: "DistributionMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_BXJGShopOrder_PaymentMethodId",
                table: "BXJGShopOrder",
                column: "PaymentMethodId");

            migrationBuilder.AddForeignKey(
                name: "FK_BXJGShopOrder_BXJGBaseInfoAdministratives_AreaId",
                table: "BXJGShopOrder",
                column: "AreaId",
                principalTable: "BXJGBaseInfoAdministratives",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BXJGShopOrder_BXJGGeneralTrees_DistributionMethodId",
                table: "BXJGShopOrder",
                column: "DistributionMethodId",
                principalTable: "BXJGGeneralTrees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BXJGShopOrder_BXJGGeneralTrees_PaymentMethodId",
                table: "BXJGShopOrder",
                column: "PaymentMethodId",
                principalTable: "BXJGGeneralTrees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BXJGShopOrder_BXJGShopCustomer_CustomerId",
                table: "BXJGShopOrder",
                column: "CustomerId",
                principalTable: "BXJGShopCustomer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BXJGShopOrderItem_BXJGShopProduct_ProductId",
                table: "BXJGShopOrderItem",
                column: "ProductId",
                principalTable: "BXJGShopProduct",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BXJGShopOrderItem_BXJGShopSku_SkuId",
                table: "BXJGShopOrderItem",
                column: "SkuId",
                principalTable: "BXJGShopSku",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
