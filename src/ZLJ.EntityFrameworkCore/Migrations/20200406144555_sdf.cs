using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZLJ.Migrations
{
    public partial class sdf : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BXJGShopItems",
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
                    Sku = table.Column<string>(maxLength: 50, nullable: true),
                    DescriptionShort = table.Column<string>(maxLength: 10000, nullable: true),
                    DescriptionFull = table.Column<string>(nullable: true),
                    Images = table.Column<string>(maxLength: 5000, nullable: true),
                    CategoryId = table.Column<long>(nullable: false),
                    OldPrice = table.Column<decimal>(nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    Integral = table.Column<int>(nullable: false),
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
                    table.PrimaryKey("PK_BXJGShopItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BXJGShopItems_BXJGShopDictionaries_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "BXJGShopDictionaries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BXJGShopItems_CategoryId",
                table: "BXJGShopItems",
                column: "CategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BXJGShopItems");
        }
    }
}
