using Microsoft.EntityFrameworkCore.Migrations;

namespace ZLJ.Migrations
{
    public partial class updateworkordercls3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkOrderCategoryTypeEntity_BXJGWorkOrderCategory_CategoryEntityId",
                table: "WorkOrderCategoryTypeEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkOrderCategoryTypeEntity",
                table: "WorkOrderCategoryTypeEntity");

            migrationBuilder.RenameTable(
                name: "WorkOrderCategoryTypeEntity",
                newName: "BXJGWorkOrderCategoryType");

            migrationBuilder.RenameColumn(
                name: "CategoryEntityId",
                table: "BXJGWorkOrderCategoryType",
                newName: "CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkOrderCategoryTypeEntity_CategoryEntityId",
                table: "BXJGWorkOrderCategoryType",
                newName: "IX_BXJGWorkOrderCategoryType_CategoryId");

            migrationBuilder.AlterColumn<string>(
                name: "WorkOrderType",
                table: "BXJGWorkOrderCategoryType",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_BXJGWorkOrderCategoryType",
                table: "BXJGWorkOrderCategoryType",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BXJGWorkOrderCategoryType_BXJGWorkOrderCategory_CategoryId",
                table: "BXJGWorkOrderCategoryType",
                column: "CategoryId",
                principalTable: "BXJGWorkOrderCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BXJGWorkOrderCategoryType_BXJGWorkOrderCategory_CategoryId",
                table: "BXJGWorkOrderCategoryType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BXJGWorkOrderCategoryType",
                table: "BXJGWorkOrderCategoryType");

            migrationBuilder.RenameTable(
                name: "BXJGWorkOrderCategoryType",
                newName: "WorkOrderCategoryTypeEntity");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "WorkOrderCategoryTypeEntity",
                newName: "CategoryEntityId");

            migrationBuilder.RenameIndex(
                name: "IX_BXJGWorkOrderCategoryType_CategoryId",
                table: "WorkOrderCategoryTypeEntity",
                newName: "IX_WorkOrderCategoryTypeEntity_CategoryEntityId");

            migrationBuilder.AlterColumn<string>(
                name: "WorkOrderType",
                table: "WorkOrderCategoryTypeEntity",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkOrderCategoryTypeEntity",
                table: "WorkOrderCategoryTypeEntity",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkOrderCategoryTypeEntity_BXJGWorkOrderCategory_CategoryEntityId",
                table: "WorkOrderCategoryTypeEntity",
                column: "CategoryEntityId",
                principalTable: "BXJGWorkOrderCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
