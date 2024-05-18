using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZLJ.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class testcgsimple : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ZLJ_TestSimple",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "普通数据测试"),
                    Age = table.Column<int>(type: "int", nullable: true, comment: "普通数据测试"),
                    Birthday = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "普通数据测试"),
                    StringField1 = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true, comment: "普通数据测试"),
                    Status = table.Column<int>(type: "int", nullable: false, comment: "普通数据测试"),
                    F2 = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true, comment: "普通数据测试"),
                    F3 = table.Column<bool>(type: "bit", nullable: false, comment: "普通数据测试"),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZLJ_TestSimple", x => x.Id);
                },
                comment: "普通数据测试");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ZLJ_TestSimple");
        }
    }
}
