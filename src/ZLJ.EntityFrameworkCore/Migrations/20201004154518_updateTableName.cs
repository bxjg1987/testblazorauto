using Microsoft.EntityFrameworkCore.Migrations;

namespace ZLJ.Migrations
{
    public partial class updateTableName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BXJGShopCustomers_BXJGBaseInfoAdministratives_AreaId",
                table: "BXJGShopCustomers");

            migrationBuilder.DropForeignKey(
                name: "FK_BXJGShopItems_BXJGGeneralTrees_BrandId",
                table: "BXJGShopItems");

            migrationBuilder.DropForeignKey(
                name: "FK_BXJGShopItems_BXJGShopProductCategories_CategoryId",
                table: "BXJGShopItems");

            migrationBuilder.DropForeignKey(
                name: "FK_BXJGShopItems_BXJGGeneralTrees_UnitId",
                table: "BXJGShopItems");

            migrationBuilder.DropForeignKey(
                name: "FK_BXJGShopOrderItems_BXJGShopItems_ItemId",
                table: "BXJGShopOrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_BXJGShopOrderItems_BXJGShopOrders_OrderId",
                table: "BXJGShopOrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_BXJGShopOrders_BXJGBaseInfoAdministratives_AreaId",
                table: "BXJGShopOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_BXJGShopOrders_BXJGShopCustomers_CustomerId",
                table: "BXJGShopOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_BXJGShopOrders_BXJGGeneralTrees_DistributionMethodId",
                table: "BXJGShopOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_BXJGShopOrders_BXJGGeneralTrees_PaymentMethodId",
                table: "BXJGShopOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_BXJGShopProductCategories_BXJGShopProductCategories_ParentId",
                table: "BXJGShopProductCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_BXJGShopSku_BXJGShopItems_ProductId",
                table: "BXJGShopSku");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BXJGShopProductCategories",
                table: "BXJGShopProductCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BXJGShopOrders",
                table: "BXJGShopOrders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BXJGShopItems",
                table: "BXJGShopItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BXJGShopCustomers",
                table: "BXJGShopCustomers");

            migrationBuilder.RenameTable(
                name: "BXJGShopProductCategories",
                newName: "BXJGShopProductCategory");

            migrationBuilder.RenameTable(
                name: "BXJGShopOrders",
                newName: "BXJGShopOrder");

            migrationBuilder.RenameTable(
                name: "BXJGShopItems",
                newName: "BXJGShopProduct");

            migrationBuilder.RenameTable(
                name: "BXJGShopCustomers",
                newName: "BXJGShopCustomer");

            migrationBuilder.RenameIndex(
                name: "IX_BXJGShopProductCategories_ParentId",
                table: "BXJGShopProductCategory",
                newName: "IX_BXJGShopProductCategory_ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_BXJGShopOrders_PaymentMethodId",
                table: "BXJGShopOrder",
                newName: "IX_BXJGShopOrder_PaymentMethodId");

            migrationBuilder.RenameIndex(
                name: "IX_BXJGShopOrders_OrderNo",
                table: "BXJGShopOrder",
                newName: "IX_BXJGShopOrder_OrderNo");

            migrationBuilder.RenameIndex(
                name: "IX_BXJGShopOrders_DistributionMethodId",
                table: "BXJGShopOrder",
                newName: "IX_BXJGShopOrder_DistributionMethodId");

            migrationBuilder.RenameIndex(
                name: "IX_BXJGShopOrders_CustomerId",
                table: "BXJGShopOrder",
                newName: "IX_BXJGShopOrder_CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_BXJGShopOrders_AreaId",
                table: "BXJGShopOrder",
                newName: "IX_BXJGShopOrder_AreaId");

            migrationBuilder.RenameIndex(
                name: "IX_BXJGShopItems_UnitId",
                table: "BXJGShopProduct",
                newName: "IX_BXJGShopProduct_UnitId");

            migrationBuilder.RenameIndex(
                name: "IX_BXJGShopItems_CategoryId",
                table: "BXJGShopProduct",
                newName: "IX_BXJGShopProduct_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_BXJGShopItems_BrandId",
                table: "BXJGShopProduct",
                newName: "IX_BXJGShopProduct_BrandId");

            migrationBuilder.RenameIndex(
                name: "IX_BXJGShopCustomers_AreaId",
                table: "BXJGShopCustomer",
                newName: "IX_BXJGShopCustomer_AreaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BXJGShopProductCategory",
                table: "BXJGShopProductCategory",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BXJGShopOrder",
                table: "BXJGShopOrder",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BXJGShopProduct",
                table: "BXJGShopProduct",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BXJGShopCustomer",
                table: "BXJGShopCustomer",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BXJGShopCustomer_BXJGBaseInfoAdministratives_AreaId",
                table: "BXJGShopCustomer",
                column: "AreaId",
                principalTable: "BXJGBaseInfoAdministratives",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BXJGShopOrder_BXJGBaseInfoAdministratives_AreaId",
                table: "BXJGShopOrder",
                column: "AreaId",
                principalTable: "BXJGBaseInfoAdministratives",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BXJGShopOrder_BXJGShopCustomer_CustomerId",
                table: "BXJGShopOrder",
                column: "CustomerId",
                principalTable: "BXJGShopCustomer",
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
                name: "FK_BXJGShopOrderItems_BXJGShopProduct_ItemId",
                table: "BXJGShopOrderItems",
                column: "ItemId",
                principalTable: "BXJGShopProduct",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BXJGShopOrderItems_BXJGShopOrder_OrderId",
                table: "BXJGShopOrderItems",
                column: "OrderId",
                principalTable: "BXJGShopOrder",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BXJGShopProduct_BXJGGeneralTrees_BrandId",
                table: "BXJGShopProduct",
                column: "BrandId",
                principalTable: "BXJGGeneralTrees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BXJGShopProduct_BXJGShopProductCategory_CategoryId",
                table: "BXJGShopProduct",
                column: "CategoryId",
                principalTable: "BXJGShopProductCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BXJGShopProduct_BXJGGeneralTrees_UnitId",
                table: "BXJGShopProduct",
                column: "UnitId",
                principalTable: "BXJGGeneralTrees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BXJGShopProductCategory_BXJGShopProductCategory_ParentId",
                table: "BXJGShopProductCategory",
                column: "ParentId",
                principalTable: "BXJGShopProductCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BXJGShopSku_BXJGShopProduct_ProductId",
                table: "BXJGShopSku",
                column: "ProductId",
                principalTable: "BXJGShopProduct",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BXJGShopCustomer_BXJGBaseInfoAdministratives_AreaId",
                table: "BXJGShopCustomer");

            migrationBuilder.DropForeignKey(
                name: "FK_BXJGShopOrder_BXJGBaseInfoAdministratives_AreaId",
                table: "BXJGShopOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_BXJGShopOrder_BXJGShopCustomer_CustomerId",
                table: "BXJGShopOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_BXJGShopOrder_BXJGGeneralTrees_DistributionMethodId",
                table: "BXJGShopOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_BXJGShopOrder_BXJGGeneralTrees_PaymentMethodId",
                table: "BXJGShopOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_BXJGShopOrderItems_BXJGShopProduct_ItemId",
                table: "BXJGShopOrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_BXJGShopOrderItems_BXJGShopOrder_OrderId",
                table: "BXJGShopOrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_BXJGShopProduct_BXJGGeneralTrees_BrandId",
                table: "BXJGShopProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_BXJGShopProduct_BXJGShopProductCategory_CategoryId",
                table: "BXJGShopProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_BXJGShopProduct_BXJGGeneralTrees_UnitId",
                table: "BXJGShopProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_BXJGShopProductCategory_BXJGShopProductCategory_ParentId",
                table: "BXJGShopProductCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_BXJGShopSku_BXJGShopProduct_ProductId",
                table: "BXJGShopSku");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BXJGShopProductCategory",
                table: "BXJGShopProductCategory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BXJGShopProduct",
                table: "BXJGShopProduct");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BXJGShopOrder",
                table: "BXJGShopOrder");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BXJGShopCustomer",
                table: "BXJGShopCustomer");

            migrationBuilder.RenameTable(
                name: "BXJGShopProductCategory",
                newName: "BXJGShopProductCategories");

            migrationBuilder.RenameTable(
                name: "BXJGShopProduct",
                newName: "BXJGShopItems");

            migrationBuilder.RenameTable(
                name: "BXJGShopOrder",
                newName: "BXJGShopOrders");

            migrationBuilder.RenameTable(
                name: "BXJGShopCustomer",
                newName: "BXJGShopCustomers");

            migrationBuilder.RenameIndex(
                name: "IX_BXJGShopProductCategory_ParentId",
                table: "BXJGShopProductCategories",
                newName: "IX_BXJGShopProductCategories_ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_BXJGShopProduct_UnitId",
                table: "BXJGShopItems",
                newName: "IX_BXJGShopItems_UnitId");

            migrationBuilder.RenameIndex(
                name: "IX_BXJGShopProduct_CategoryId",
                table: "BXJGShopItems",
                newName: "IX_BXJGShopItems_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_BXJGShopProduct_BrandId",
                table: "BXJGShopItems",
                newName: "IX_BXJGShopItems_BrandId");

            migrationBuilder.RenameIndex(
                name: "IX_BXJGShopOrder_PaymentMethodId",
                table: "BXJGShopOrders",
                newName: "IX_BXJGShopOrders_PaymentMethodId");

            migrationBuilder.RenameIndex(
                name: "IX_BXJGShopOrder_OrderNo",
                table: "BXJGShopOrders",
                newName: "IX_BXJGShopOrders_OrderNo");

            migrationBuilder.RenameIndex(
                name: "IX_BXJGShopOrder_DistributionMethodId",
                table: "BXJGShopOrders",
                newName: "IX_BXJGShopOrders_DistributionMethodId");

            migrationBuilder.RenameIndex(
                name: "IX_BXJGShopOrder_CustomerId",
                table: "BXJGShopOrders",
                newName: "IX_BXJGShopOrders_CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_BXJGShopOrder_AreaId",
                table: "BXJGShopOrders",
                newName: "IX_BXJGShopOrders_AreaId");

            migrationBuilder.RenameIndex(
                name: "IX_BXJGShopCustomer_AreaId",
                table: "BXJGShopCustomers",
                newName: "IX_BXJGShopCustomers_AreaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BXJGShopProductCategories",
                table: "BXJGShopProductCategories",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BXJGShopItems",
                table: "BXJGShopItems",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BXJGShopOrders",
                table: "BXJGShopOrders",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BXJGShopCustomers",
                table: "BXJGShopCustomers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BXJGShopCustomers_BXJGBaseInfoAdministratives_AreaId",
                table: "BXJGShopCustomers",
                column: "AreaId",
                principalTable: "BXJGBaseInfoAdministratives",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BXJGShopItems_BXJGGeneralTrees_BrandId",
                table: "BXJGShopItems",
                column: "BrandId",
                principalTable: "BXJGGeneralTrees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BXJGShopItems_BXJGShopProductCategories_CategoryId",
                table: "BXJGShopItems",
                column: "CategoryId",
                principalTable: "BXJGShopProductCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BXJGShopItems_BXJGGeneralTrees_UnitId",
                table: "BXJGShopItems",
                column: "UnitId",
                principalTable: "BXJGGeneralTrees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BXJGShopOrderItems_BXJGShopItems_ItemId",
                table: "BXJGShopOrderItems",
                column: "ItemId",
                principalTable: "BXJGShopItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BXJGShopOrderItems_BXJGShopOrders_OrderId",
                table: "BXJGShopOrderItems",
                column: "OrderId",
                principalTable: "BXJGShopOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BXJGShopOrders_BXJGBaseInfoAdministratives_AreaId",
                table: "BXJGShopOrders",
                column: "AreaId",
                principalTable: "BXJGBaseInfoAdministratives",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BXJGShopOrders_BXJGShopCustomers_CustomerId",
                table: "BXJGShopOrders",
                column: "CustomerId",
                principalTable: "BXJGShopCustomers",
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

            migrationBuilder.AddForeignKey(
                name: "FK_BXJGShopProductCategories_BXJGShopProductCategories_ParentId",
                table: "BXJGShopProductCategories",
                column: "ParentId",
                principalTable: "BXJGShopProductCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BXJGShopSku_BXJGShopItems_ProductId",
                table: "BXJGShopSku",
                column: "ProductId",
                principalTable: "BXJGShopItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
