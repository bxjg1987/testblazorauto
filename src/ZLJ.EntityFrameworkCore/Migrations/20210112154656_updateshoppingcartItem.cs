using Microsoft.EntityFrameworkCore.Migrations;

namespace ZLJ.Migrations
{
    public partial class updateshoppingcartItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Quantity",
                table: "ShoppingCartItemEntity",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "ShoppingCartItemEntity",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "ExtensionData",
                table: "ShoppingCartItemEntity",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IntegralTotal",
                table: "ShoppingCartItemEntity",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "ShoppingCartItemEntity",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "ShoppingCartEntity",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "ExtensionData",
                table: "ShoppingCartEntity",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IntegralTotal",
                table: "ShoppingCartEntity",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "ShoppingCartItemEntity");

            migrationBuilder.DropColumn(
                name: "ExtensionData",
                table: "ShoppingCartItemEntity");

            migrationBuilder.DropColumn(
                name: "IntegralTotal",
                table: "ShoppingCartItemEntity");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "ShoppingCartItemEntity");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "ShoppingCartEntity");

            migrationBuilder.DropColumn(
                name: "ExtensionData",
                table: "ShoppingCartEntity");

            migrationBuilder.DropColumn(
                name: "IntegralTotal",
                table: "ShoppingCartEntity");

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "ShoppingCartItemEntity",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }
    }
}
