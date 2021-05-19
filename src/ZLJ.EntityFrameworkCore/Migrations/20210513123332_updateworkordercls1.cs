using Microsoft.EntityFrameworkCore.Migrations;

namespace ZLJ.Migrations
{
    public partial class updateworkordercls1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WorkOrderType",
                table: "BXJGWorkOrderCategory");

            migrationBuilder.CreateTable(
                name: "WorkOrderCategoryTypeEntity",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    WorkOrderType = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    CategoryEntityId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkOrderCategoryTypeEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkOrderCategoryTypeEntity_BXJGWorkOrderCategory_CategoryEntityId",
                        column: x => x.CategoryEntityId,
                        principalTable: "BXJGWorkOrderCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrderCategoryTypeEntity_CategoryEntityId",
                table: "WorkOrderCategoryTypeEntity",
                column: "CategoryEntityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkOrderCategoryTypeEntity");

            migrationBuilder.AddColumn<string>(
                name: "WorkOrderType",
                table: "BXJGWorkOrderCategory",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }
    }
}
