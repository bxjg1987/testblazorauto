using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZLJ.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class updateFileAttachment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BXJGUtilsAttachments_BXJGUtilsFiles_Id",
                table: "BXJGUtilsAttachments");

            migrationBuilder.AddColumn<Guid>(
                name: "FileId",
                table: "BXJGUtilsAttachments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "Parameters",
                table: "AbpAuditLogs",
                type: "nvarchar(max)",
                maxLength: 4096,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(2048)",
                oldMaxLength: 2048,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BXJGUtilsAttachments_FileId",
                table: "BXJGUtilsAttachments",
                column: "FileId");

            migrationBuilder.AddForeignKey(
                name: "FK_BXJGUtilsAttachments_BXJGUtilsFiles_FileId",
                table: "BXJGUtilsAttachments",
                column: "FileId",
                principalTable: "BXJGUtilsFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BXJGUtilsAttachments_BXJGUtilsFiles_FileId",
                table: "BXJGUtilsAttachments");

            migrationBuilder.DropIndex(
                name: "IX_BXJGUtilsAttachments_FileId",
                table: "BXJGUtilsAttachments");

            migrationBuilder.DropColumn(
                name: "FileId",
                table: "BXJGUtilsAttachments");

            migrationBuilder.AlterColumn<string>(
                name: "Parameters",
                table: "AbpAuditLogs",
                type: "nvarchar(2048)",
                maxLength: 2048,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldMaxLength: 4096,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BXJGUtilsAttachments_BXJGUtilsFiles_Id",
                table: "BXJGUtilsAttachments",
                column: "Id",
                principalTable: "BXJGUtilsFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
