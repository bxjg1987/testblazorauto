using Microsoft.EntityFrameworkCore.Migrations;

namespace ZLJ.Migrations
{
    public partial class updateCustomer2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BXJGShippingAddress_BXJGBaseInfoAdministratives_AreaId",
                table: "BXJGShippingAddress");

            migrationBuilder.DropForeignKey(
                name: "FK_BXJGShopCustomer_BXJGBaseInfoAdministratives_AreaId",
                table: "BXJGShopCustomer");

            migrationBuilder.DropIndex(
                name: "IX_BXJGShopCustomer_AreaId",
                table: "BXJGShopCustomer");

            migrationBuilder.DropIndex(
                name: "IX_BXJGShippingAddress_AreaId",
                table: "BXJGShippingAddress");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_BXJGShopCustomer_AreaId",
                table: "BXJGShopCustomer",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_BXJGShippingAddress_AreaId",
                table: "BXJGShippingAddress",
                column: "AreaId");

            migrationBuilder.AddForeignKey(
                name: "FK_BXJGShippingAddress_BXJGBaseInfoAdministratives_AreaId",
                table: "BXJGShippingAddress",
                column: "AreaId",
                principalTable: "BXJGBaseInfoAdministratives",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BXJGShopCustomer_BXJGBaseInfoAdministratives_AreaId",
                table: "BXJGShopCustomer",
                column: "AreaId",
                principalTable: "BXJGBaseInfoAdministratives",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
