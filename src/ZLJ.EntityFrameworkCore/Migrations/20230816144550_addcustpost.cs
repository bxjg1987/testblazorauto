using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZLJ.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class addcustpost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterTable(
                name: "AbpRoles",
                comment: "客户那边用户的角色（也是岗位）");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterTable(
                name: "AbpRoles",
                oldComment: "客户那边用户的角色（也是岗位）");
        }
    }
}
