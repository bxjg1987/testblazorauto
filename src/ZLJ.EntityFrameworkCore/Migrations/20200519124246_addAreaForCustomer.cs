using Microsoft.EntityFrameworkCore.Migrations;

namespace ZLJ.Migrations
{
    public partial class addAreaForCustomer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Gender",
                table: "BXJGShopCustomers",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<long>(
                name: "AreaId",
                table: "BXJGShopCustomers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BXJGShopOrders_OrderNo",
                table: "BXJGShopOrders",
                column: "OrderNo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BXJGShopCustomers_AreaId",
                table: "BXJGShopCustomers",
                column: "AreaId");

            migrationBuilder.AddForeignKey(
                name: "FK_BXJGShopCustomers_Administratives_AreaId",
                table: "BXJGShopCustomers",
                column: "AreaId",
                principalTable: "Administratives",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BXJGShopCustomers_Administratives_AreaId",
                table: "BXJGShopCustomers");

            migrationBuilder.DropIndex(
                name: "IX_BXJGShopOrders_OrderNo",
                table: "BXJGShopOrders");

            migrationBuilder.DropIndex(
                name: "IX_BXJGShopCustomers_AreaId",
                table: "BXJGShopCustomers");

            migrationBuilder.DropColumn(
                name: "AreaId",
                table: "BXJGShopCustomers");

            migrationBuilder.AlterColumn<int>(
                name: "Gender",
                table: "BXJGShopCustomers",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
