using Microsoft.EntityFrameworkCore.Migrations;

namespace ZLJ.Migrations
{
    public partial class updatesku : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DynamicEntityPropertyValue1",
                table: "BXJGShopSku");

            migrationBuilder.DropColumn(
                name: "DynamicEntityPropertyValue2",
                table: "BXJGShopSku");

            migrationBuilder.DropColumn(
                name: "DynamicEntityPropertyValue3",
                table: "BXJGShopSku");

            migrationBuilder.DropColumn(
                name: "DynamicEntityPropertyValue4",
                table: "BXJGShopSku");

            migrationBuilder.DropColumn(
                name: "DynamicEntityPropertyValue5",
                table: "BXJGShopSku");

            migrationBuilder.AddColumn<string>(
                name: "DynamicEntityProperty1Value",
                table: "BXJGShopSku",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DynamicEntityProperty2Value",
                table: "BXJGShopSku",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DynamicEntityProperty3Value",
                table: "BXJGShopSku",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DynamicEntityProperty4Value",
                table: "BXJGShopSku",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DynamicEntityProperty5Value",
                table: "BXJGShopSku",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DynamicEntityProperty1Value",
                table: "BXJGShopSku");

            migrationBuilder.DropColumn(
                name: "DynamicEntityProperty2Value",
                table: "BXJGShopSku");

            migrationBuilder.DropColumn(
                name: "DynamicEntityProperty3Value",
                table: "BXJGShopSku");

            migrationBuilder.DropColumn(
                name: "DynamicEntityProperty4Value",
                table: "BXJGShopSku");

            migrationBuilder.DropColumn(
                name: "DynamicEntityProperty5Value",
                table: "BXJGShopSku");

            migrationBuilder.AddColumn<string>(
                name: "DynamicEntityPropertyValue1",
                table: "BXJGShopSku",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DynamicEntityPropertyValue2",
                table: "BXJGShopSku",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DynamicEntityPropertyValue3",
                table: "BXJGShopSku",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DynamicEntityPropertyValue4",
                table: "BXJGShopSku",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DynamicEntityPropertyValue5",
                table: "BXJGShopSku",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
