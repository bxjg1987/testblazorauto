using Microsoft.EntityFrameworkCore.Migrations;

namespace ZLJ.Migrations
{
    public partial class addColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ColumnEntity_ColumnEntity_ParentId",
                table: "ColumnEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ColumnEntity",
                table: "ColumnEntity");

            migrationBuilder.RenameTable(
                name: "ColumnEntity",
                newName: "BXJGCMSColumns");

            migrationBuilder.RenameIndex(
                name: "IX_ColumnEntity_ParentId",
                table: "BXJGCMSColumns",
                newName: "IX_BXJGCMSColumns_ParentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BXJGCMSColumns",
                table: "BXJGCMSColumns",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BXJGCMSColumns_BXJGCMSColumns_ParentId",
                table: "BXJGCMSColumns",
                column: "ParentId",
                principalTable: "BXJGCMSColumns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BXJGCMSColumns_BXJGCMSColumns_ParentId",
                table: "BXJGCMSColumns");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BXJGCMSColumns",
                table: "BXJGCMSColumns");

            migrationBuilder.RenameTable(
                name: "BXJGCMSColumns",
                newName: "ColumnEntity");

            migrationBuilder.RenameIndex(
                name: "IX_BXJGCMSColumns_ParentId",
                table: "ColumnEntity",
                newName: "IX_ColumnEntity_ParentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ColumnEntity",
                table: "ColumnEntity",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ColumnEntity_ColumnEntity_ParentId",
                table: "ColumnEntity",
                column: "ParentId",
                principalTable: "ColumnEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
