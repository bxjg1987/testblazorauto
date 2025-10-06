using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZLJ.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class kehuupdate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AbpOrganizationUnits_baseinfo_associated_company_CustomerId",
                table: "AbpOrganizationUnits");

            migrationBuilder.DropForeignKey(
                name: "FK_AbpUsers_baseinfo_associated_company_CustomerId",
                table: "AbpUsers");

            migrationBuilder.DropTable(
                name: "baseinfo_associated_company");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "BXJGUtilsFeedbacks",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                defaultValue: "",
                comment: "内容",
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000,
                oldNullable: true,
                oldComment: "内容");

            migrationBuilder.CreateTable(
                name: "KehuXinxi",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Pinyin = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    TaxNo = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    LinkMan = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    LinkManPinyin = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    LinkPhone = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    AddressPinyin = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Lng = table.Column<decimal>(type: "decimal(32,24)", precision: 18, scale: 2, nullable: true),
                    Lat = table.Column<decimal>(type: "decimal(32,24)", precision: 18, scale: 2, nullable: true),
                    AreaId = table.Column<long>(type: "bigint", nullable: true),
                    LevelId = table.Column<long>(type: "bigint", nullable: true),
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
                    table.PrimaryKey("PK_KehuXinxi", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KehuXinxi_BXJGUtilsDataDictionaries_LevelId",
                        column: x => x.LevelId,
                        principalTable: "BXJGUtilsDataDictionaries",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_KehuXinxi_baseinfo_administrative_AreaId",
                        column: x => x.AreaId,
                        principalTable: "baseinfo_administrative",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_KehuXinxi_AreaId",
                table: "KehuXinxi",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_KehuXinxi_LevelId",
                table: "KehuXinxi",
                column: "LevelId");

            migrationBuilder.CreateIndex(
                name: "IX_KehuXinxi_TenantId_Name_IsDeleted",
                table: "KehuXinxi",
                columns: new[] { "TenantId", "Name", "IsDeleted" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AbpOrganizationUnits_KehuXinxi_CustomerId",
                table: "AbpOrganizationUnits",
                column: "CustomerId",
                principalTable: "KehuXinxi",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AbpUsers_KehuXinxi_CustomerId",
                table: "AbpUsers",
                column: "CustomerId",
                principalTable: "KehuXinxi",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AbpOrganizationUnits_KehuXinxi_CustomerId",
                table: "AbpOrganizationUnits");

            migrationBuilder.DropForeignKey(
                name: "FK_AbpUsers_KehuXinxi_CustomerId",
                table: "AbpUsers");

            migrationBuilder.DropTable(
                name: "KehuXinxi");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "BXJGUtilsFeedbacks",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true,
                comment: "内容",
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000,
                oldComment: "内容");

            migrationBuilder.CreateTable(
                name: "baseinfo_associated_company",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdminId = table.Column<long>(type: "bigint", nullable: true),
                    AreaId = table.Column<long>(type: "bigint", nullable: true),
                    CategoryId = table.Column<long>(type: "bigint", nullable: true),
                    LevelId = table.Column<long>(type: "bigint", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    Lat = table.Column<decimal>(type: "decimal(32,24)", precision: 18, scale: 2, nullable: true),
                    LinkMan = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    LinkPhone = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    Lng = table.Column<decimal>(type: "decimal(32,24)", precision: 18, scale: 2, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Pinyin = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    TaxNo = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_baseinfo_associated_company", x => x.Id);
                    table.ForeignKey(
                        name: "FK_baseinfo_associated_company_AbpUsers_AdminId",
                        column: x => x.AdminId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_baseinfo_associated_company_BXJGUtilsDataDictionaries_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "BXJGUtilsDataDictionaries",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_baseinfo_associated_company_BXJGUtilsDataDictionaries_LevelId",
                        column: x => x.LevelId,
                        principalTable: "BXJGUtilsDataDictionaries",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_baseinfo_associated_company_baseinfo_administrative_AreaId",
                        column: x => x.AreaId,
                        principalTable: "baseinfo_administrative",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_baseinfo_associated_company_AdminId",
                table: "baseinfo_associated_company",
                column: "AdminId",
                unique: true,
                filter: "[AdminId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_baseinfo_associated_company_AreaId",
                table: "baseinfo_associated_company",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_baseinfo_associated_company_CategoryId",
                table: "baseinfo_associated_company",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_baseinfo_associated_company_LevelId",
                table: "baseinfo_associated_company",
                column: "LevelId");

            migrationBuilder.AddForeignKey(
                name: "FK_AbpOrganizationUnits_baseinfo_associated_company_CustomerId",
                table: "AbpOrganizationUnits",
                column: "CustomerId",
                principalTable: "baseinfo_associated_company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AbpUsers_baseinfo_associated_company_CustomerId",
                table: "AbpUsers",
                column: "CustomerId",
                principalTable: "baseinfo_associated_company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
