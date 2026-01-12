using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZLJ.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class update8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ZLJ_TestTree_Name",
                table: "ZLJ_TestTree",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_BXJGUtilsDataDictionaries_Name",
                table: "BXJGUtilsDataDictionaries",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_baseinfo_administrative_Name",
                table: "baseinfo_administrative",
                column: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ZLJ_TestTree_Name",
                table: "ZLJ_TestTree");

            migrationBuilder.DropIndex(
                name: "IX_BXJGUtilsDataDictionaries_Name",
                table: "BXJGUtilsDataDictionaries");

            migrationBuilder.DropIndex(
                name: "IX_baseinfo_administrative_Name",
                table: "baseinfo_administrative");
        }
    }
}
