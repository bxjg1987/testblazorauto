using Microsoft.EntityFrameworkCore.Migrations;

namespace ZLJ.Migrations
{
    public partial class updateworkorder9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BXJGWorkOrderCategoryType_BXJGWorkOrderCategory_CategoryId",
                table: "BXJGWorkOrderCategoryType");

            migrationBuilder.AlterColumn<long>(
                name: "CategoryId",
                table: "BXJGWorkOrderCategoryType",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CategoryId1",
                table: "BXJGWorkOrderCategoryType",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BXJGWorkOrderCategoryType_CategoryId1",
                table: "BXJGWorkOrderCategoryType",
                column: "CategoryId1");

            migrationBuilder.AddForeignKey(
                name: "FK_BXJGWorkOrderCategoryType_BXJGWorkOrderCategory_CategoryId",
                table: "BXJGWorkOrderCategoryType",
                column: "CategoryId",
                principalTable: "BXJGWorkOrderCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BXJGWorkOrderCategoryType_BXJGWorkOrderCategory_CategoryId1",
                table: "BXJGWorkOrderCategoryType",
                column: "CategoryId1",
                principalTable: "BXJGWorkOrderCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BXJGWorkOrderCategoryType_BXJGWorkOrderCategory_CategoryId",
                table: "BXJGWorkOrderCategoryType");

            migrationBuilder.DropForeignKey(
                name: "FK_BXJGWorkOrderCategoryType_BXJGWorkOrderCategory_CategoryId1",
                table: "BXJGWorkOrderCategoryType");

            migrationBuilder.DropIndex(
                name: "IX_BXJGWorkOrderCategoryType_CategoryId1",
                table: "BXJGWorkOrderCategoryType");

            migrationBuilder.DropColumn(
                name: "CategoryId1",
                table: "BXJGWorkOrderCategoryType");

            migrationBuilder.AlterColumn<long>(
                name: "CategoryId",
                table: "BXJGWorkOrderCategoryType",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_BXJGWorkOrderCategoryType_BXJGWorkOrderCategory_CategoryId",
                table: "BXJGWorkOrderCategoryType",
                column: "CategoryId",
                principalTable: "BXJGWorkOrderCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
