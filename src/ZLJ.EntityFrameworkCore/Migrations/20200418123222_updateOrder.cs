using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZLJ.Migrations
{
    public partial class updateOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ZipCode",
                table: "BXJGShopOrders");

            migrationBuilder.AlterColumn<int>(
                name: "PaymentStatus",
                table: "BXJGShopOrders",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "OrderTime",
                table: "BXJGShopOrders",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderTime",
                table: "BXJGShopOrders");

            migrationBuilder.AlterColumn<int>(
                name: "PaymentStatus",
                table: "BXJGShopOrders",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ZipCode",
                table: "BXJGShopOrders",
                type: "varchar(50)",
                nullable: true);
        }
    }
}
