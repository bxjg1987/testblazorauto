using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZLJ.Migrations
{
    public partial class addWorkOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CategoryEntityId",
                table: "BXJGGeneralTrees",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BXJGWorkOrder",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CategoryId = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    UrgencyDegree = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    StatusChangedDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    StatusChangedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    EstimatedExecutionTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    EstimatedCompletionTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ExecutionTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CompletionTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    EmployeeId = table.Column<string>(type: "varchar(64)", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    ExtensionData = table.Column<string>(type: "nvarchar(max)", maxLength: 2147483647, nullable: true),
                    ExtendedField1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExtendedField2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExtendedField3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExtendedField4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExtendedField5 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BXJGWorkOrder", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BXJGWorkOrderCategory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    ExtensionData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(95)", maxLength: 95, nullable: false),
                    ParentId = table.Column<long>(type: "bigint", nullable: true),
                    DisplayName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BXJGWorkOrderCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BXJGWorkOrderCategory_BXJGGeneralTrees_ParentId",
                        column: x => x.ParentId,
                        principalTable: "BXJGGeneralTrees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BXJGGeneralTrees_CategoryEntityId",
                table: "BXJGGeneralTrees",
                column: "CategoryEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_BXJGWorkOrderCategory_ParentId",
                table: "BXJGWorkOrderCategory",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_BXJGGeneralTrees_BXJGWorkOrderCategory_CategoryEntityId",
                table: "BXJGGeneralTrees",
                column: "CategoryEntityId",
                principalTable: "BXJGWorkOrderCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BXJGGeneralTrees_BXJGWorkOrderCategory_CategoryEntityId",
                table: "BXJGGeneralTrees");

            migrationBuilder.DropTable(
                name: "BXJGWorkOrder");

            migrationBuilder.DropTable(
                name: "BXJGWorkOrderCategory");

            migrationBuilder.DropIndex(
                name: "IX_BXJGGeneralTrees_CategoryEntityId",
                table: "BXJGGeneralTrees");

            migrationBuilder.DropColumn(
                name: "CategoryEntityId",
                table: "BXJGGeneralTrees");
        }
    }
}
