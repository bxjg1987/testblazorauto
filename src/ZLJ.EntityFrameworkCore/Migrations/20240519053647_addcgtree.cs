using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZLJ.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class addcgtree : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ZLJ_TestTree",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "测试树"),
                    Age = table.Column<int>(type: "int", nullable: true, comment: "测试树"),
                    Birthday = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "测试树"),
                    StringField1 = table.Column<string>(type: "varchar(60)", unicode: false, maxLength: 60, nullable: false, comment: "测试树"),
                    Status = table.Column<int>(type: "int", nullable: false, comment: "测试树"),
                    F2 = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true, comment: "测试树"),
                    F3 = table.Column<bool>(type: "bit", nullable: false, comment: "测试树"),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(95)", maxLength: 95, nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    ChildrenCount = table.Column<int>(type: "int", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ExtensionData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZLJ_TestTree", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ZLJ_TestTree_ZLJ_TestTree_ParentId",
                        column: x => x.ParentId,
                        principalTable: "ZLJ_TestTree",
                        principalColumn: "Id");
                },
                comment: "测试树");

            migrationBuilder.CreateIndex(
                name: "IX_ZLJ_TestTree_Code",
                table: "ZLJ_TestTree",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ZLJ_TestTree_ParentId",
                table: "ZLJ_TestTree",
                column: "ParentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ZLJ_TestTree");
        }
    }
}
