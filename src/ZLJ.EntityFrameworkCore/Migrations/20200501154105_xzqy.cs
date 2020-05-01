using Microsoft.EntityFrameworkCore.Migrations;

namespace ZLJ.Migrations
{
    public partial class xzqy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdministrativeEntity_AdministrativeEntity_ParentId",
                table: "AdministrativeEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_BXJGShopOrders_AdministrativeEntity_AreaId",
                table: "BXJGShopOrders");

            migrationBuilder.DropIndex(
                name: "IX_AbpOrganizationUnits_TenantId_Code",
                table: "AbpOrganizationUnits");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AdministrativeEntity",
                table: "AdministrativeEntity");

            migrationBuilder.RenameTable(
                name: "AdministrativeEntity",
                newName: "Administratives");

            migrationBuilder.RenameIndex(
                name: "IX_AdministrativeEntity_ParentId",
                table: "Administratives",
                newName: "IX_Administratives_ParentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Administratives",
                table: "Administratives",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_AbpOrganizationUnits_TenantId_Code",
                table: "AbpOrganizationUnits",
                columns: new[] { "TenantId", "Code" });

            migrationBuilder.AddForeignKey(
                name: "FK_Administratives_Administratives_ParentId",
                table: "Administratives",
                column: "ParentId",
                principalTable: "Administratives",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BXJGShopOrders_Administratives_AreaId",
                table: "BXJGShopOrders",
                column: "AreaId",
                principalTable: "Administratives",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Administratives_Administratives_ParentId",
                table: "Administratives");

            migrationBuilder.DropForeignKey(
                name: "FK_BXJGShopOrders_Administratives_AreaId",
                table: "BXJGShopOrders");

            migrationBuilder.DropIndex(
                name: "IX_AbpOrganizationUnits_TenantId_Code",
                table: "AbpOrganizationUnits");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Administratives",
                table: "Administratives");

            migrationBuilder.RenameTable(
                name: "Administratives",
                newName: "AdministrativeEntity");

            migrationBuilder.RenameIndex(
                name: "IX_Administratives_ParentId",
                table: "AdministrativeEntity",
                newName: "IX_AdministrativeEntity_ParentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AdministrativeEntity",
                table: "AdministrativeEntity",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_AbpOrganizationUnits_TenantId_Code",
                table: "AbpOrganizationUnits",
                columns: new[] { "TenantId", "Code" },
                unique: true,
                filter: "[TenantId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AdministrativeEntity_AdministrativeEntity_ParentId",
                table: "AdministrativeEntity",
                column: "ParentId",
                principalTable: "AdministrativeEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BXJGShopOrders_AdministrativeEntity_AreaId",
                table: "BXJGShopOrders",
                column: "AreaId",
                principalTable: "AdministrativeEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
