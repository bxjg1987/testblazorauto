using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZLJ.Migrations
{
    public partial class addShop : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BXJGShopCustomers",
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
                    UserId = table.Column<long>(nullable: false),
                    Integral = table.Column<long>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false),
                    Gender = table.Column<int>(nullable: false),
                    Birthday = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BXJGShopCustomers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BXJGShopCustomers_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BXJGShopDictionaries",
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
                    Code = table.Column<string>(maxLength: 95, nullable: false),
                    ParentId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    DisplayName = table.Column<string>(maxLength: 128, nullable: false),
                    ExtensionData = table.Column<string>(nullable: true),
                    Icon = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BXJGShopDictionaries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BXJGShopDictionaries_BXJGShopDictionaries_ParentId",
                        column: x => x.ParentId,
                        principalTable: "BXJGShopDictionaries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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

            migrationBuilder.CreateTable(
                name: "BXJGShopOrders",
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
                    CustomerId = table.Column<long>(nullable: false),
                    OrderNo = table.Column<string>(type: "varchar(36)", nullable: true),
                    Status = table.Column<int>(nullable: false),
                    CustomerRemark = table.Column<string>(maxLength: 500, nullable: true),
                    MerchandiseSubtotal = table.Column<decimal>(nullable: false),
                    DistributionFee = table.Column<decimal>(nullable: false),
                    InvoiceRequired = table.Column<bool>(nullable: false),
                    InvoiceTax = table.Column<decimal>(nullable: false),
                    Integral = table.Column<long>(nullable: false),
                    PaymentMethodId = table.Column<long>(nullable: true),
                    PaymentAmount = table.Column<decimal>(nullable: false),
                    PaymentStatus = table.Column<int>(nullable: false),
                    Consignee = table.Column<string>(maxLength: 20, nullable: true),
                    ConsigneePhoneNumber = table.Column<string>(type: "varchar(50)", nullable: true),
                    ReceivingAddress = table.Column<string>(maxLength: 200, nullable: true),
                    ZipCode = table.Column<string>(type: "varchar(50)", nullable: true),
                    DistributionMethodId = table.Column<long>(nullable: true),
                    LogisticsNumber = table.Column<string>(type: "varchar(50)", nullable: true),
                    LogisticsStatus = table.Column<int>(nullable: true),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BXJGShopOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BXJGShopOrders_BXJGShopCustomers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "BXJGShopCustomers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BXJGShopOrders_BXJGShopDictionaries_DistributionMethodId",
                        column: x => x.DistributionMethodId,
                        principalTable: "BXJGShopDictionaries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BXJGShopOrders_BXJGShopDictionaries_PaymentMethodId",
                        column: x => x.PaymentMethodId,
                        principalTable: "BXJGShopDictionaries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BXJGShopOrderItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<long>(nullable: false),
                    ItemId = table.Column<long>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Image = table.Column<string>(nullable: true),
                    Price = table.Column<decimal>(nullable: false),
                    Quantity = table.Column<decimal>(nullable: false),
                    Integral = table.Column<int>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false),
                    TotalIntegral = table.Column<int>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BXJGShopOrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BXJGShopOrderItems_BXJGShopItems_ItemId",
                        column: x => x.ItemId,
                        principalTable: "BXJGShopItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BXJGShopOrderItems_BXJGShopOrders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "BXJGShopOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BXJGShopCustomers_UserId",
                table: "BXJGShopCustomers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BXJGShopDictionaries_ParentId",
                table: "BXJGShopDictionaries",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_BXJGShopItems_CategoryId",
                table: "BXJGShopItems",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_BXJGShopOrderItems_ItemId",
                table: "BXJGShopOrderItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_BXJGShopOrderItems_OrderId",
                table: "BXJGShopOrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_BXJGShopOrders_CustomerId",
                table: "BXJGShopOrders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_BXJGShopOrders_DistributionMethodId",
                table: "BXJGShopOrders",
                column: "DistributionMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_BXJGShopOrders_PaymentMethodId",
                table: "BXJGShopOrders",
                column: "PaymentMethodId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BXJGShopOrderItems");

            migrationBuilder.DropTable(
                name: "BXJGShopItems");

            migrationBuilder.DropTable(
                name: "BXJGShopOrders");

            migrationBuilder.DropTable(
                name: "BXJGShopCustomers");

            migrationBuilder.DropTable(
                name: "BXJGShopDictionaries");
        }
    }
}
