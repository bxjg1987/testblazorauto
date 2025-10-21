using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZLJ.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class update1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PermissionNames",
                table: "BXJGUtilsFiles",
                type: "varchar(1000)",
                unicode: false,
                maxLength: 1000,
                nullable: true,
                comment: "当Permission为PermissionNames时，此字段存储哪些权限可以访问此文件，多个权限用英文逗号分割");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PermissionNames",
                table: "BXJGUtilsFiles");
        }
    }
}
