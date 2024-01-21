using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZLJ.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class updateddtree2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_baseinfo_associated_company_bxjg_utils_data_dictionary_CategoryId",
                table: "baseinfo_associated_company");

            migrationBuilder.DropForeignKey(
                name: "FK_baseinfo_associated_company_bxjg_utils_data_dictionary_LevelId",
                table: "baseinfo_associated_company");

            migrationBuilder.DropForeignKey(
                name: "FK_bxjg_utils_data_dictionary_bxjg_utils_data_dictionary_ParentId",
                table: "bxjg_utils_data_dictionary");

            migrationBuilder.DropPrimaryKey(
                name: "PK_bxjg_utils_data_dictionary",
                table: "bxjg_utils_data_dictionary");

            migrationBuilder.RenameTable(
                name: "bxjg_utils_data_dictionary",
                newName: "BXJGGeneralTrees");

            migrationBuilder.RenameIndex(
                name: "IX_bxjg_utils_data_dictionary_ParentId",
                table: "BXJGGeneralTrees",
                newName: "IX_BXJGGeneralTrees_ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_bxjg_utils_data_dictionary_Code",
                table: "BXJGGeneralTrees",
                newName: "IX_BXJGGeneralTrees_Code");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BXJGGeneralTrees",
                table: "BXJGGeneralTrees",
                column: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_BXJGGeneralTrees_BXJGGeneralTrees_ParentId",
                table: "BXJGGeneralTrees",
                column: "ParentId",
                principalTable: "BXJGGeneralTrees",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_baseinfo_associated_company_BXJGGeneralTrees_CategoryId",
                table: "baseinfo_associated_company");

            migrationBuilder.DropForeignKey(
                name: "FK_baseinfo_associated_company_BXJGGeneralTrees_LevelId",
                table: "baseinfo_associated_company");

            migrationBuilder.DropForeignKey(
                name: "FK_BXJGGeneralTrees_BXJGGeneralTrees_ParentId",
                table: "BXJGGeneralTrees");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BXJGGeneralTrees",
                table: "BXJGGeneralTrees");

            migrationBuilder.RenameTable(
                name: "BXJGGeneralTrees",
                newName: "bxjg_utils_data_dictionary");

            migrationBuilder.RenameIndex(
                name: "IX_BXJGGeneralTrees_ParentId",
                table: "bxjg_utils_data_dictionary",
                newName: "IX_bxjg_utils_data_dictionary_ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_BXJGGeneralTrees_Code",
                table: "bxjg_utils_data_dictionary",
                newName: "IX_bxjg_utils_data_dictionary_Code");

            migrationBuilder.AddPrimaryKey(
                name: "PK_bxjg_utils_data_dictionary",
                table: "bxjg_utils_data_dictionary",
                column: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_bxjg_utils_data_dictionary_bxjg_utils_data_dictionary_ParentId",
                table: "bxjg_utils_data_dictionary",
                column: "ParentId",
                principalTable: "bxjg_utils_data_dictionary",
                principalColumn: "Id");
        }
    }
}
