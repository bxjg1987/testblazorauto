using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZLJ.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class adddatapermission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EntityTypeFullName",
                table: "BXJGUtilsMetadata",
                type: "varchar(100)",
                unicode: false,
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                comment: "指定的实体类型才会做数据权限控制");

            migrationBuilder.CreateTable(
                name: "BXJGUtilsDataPermission",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: true, comment: "对具体用户进行数据授权"),
                    RoleId = table.Column<int>(type: "int", nullable: true, comment: "仅对这个角色下的用户授权"),
                    UserOrganizationUnit = table.Column<long>(type: "bigint", nullable: true, comment: "仅对这个组织单位下的用户授权，但不包含后代单位"),
                    EntityTypeFullName = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false, comment: "指定的实体类型才会做数据权限控制"),
                    MetaDataId = table.Column<long>(type: "bigint", nullable: false, comment: "MetaData表元数据"),
                    DataOrganizationUnit = table.Column<long>(type: "bigint", nullable: true, comment: "属于此单位的数据"),
                    GrantType = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_BXJGUtilsDataPermission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BXJGUtilsDataPermission_BXJGUtilsMetadata_MetaDataId",
                        column: x => x.MetaDataId,
                        principalTable: "BXJGUtilsMetadata",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "数据权限");

            migrationBuilder.CreateIndex(
                name: "IX_BXJGUtilsMetadata_Code",
                table: "BXJGUtilsMetadata",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_BXJGUtilsMetadata_Name",
                table: "BXJGUtilsMetadata",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_BXJGUtilsDataPermission_DataOrganizationUnit",
                table: "BXJGUtilsDataPermission",
                column: "DataOrganizationUnit");

            migrationBuilder.CreateIndex(
                name: "IX_BXJGUtilsDataPermission_EntityTypeFullName",
                table: "BXJGUtilsDataPermission",
                column: "EntityTypeFullName");

            migrationBuilder.CreateIndex(
                name: "IX_BXJGUtilsDataPermission_GrantType",
                table: "BXJGUtilsDataPermission",
                column: "GrantType");

            migrationBuilder.CreateIndex(
                name: "IX_BXJGUtilsDataPermission_MetaDataId",
                table: "BXJGUtilsDataPermission",
                column: "MetaDataId");

            migrationBuilder.CreateIndex(
                name: "IX_BXJGUtilsDataPermission_RoleId",
                table: "BXJGUtilsDataPermission",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_BXJGUtilsDataPermission_UserId",
                table: "BXJGUtilsDataPermission",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BXJGUtilsDataPermission_UserOrganizationUnit",
                table: "BXJGUtilsDataPermission",
                column: "UserOrganizationUnit");

            migrationBuilder.CreateIndex(
                name: "IX_DataPermissionEntity_TenantId",
                table: "BXJGUtilsDataPermission",
                column: "TenantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BXJGUtilsDataPermission");

            migrationBuilder.DropIndex(
                name: "IX_BXJGUtilsMetadata_Code",
                table: "BXJGUtilsMetadata");

            migrationBuilder.DropIndex(
                name: "IX_BXJGUtilsMetadata_Name",
                table: "BXJGUtilsMetadata");

            migrationBuilder.DropColumn(
                name: "EntityTypeFullName",
                table: "BXJGUtilsMetadata");
        }
    }
}
