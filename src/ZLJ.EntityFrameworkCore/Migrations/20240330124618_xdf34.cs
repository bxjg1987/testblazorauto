using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZLJ.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class xdf34 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BXJGUtilsAttachments_BXJGUtilsFiles_FileId",
                table: "BXJGUtilsAttachments");

            migrationBuilder.DropTable(
                name: "BXJGUtilsAttchmentPermissions");

            migrationBuilder.DropIndex(
                name: "IX_BXJGUtilsAttachments_FileId",
                table: "BXJGUtilsAttachments");

            migrationBuilder.DropColumn(
                name: "ExtensionData",
                table: "BXJGUtilsAttachments");

            migrationBuilder.DropColumn(
                name: "FileId",
                table: "BXJGUtilsAttachments");

            migrationBuilder.AddForeignKey(
                name: "FK_BXJGUtilsAttachments_BXJGUtilsFiles_Id",
                table: "BXJGUtilsAttachments",
                column: "Id",
                principalTable: "BXJGUtilsFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BXJGUtilsAttachments_BXJGUtilsFiles_Id",
                table: "BXJGUtilsAttachments");

            migrationBuilder.AddColumn<string>(
                name: "ExtensionData",
                table: "BXJGUtilsAttachments",
                type: "varchar(4000)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "FileId",
                table: "BXJGUtilsAttachments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "BXJGUtilsAttchmentPermissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeletePermissionName = table.Column<string>(type: "varchar(100)", nullable: true, comment: "允许删除的权限名称"),
                    DownloadPermissionName = table.Column<string>(type: "varchar(100)", nullable: true, comment: "允许下载的权限名称"),
                    EntityId = table.Column<string>(type: "varchar(60)", nullable: true, comment: "关联实体id"),
                    EntityType = table.Column<string>(type: "varchar(100)", nullable: false, comment: "关联实体类型，可以是任意唯一字符串，通常是实体类型.FullTypeName")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BXJGUtilsAttchmentPermissions", x => x.Id);
                });

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
    }
}
