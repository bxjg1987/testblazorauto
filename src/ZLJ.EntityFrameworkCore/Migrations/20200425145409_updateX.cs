using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZLJ.Migrations
{
    public partial class updateX : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "AreaId",
                table: "BXJGShopOrders",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "AdministrativeEntity",
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
                    Level = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdministrativeEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdministrativeEntity_AdministrativeEntity_ParentId",
                        column: x => x.ParentId,
                        principalTable: "AdministrativeEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BXJGShopOrders_AreaId",
                table: "BXJGShopOrders",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_AdministrativeEntity_ParentId",
                table: "AdministrativeEntity",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_BXJGShopOrders_AdministrativeEntity_AreaId",
                table: "BXJGShopOrders",
                column: "AreaId",
                principalTable: "AdministrativeEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BXJGShopOrders_AdministrativeEntity_AreaId",
                table: "BXJGShopOrders");

            migrationBuilder.DropTable(
                name: "AdministrativeEntity");

            migrationBuilder.DropIndex(
                name: "IX_BXJGShopOrders_AreaId",
                table: "BXJGShopOrders");

            migrationBuilder.DropColumn(
                name: "AreaId",
                table: "BXJGShopOrders");
        }
    }
}
