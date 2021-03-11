using Microsoft.EntityFrameworkCore.Migrations;

namespace ZLJ.Migrations
{
    public partial class addIndexForWorkOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_BXJGWorkOrder_CategoryId_EmployeeId",
                table: "BXJGWorkOrder",
                columns: new[] { "CategoryId", "EmployeeId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BXJGWorkOrder_CategoryId_EmployeeId",
                table: "BXJGWorkOrder");
        }
    }
}
