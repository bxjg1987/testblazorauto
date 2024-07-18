using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZLJ.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class updategt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ZLJ_TestTree_Code",
                table: "ZLJ_TestTree");

            migrationBuilder.DropIndex(
                name: "IX_BXJGUtilsDataDictionaries_Code",
                table: "BXJGUtilsDataDictionaries");

            migrationBuilder.DropIndex(
                name: "IX_baseinfo_administrative_Code",
                table: "baseinfo_administrative");

            migrationBuilder.CreateIndex(
                name: "IX_ZLJ_TestTree_Code",
                table: "ZLJ_TestTree",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_BXJGUtilsDataDictionaries_Code",
                table: "BXJGUtilsDataDictionaries",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_baseinfo_administrative_Code",
                table: "baseinfo_administrative",
                column: "Code");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ZLJ_TestTree_Code",
                table: "ZLJ_TestTree");

            migrationBuilder.DropIndex(
                name: "IX_BXJGUtilsDataDictionaries_Code",
                table: "BXJGUtilsDataDictionaries");

            migrationBuilder.DropIndex(
                name: "IX_baseinfo_administrative_Code",
                table: "baseinfo_administrative");

            migrationBuilder.CreateIndex(
                name: "IX_ZLJ_TestTree_Code",
                table: "ZLJ_TestTree",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BXJGUtilsDataDictionaries_Code",
                table: "BXJGUtilsDataDictionaries",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_baseinfo_administrative_Code",
                table: "baseinfo_administrative",
                column: "Code",
                unique: true);
        }
    }
}
