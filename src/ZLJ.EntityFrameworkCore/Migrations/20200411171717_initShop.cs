using Microsoft.EntityFrameworkCore.Migrations;

namespace ZLJ.Migrations
{
    public partial class initShop : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BXJGShopOrders",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: false),
                    CustomerId = table.Column<long>(nullable: false),
                    OrderNo = table.Column<string>(nullable: true),
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
                    Consignee = table.Column<string>(nullable: true),
                    ConsigneePhoneNumber = table.Column<string>(nullable: true),
                    ReceivingAddress = table.Column<string>(nullable: true),
                    ZipCode = table.Column<string>(nullable: true),
                    DistributionMethodId = table.Column<long>(nullable: true),
                    LogisticsNumber = table.Column<string>(nullable: true),
                    LogisticsStatus = table.Column<int>(nullable: true)
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
                name: "BXJGShopOrders");
        }
    }
}
