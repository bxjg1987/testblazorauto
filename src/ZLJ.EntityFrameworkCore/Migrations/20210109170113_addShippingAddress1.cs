using Microsoft.EntityFrameworkCore.Migrations;

namespace ZLJ.Migrations
{
    public partial class addShippingAddress1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_bxjgShippingAddress_BXJGShopCustomer_CustomerId",
                table: "bxjgShippingAddress");

            migrationBuilder.DropPrimaryKey(
                name: "PK_bxjgShippingAddress",
                table: "bxjgShippingAddress");

            migrationBuilder.RenameTable(
                name: "bxjgShippingAddress",
                newName: "BXJGShippingAddress");

            migrationBuilder.RenameIndex(
                name: "IX_bxjgShippingAddress_CustomerId",
                table: "BXJGShippingAddress",
                newName: "IX_BXJGShippingAddress_CustomerId");

            migrationBuilder.AlterColumn<string>(
                name: "ZipCode",
                table: "BXJGShippingAddress",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "BXJGShippingAddress",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BXJGShippingAddress_BXJGShopCustomer_CustomerId",
                table: "BXJGShippingAddress");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BXJGShippingAddress",
                table: "BXJGShippingAddress");

            migrationBuilder.RenameTable(
                name: "BXJGShippingAddress",
                newName: "bxjgShippingAddress");

            migrationBuilder.RenameIndex(
                name: "IX_BXJGShippingAddress_CustomerId",
                table: "bxjgShippingAddress",
                newName: "IX_bxjgShippingAddress_CustomerId");

            migrationBuilder.AlterColumn<string>(
                name: "ZipCode",
                table: "bxjgShippingAddress",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "bxjgShippingAddress",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_bxjgShippingAddress",
                table: "bxjgShippingAddress",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_bxjgShippingAddress_BXJGShopCustomer_CustomerId",
                table: "bxjgShippingAddress",
                column: "CustomerId",
                principalTable: "BXJGShopCustomer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
