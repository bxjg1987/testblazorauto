using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZLJ.Migrations
{
    public partial class addProductAndSku : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExtensionData",
                table: "BXJGShopCustomers",
                maxLength: 2147483647,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BXJGShopProducts",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    Title = table.Column<string>(maxLength: 100, nullable: false),
                    DescriptionShort = table.Column<string>(maxLength: 10000, nullable: true),
                    DescriptionFull = table.Column<string>(nullable: true),
                    Images = table.Column<string>(type: "varchar(5000)", nullable: true),
                    CategoryId = table.Column<long>(nullable: false),
                    BrandId = table.Column<long>(nullable: true),
                    UnitId = table.Column<long>(nullable: true),
                    Hot = table.Column<bool>(nullable: false),
                    New = table.Column<bool>(nullable: false),
                    Home = table.Column<bool>(nullable: false),
                    Focus = table.Column<bool>(nullable: false),
                    Published = table.Column<bool>(nullable: false),
                    AvailableStart = table.Column<DateTimeOffset>(nullable: true),
                    AvailableEnd = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BXJGShopProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BXJGShopProducts_BXJGGeneralTrees_BrandId",
                        column: x => x.BrandId,
                        principalTable: "BXJGGeneralTrees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BXJGShopProducts_BXJGShopItemCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "BXJGShopItemCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BXJGShopProducts_BXJGGeneralTrees_UnitId",
                        column: x => x.UnitId,
                        principalTable: "BXJGGeneralTrees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SkuEntity",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OldPrice = table.Column<decimal>(nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    Integral = table.Column<int>(nullable: false),
                    ProductId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkuEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SkuEntity_BXJGShopProducts_ProductId",
                        column: x => x.ProductId,
                        principalTable: "BXJGShopProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BXJGShopProducts_BrandId",
                table: "BXJGShopProducts",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_BXJGShopProducts_CategoryId",
                table: "BXJGShopProducts",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_BXJGShopProducts_UnitId",
                table: "BXJGShopProducts",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_SkuEntity_ProductId",
                table: "SkuEntity",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SkuEntity");

            migrationBuilder.DropTable(
                name: "BXJGShopProducts");

            migrationBuilder.DropColumn(
                name: "ExtensionData",
                table: "BXJGShopCustomers");
        }
    }
}
