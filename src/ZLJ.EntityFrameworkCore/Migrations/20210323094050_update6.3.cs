using Microsoft.EntityFrameworkCore.Migrations;

namespace ZLJ.Migrations
{
    public partial class update63 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BXJGGeneralTrees_BXJGWorkOrderCategory_CategoryEntityId",
                table: "BXJGGeneralTrees");

            migrationBuilder.DropForeignKey(
                name: "FK_BXJGWorkOrderCategory_BXJGGeneralTrees_ParentId",
                table: "BXJGWorkOrderCategory");

            migrationBuilder.DropIndex(
                name: "IX_BXJGGeneralTrees_CategoryEntityId",
                table: "BXJGGeneralTrees");

            migrationBuilder.DropColumn(
                name: "CategoryEntityId",
                table: "BXJGGeneralTrees");

            migrationBuilder.AlterColumn<int>(
                name: "TenantId",
                table: "BXJGWorkOrderCategory",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "PropertyName",
                table: "AbpDynamicProperties",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "EntityFullName",
                table: "AbpDynamicEntityProperties",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExceptionMessage",
                table: "AbpAuditLogs",
                type: "nvarchar(1024)",
                maxLength: 1024,
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BXJGWorkOrderCategory_BXJGWorkOrderCategory_ParentId",
                table: "BXJGWorkOrderCategory",
                column: "ParentId",
                principalTable: "BXJGWorkOrderCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BXJGWorkOrderCategory_BXJGWorkOrderCategory_ParentId",
                table: "BXJGWorkOrderCategory");

            migrationBuilder.DropColumn(
                name: "ExceptionMessage",
                table: "AbpAuditLogs");

            migrationBuilder.AlterColumn<int>(
                name: "TenantId",
                table: "BXJGWorkOrderCategory",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CategoryEntityId",
                table: "BXJGGeneralTrees",
                type: "bigint",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PropertyName",
                table: "AbpDynamicProperties",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "EntityFullName",
                table: "AbpDynamicEntityProperties",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BXJGGeneralTrees_CategoryEntityId",
                table: "BXJGGeneralTrees",
                column: "CategoryEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_BXJGGeneralTrees_BXJGWorkOrderCategory_CategoryEntityId",
                table: "BXJGGeneralTrees",
                column: "CategoryEntityId",
                principalTable: "BXJGWorkOrderCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BXJGWorkOrderCategory_BXJGGeneralTrees_ParentId",
                table: "BXJGWorkOrderCategory",
                column: "ParentId",
                principalTable: "BXJGGeneralTrees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
