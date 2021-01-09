using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZLJ.Migrations
{
    public partial class addShippingAddress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "DynamicProperty1Id",
                table: "BXJGShopSku",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "bxjgShippingAddress",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    CustomerId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ZipCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    ExtensionData = table.Column<string>(type: "nvarchar(max)", maxLength: 2147483647, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bxjgShippingAddress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_bxjgShippingAddress_BXJGShopCustomer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "BXJGShopCustomer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingCartEntity",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    CustomerId = table.Column<long>(type: "bigint", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingCartEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShoppingCartEntity_BXJGShopCustomer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "BXJGShopCustomer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingCartItemEntity",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShoppingCartId = table.Column<long>(type: "bigint", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    SkuId = table.Column<long>(type: "bigint", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingCartItemEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShoppingCartItemEntity_BXJGShopProduct_ProductId",
                        column: x => x.ProductId,
                        principalTable: "BXJGShopProduct",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShoppingCartItemEntity_BXJGShopSku_SkuId",
                        column: x => x.SkuId,
                        principalTable: "BXJGShopSku",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShoppingCartItemEntity_ShoppingCartEntity_ShoppingCartId",
                        column: x => x.ShoppingCartId,
                        principalTable: "ShoppingCartEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_bxjgShippingAddress_CustomerId",
                table: "bxjgShippingAddress",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCartEntity_CustomerId",
                table: "ShoppingCartEntity",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCartItemEntity_ProductId",
                table: "ShoppingCartItemEntity",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCartItemEntity_ShoppingCartId",
                table: "ShoppingCartItemEntity",
                column: "ShoppingCartId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCartItemEntity_SkuId",
                table: "ShoppingCartItemEntity",
                column: "SkuId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "bxjgShippingAddress");

            migrationBuilder.DropTable(
                name: "ShoppingCartItemEntity");

            migrationBuilder.DropTable(
                name: "ShoppingCartEntity");

            migrationBuilder.AlterColumn<int>(
                name: "DynamicProperty1Id",
                table: "BXJGShopSku",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
