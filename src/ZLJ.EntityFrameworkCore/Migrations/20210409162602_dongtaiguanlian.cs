using Microsoft.EntityFrameworkCore.Migrations;

namespace ZLJ.Migrations
{
    public partial class dongtaiguanlian : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DynamicAssociateData",
                table: "BXJGWorkOrder",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DynamicAssociateData",
                table: "BXJGWorkOrder");
        }
    }
}
