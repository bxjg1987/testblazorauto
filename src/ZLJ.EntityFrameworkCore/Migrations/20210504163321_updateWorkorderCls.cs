using Microsoft.EntityFrameworkCore.Migrations;

namespace ZLJ.Migrations
{
    public partial class updateWorkorderCls : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DynamicAssociateData",
                table: "BXJGWorkOrder");

            migrationBuilder.AddColumn<bool>(
                name: "IsDefault",
                table: "BXJGWorkOrderCategory",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "WorkOrderType",
                table: "BXJGWorkOrderCategory",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDefault",
                table: "BXJGWorkOrderCategory");

            migrationBuilder.DropColumn(
                name: "WorkOrderType",
                table: "BXJGWorkOrderCategory");

            migrationBuilder.AddColumn<string>(
                name: "DynamicAssociateData",
                table: "BXJGWorkOrder",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
