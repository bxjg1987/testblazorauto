using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZLJ.Migrations
{
    public partial class updateitemtoproduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SkuEntity_BXJGShopProducts_ProductId",
                table: "SkuEntity");

            migrationBuilder.DropTable(
                name: "BXJGShopProducts");

            migrationBuilder.DropColumn(
                name: "Sku",
                table: "BXJGShopItems");

            migrationBuilder.AddForeignKey(
                name: "FK_SkuEntity_BXJGShopItems_ProductId",
                table: "SkuEntity",
                column: "ProductId",
                principalTable: "BXJGShopItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SkuEntity_BXJGShopItems_ProductId",
                table: "SkuEntity");

            migrationBuilder.AddColumn<string>(
                name: "Sku",
                table: "BXJGShopItems",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BXJGShopProducts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AvailableEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    AvailableStart = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    BrandId = table.Column<long>(type: "bigint", nullable: true),
                    CategoryId = table.Column<long>(type: "bigint", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DescriptionFull = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionShort = table.Column<string>(type: "nvarchar(max)", maxLength: 10000, nullable: true),
                    Focus = table.Column<bool>(type: "bit", nullable: false),
                    Home = table.Column<bool>(type: "bit", nullable: false),
                    Hot = table.Column<bool>(type: "bit", nullable: false),
                    Images = table.Column<string>(type: "varchar(5000)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    New = table.Column<bool>(type: "bit", nullable: false),
                    Published = table.Column<bool>(type: "bit", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UnitId = table.Column<long>(type: "bigint", nullable: true)
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
                        name: "FK_BXJGShopProducts_BXJGShopProductCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "BXJGShopProductCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BXJGShopProducts_BXJGGeneralTrees_UnitId",
                        column: x => x.UnitId,
                        principalTable: "BXJGGeneralTrees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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

            migrationBuilder.AddForeignKey(
                name: "FK_SkuEntity_BXJGShopProducts_ProductId",
                table: "SkuEntity",
                column: "ProductId",
                principalTable: "BXJGShopProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
