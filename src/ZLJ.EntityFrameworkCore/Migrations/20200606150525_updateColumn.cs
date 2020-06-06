using Microsoft.EntityFrameworkCore.Migrations;

namespace ZLJ.Migrations
{
    public partial class updateColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "BXJGCMSColumns");

            migrationBuilder.AlterColumn<string>(
                name: "SeoTitle",
                table: "BXJGCMSColumns",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SeoKeyword",
                table: "BXJGCMSColumns",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Icon",
                table: "BXJGCMSColumns",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ColumnType",
                table: "BXJGCMSColumns",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "ContentTypeId",
                table: "BXJGCMSColumns",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "DetailTemplate",
                table: "BXJGCMSColumns",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ListTemplate",
                table: "BXJGCMSColumns",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SystemDefine",
                table: "BXJGCMSColumns",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_BXJGCMSColumns_ContentTypeId",
                table: "BXJGCMSColumns",
                column: "ContentTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_BXJGCMSColumns_BXJGGeneralTrees_ContentTypeId",
                table: "BXJGCMSColumns",
                column: "ContentTypeId",
                principalTable: "BXJGGeneralTrees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BXJGCMSColumns_BXJGGeneralTrees_ContentTypeId",
                table: "BXJGCMSColumns");

            migrationBuilder.DropIndex(
                name: "IX_BXJGCMSColumns_ContentTypeId",
                table: "BXJGCMSColumns");

            migrationBuilder.DropColumn(
                name: "ColumnType",
                table: "BXJGCMSColumns");

            migrationBuilder.DropColumn(
                name: "ContentTypeId",
                table: "BXJGCMSColumns");

            migrationBuilder.DropColumn(
                name: "DetailTemplate",
                table: "BXJGCMSColumns");

            migrationBuilder.DropColumn(
                name: "ListTemplate",
                table: "BXJGCMSColumns");

            migrationBuilder.DropColumn(
                name: "SystemDefine",
                table: "BXJGCMSColumns");

            migrationBuilder.AlterColumn<string>(
                name: "SeoTitle",
                table: "BXJGCMSColumns",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SeoKeyword",
                table: "BXJGCMSColumns",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Icon",
                table: "BXJGCMSColumns",
                type: "varchar(200)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "BXJGCMSColumns",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);
        }
    }
}
