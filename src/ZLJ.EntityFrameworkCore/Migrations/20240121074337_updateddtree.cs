using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZLJ.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class updateddtree : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_baseinfo_associated_company_BXJGGeneralTrees_CategoryId",
                table: "baseinfo_associated_company");

            migrationBuilder.DropForeignKey(
                name: "FK_baseinfo_associated_company_BXJGGeneralTrees_LevelId",
                table: "baseinfo_associated_company");

            migrationBuilder.DropTable(
                name: "BXJGGeneralTrees");

            migrationBuilder.CreateTable(
                name: "bxjg_utils_data_dictionary",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsSysDefine = table.Column<bool>(type: "bit", nullable: false),
                    IsTree = table.Column<bool>(type: "bit", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(95)", maxLength: 95, nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    ChildrenCount = table.Column<int>(type: "int", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ExtensionData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bxjg_utils_data_dictionary", x => x.Id);
                    table.ForeignKey(
                        name: "FK_bxjg_utils_data_dictionary_bxjg_utils_data_dictionary_ParentId",
                        column: x => x.ParentId,
                        principalTable: "bxjg_utils_data_dictionary",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_bxjg_utils_data_dictionary_Code",
                table: "bxjg_utils_data_dictionary",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_bxjg_utils_data_dictionary_ParentId",
                table: "bxjg_utils_data_dictionary",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_baseinfo_associated_company_bxjg_utils_data_dictionary_CategoryId",
                table: "baseinfo_associated_company",
                column: "CategoryId",
                principalTable: "bxjg_utils_data_dictionary",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_baseinfo_associated_company_bxjg_utils_data_dictionary_LevelId",
                table: "baseinfo_associated_company",
                column: "LevelId",
                principalTable: "bxjg_utils_data_dictionary",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_baseinfo_associated_company_bxjg_utils_data_dictionary_CategoryId",
                table: "baseinfo_associated_company");

            migrationBuilder.DropForeignKey(
                name: "FK_baseinfo_associated_company_bxjg_utils_data_dictionary_LevelId",
                table: "baseinfo_associated_company");

            migrationBuilder.DropTable(
                name: "bxjg_utils_data_dictionary");

            migrationBuilder.CreateTable(
                name: "BXJGGeneralTrees",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentId = table.Column<long>(type: "bigint", nullable: true),
                    ChildrenCount = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(95)", maxLength: 95, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DisplayName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ExtensionData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsSysDefine = table.Column<bool>(type: "bit", nullable: false),
                    IsTree = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BXJGGeneralTrees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BXJGGeneralTrees_BXJGGeneralTrees_ParentId",
                        column: x => x.ParentId,
                        principalTable: "BXJGGeneralTrees",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BXJGGeneralTrees_Code",
                table: "BXJGGeneralTrees",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BXJGGeneralTrees_ParentId",
                table: "BXJGGeneralTrees",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_baseinfo_associated_company_BXJGGeneralTrees_CategoryId",
                table: "baseinfo_associated_company",
                column: "CategoryId",
                principalTable: "BXJGGeneralTrees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_baseinfo_associated_company_BXJGGeneralTrees_LevelId",
                table: "baseinfo_associated_company",
                column: "LevelId",
                principalTable: "BXJGGeneralTrees",
                principalColumn: "Id");
        }
    }
}
