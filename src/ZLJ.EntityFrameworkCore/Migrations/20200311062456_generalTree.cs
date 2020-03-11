using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZLJ.Migrations
{
    public partial class generalTree : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AbpOrganizationUnits_TenantId_Code",
                table: "AbpOrganizationUnits");

            migrationBuilder.CreateTable(
                name: "BXJGGeneralTrees",
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
                    IsSysDefine = table.Column<bool>(nullable: false),
                    IsTree = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BXJGGeneralTrees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BXJGGeneralTrees_BXJGGeneralTrees_ParentId",
                        column: x => x.ParentId,
                        principalTable: "BXJGGeneralTrees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AbpOrganizationUnits_TenantId_Code",
                table: "AbpOrganizationUnits",
                columns: new[] { "TenantId", "Code" },
                unique: true,
                filter: "[TenantId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BXJGGeneralTrees_ParentId",
                table: "BXJGGeneralTrees",
                column: "ParentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BXJGGeneralTrees");

            migrationBuilder.DropIndex(
                name: "IX_AbpOrganizationUnits_TenantId_Code",
                table: "AbpOrganizationUnits");

            migrationBuilder.CreateIndex(
                name: "IX_AbpOrganizationUnits_TenantId_Code",
                table: "AbpOrganizationUnits",
                columns: new[] { "TenantId", "Code" });
        }
    }
}
