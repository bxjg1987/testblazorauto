using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZLJ.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class addtreename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "BXJGUtilsDataDictionaries",
                type: "varchar(100)",
                unicode: false,
                maxLength: 100,
                nullable: true,
                comment: "如：pingpai  表示品牌节点，不同租户下此字段值一样。使用场景：在数据字典功能中，前端下拉框绑定时可以通过此字段绑定指定节点类型");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "baseinfo_administrative",
                type: "varchar(100)",
                unicode: false,
                maxLength: 100,
                nullable: true,
                comment: "如：pingpai  表示品牌节点，不同租户下此字段值一样。使用场景：在数据字典功能中，前端下拉框绑定时可以通过此字段绑定指定节点类型");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AbpOrganizationUnits",
                type: "varchar(100)",
                unicode: false,
                maxLength: 100,
                nullable: true,
                comment: "如：pingpai  表示品牌节点，不同租户下此字段值一样。使用场景：在数据字典功能中，前端下拉框绑定时可以通过此字段绑定指定节点类型");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "BXJGUtilsDataDictionaries");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "baseinfo_administrative");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "AbpOrganizationUnits");
        }
    }
}
