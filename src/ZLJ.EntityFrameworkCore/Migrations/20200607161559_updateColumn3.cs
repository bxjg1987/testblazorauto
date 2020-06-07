using Microsoft.EntityFrameworkCore.Migrations;

namespace ZLJ.Migrations
{
    public partial class updateColumn3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SystemDefine",
                table: "BXJGCMSColumns");

            migrationBuilder.AddColumn<bool>(
                name: "IsSysDefine",
                table: "BXJGCMSColumns",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSysDefine",
                table: "BXJGCMSColumns");

            migrationBuilder.AddColumn<bool>(
                name: "SystemDefine",
                table: "BXJGCMSColumns",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
