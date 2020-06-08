using Microsoft.EntityFrameworkCore.Migrations;

namespace ZLJ.Migrations
{
    public partial class updateArticle2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SystemDefine",
                table: "BXJGCMSArticles");

            migrationBuilder.AddColumn<long>(
                name: "ColumnId",
                table: "BXJGCMSArticles",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                name: "IsSysDefine",
                table: "BXJGCMSArticles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_BXJGCMSArticles_ColumnId",
                table: "BXJGCMSArticles",
                column: "ColumnId");

            migrationBuilder.AddForeignKey(
                name: "FK_BXJGCMSArticles_BXJGCMSColumns_ColumnId",
                table: "BXJGCMSArticles",
                column: "ColumnId",
                principalTable: "BXJGCMSColumns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BXJGCMSArticles_BXJGCMSColumns_ColumnId",
                table: "BXJGCMSArticles");

            migrationBuilder.DropIndex(
                name: "IX_BXJGCMSArticles_ColumnId",
                table: "BXJGCMSArticles");

            migrationBuilder.DropColumn(
                name: "ColumnId",
                table: "BXJGCMSArticles");

            migrationBuilder.DropColumn(
                name: "IsSysDefine",
                table: "BXJGCMSArticles");

            migrationBuilder.AddColumn<bool>(
                name: "SystemDefine",
                table: "BXJGCMSArticles",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
