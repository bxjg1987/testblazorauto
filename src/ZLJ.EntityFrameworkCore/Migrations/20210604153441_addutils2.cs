using Microsoft.EntityFrameworkCore.Migrations;

namespace ZLJ.Migrations
{
    public partial class addutils2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Attachments",
                table: "Attachments");

            migrationBuilder.RenameTable(
                name: "Attachments",
                newName: "BXJGAttachments");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BXJGAttachments",
                table: "BXJGAttachments",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_BXJGAttachments",
                table: "BXJGAttachments");

            migrationBuilder.RenameTable(
                name: "BXJGAttachments",
                newName: "Attachments");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Attachments",
                table: "Attachments",
                column: "Id");
        }
    }
}
