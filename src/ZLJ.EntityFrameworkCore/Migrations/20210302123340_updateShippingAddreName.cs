using Microsoft.EntityFrameworkCore.Migrations;

namespace ZLJ.Migrations
{
    public partial class updateShippingAddreName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BXJGShippingAddress_BXJGShopCustomer_CustomerId",
                table: "BXJGShippingAddress");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BXJGShippingAddress",
                table: "BXJGShippingAddress");

            migrationBuilder.RenameTable(
                name: "BXJGShippingAddress",
                newName: "BXJGShopShippingAddress");

            migrationBuilder.RenameIndex(
                name: "IX_BXJGShippingAddress_CustomerId",
                table: "BXJGShopShippingAddress",
                newName: "IX_BXJGShopShippingAddress_CustomerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BXJGShopShippingAddress",
                table: "BXJGShopShippingAddress",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BXJGShopShippingAddress_BXJGShopCustomer_CustomerId",
                table: "BXJGShopShippingAddress",
                column: "CustomerId",
                principalTable: "BXJGShopCustomer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BXJGShopShippingAddress_BXJGShopCustomer_CustomerId",
                table: "BXJGShopShippingAddress");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BXJGShopShippingAddress",
                table: "BXJGShopShippingAddress");

            migrationBuilder.RenameTable(
                name: "BXJGShopShippingAddress",
                newName: "BXJGShippingAddress");

            migrationBuilder.RenameIndex(
                name: "IX_BXJGShopShippingAddress_CustomerId",
                table: "BXJGShippingAddress",
                newName: "IX_BXJGShippingAddress_CustomerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BXJGShippingAddress",
                table: "BXJGShippingAddress",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BXJGShippingAddress_BXJGShopCustomer_CustomerId",
                table: "BXJGShippingAddress",
                column: "CustomerId",
                principalTable: "BXJGShopCustomer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
