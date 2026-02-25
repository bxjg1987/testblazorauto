using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZLJ.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class changebxjg : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BXJGUtilsAttachments_BXJGUtilsFiles_FileId",
                table: "BXJGUtilsAttachments");

            migrationBuilder.DropForeignKey(
                name: "FK_BXJGUtilsDataDictionaries_BXJGUtilsDataDictionaries_ParentId",
                table: "BXJGUtilsDataDictionaries");

            migrationBuilder.DropForeignKey(
                name: "FK_BXJGUtilsDataPermission_BXJGUtilsMetadata_MetaDataId",
                table: "BXJGUtilsDataPermission");

            migrationBuilder.DropForeignKey(
                name: "FK_BXJGUtilsMetadata_BXJGUtilsMetadata_ParentId",
                table: "BXJGUtilsMetadata");

            migrationBuilder.DropForeignKey(
                name: "FK_KehuXinxi_BXJGUtilsDataDictionaries_LevelId",
                table: "KehuXinxi");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BXJGUtilsMetadata",
                table: "BXJGUtilsMetadata");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BXJGUtilsFiles",
                table: "BXJGUtilsFiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BXJGUtilsFeedbacks",
                table: "BXJGUtilsFeedbacks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BXJGUtilsDataPermission",
                table: "BXJGUtilsDataPermission");

            migrationBuilder.DropIndex(
                name: "IX_BXJGUtilsDataPermission_GrantType",
                table: "BXJGUtilsDataPermission");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BXJGUtilsDataDictionaries",
                table: "BXJGUtilsDataDictionaries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BXJGUtilsAttachments",
                table: "BXJGUtilsAttachments");

            migrationBuilder.RenameTable(
                name: "BXJGUtilsMetadata",
                newName: "BXJGMetadata");

            migrationBuilder.RenameTable(
                name: "BXJGUtilsFiles",
                newName: "BXJGFiles");

            migrationBuilder.RenameTable(
                name: "BXJGUtilsFeedbacks",
                newName: "BXJGFeedbacks");

            migrationBuilder.RenameTable(
                name: "BXJGUtilsDataPermission",
                newName: "BXJGDataPermission");

            migrationBuilder.RenameTable(
                name: "BXJGUtilsDataDictionaries",
                newName: "BXJGDataDictionaries");

            migrationBuilder.RenameTable(
                name: "BXJGUtilsAttachments",
                newName: "BXJGAttachments");

            migrationBuilder.RenameIndex(
                name: "IX_BXJGUtilsMetadata_ParentId",
                table: "BXJGMetadata",
                newName: "IX_BXJGMetadata_ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_BXJGUtilsMetadata_Name",
                table: "BXJGMetadata",
                newName: "IX_BXJGMetadata_Name");

            migrationBuilder.RenameIndex(
                name: "IX_BXJGUtilsMetadata_Code",
                table: "BXJGMetadata",
                newName: "IX_BXJGMetadata_Code");

            migrationBuilder.RenameIndex(
                name: "IX_BXJGUtilsFiles_Size",
                table: "BXJGFiles",
                newName: "IX_BXJGFiles_Size");

            migrationBuilder.RenameIndex(
                name: "IX_BXJGUtilsFiles_ResponseContentType",
                table: "BXJGFiles",
                newName: "IX_BXJGFiles_ResponseContentType");

            migrationBuilder.RenameIndex(
                name: "IX_BXJGUtilsFiles_RealName",
                table: "BXJGFiles",
                newName: "IX_BXJGFiles_RealName");

            migrationBuilder.RenameIndex(
                name: "IX_BXJGUtilsFeedbacks_CreationTime",
                table: "BXJGFeedbacks",
                newName: "IX_BXJGFeedbacks_CreationTime");

            migrationBuilder.RenameIndex(
                name: "IX_BXJGUtilsDataPermission_UserOrganizationUnit",
                table: "BXJGDataPermission",
                newName: "IX_BXJGDataPermission_UserOrganizationUnit");

            migrationBuilder.RenameIndex(
                name: "IX_BXJGUtilsDataPermission_UserId",
                table: "BXJGDataPermission",
                newName: "IX_BXJGDataPermission_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_BXJGUtilsDataPermission_RoleId",
                table: "BXJGDataPermission",
                newName: "IX_BXJGDataPermission_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_BXJGUtilsDataPermission_MetaDataId",
                table: "BXJGDataPermission",
                newName: "IX_BXJGDataPermission_MetaDataId");

            migrationBuilder.RenameIndex(
                name: "IX_BXJGUtilsDataPermission_EntityTypeFullName",
                table: "BXJGDataPermission",
                newName: "IX_BXJGDataPermission_EntityTypeFullName");

            migrationBuilder.RenameIndex(
                name: "IX_BXJGUtilsDataPermission_DataOrganizationUnit",
                table: "BXJGDataPermission",
                newName: "IX_BXJGDataPermission_DataOrganizationUnit");

            migrationBuilder.RenameIndex(
                name: "IX_BXJGUtilsDataDictionaries_ParentId",
                table: "BXJGDataDictionaries",
                newName: "IX_BXJGDataDictionaries_ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_BXJGUtilsDataDictionaries_Name",
                table: "BXJGDataDictionaries",
                newName: "IX_BXJGDataDictionaries_Name");

            migrationBuilder.RenameIndex(
                name: "IX_BXJGUtilsDataDictionaries_Code",
                table: "BXJGDataDictionaries",
                newName: "IX_BXJGDataDictionaries_Code");

            migrationBuilder.RenameIndex(
                name: "IX_BXJGUtilsAttachments_PropertyName",
                table: "BXJGAttachments",
                newName: "IX_BXJGAttachments_PropertyName");

            migrationBuilder.RenameIndex(
                name: "IX_BXJGUtilsAttachments_OrderIndex",
                table: "BXJGAttachments",
                newName: "IX_BXJGAttachments_OrderIndex");

            migrationBuilder.RenameIndex(
                name: "IX_BXJGUtilsAttachments_FileId",
                table: "BXJGAttachments",
                newName: "IX_BXJGAttachments_FileId");

            migrationBuilder.RenameIndex(
                name: "IX_BXJGUtilsAttachments_EntityType",
                table: "BXJGAttachments",
                newName: "IX_BXJGAttachments_EntityType");

            migrationBuilder.RenameIndex(
                name: "IX_BXJGUtilsAttachments_EntityId",
                table: "BXJGAttachments",
                newName: "IX_BXJGAttachments_EntityId");

            migrationBuilder.AlterColumn<int>(
                name: "TenantId",
                table: "BXJGDataPermission",
                type: "int",
                nullable: true,
                comment: "租户id",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "DataOrganizationUnit",
                table: "BXJGDataPermission",
                type: "bigint",
                nullable: true,
                comment: "属于此单位的数据 EntityTypeFullName指定数据类型是必须的，在它基础上进一步限定指定单位的数据进行权限控制",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true,
                oldComment: "属于此单位的数据");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BXJGMetadata",
                table: "BXJGMetadata",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BXJGFiles",
                table: "BXJGFiles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BXJGFeedbacks",
                table: "BXJGFeedbacks",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BXJGDataPermission",
                table: "BXJGDataPermission",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BXJGDataDictionaries",
                table: "BXJGDataDictionaries",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BXJGAttachments",
                table: "BXJGAttachments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BXJGAttachments_BXJGFiles_FileId",
                table: "BXJGAttachments",
                column: "FileId",
                principalTable: "BXJGFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BXJGDataDictionaries_BXJGDataDictionaries_ParentId",
                table: "BXJGDataDictionaries",
                column: "ParentId",
                principalTable: "BXJGDataDictionaries",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BXJGDataPermission_BXJGMetadata_MetaDataId",
                table: "BXJGDataPermission",
                column: "MetaDataId",
                principalTable: "BXJGMetadata",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BXJGMetadata_BXJGMetadata_ParentId",
                table: "BXJGMetadata",
                column: "ParentId",
                principalTable: "BXJGMetadata",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_KehuXinxi_BXJGDataDictionaries_LevelId",
                table: "KehuXinxi",
                column: "LevelId",
                principalTable: "BXJGDataDictionaries",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BXJGAttachments_BXJGFiles_FileId",
                table: "BXJGAttachments");

            migrationBuilder.DropForeignKey(
                name: "FK_BXJGDataDictionaries_BXJGDataDictionaries_ParentId",
                table: "BXJGDataDictionaries");

            migrationBuilder.DropForeignKey(
                name: "FK_BXJGDataPermission_BXJGMetadata_MetaDataId",
                table: "BXJGDataPermission");

            migrationBuilder.DropForeignKey(
                name: "FK_BXJGMetadata_BXJGMetadata_ParentId",
                table: "BXJGMetadata");

            migrationBuilder.DropForeignKey(
                name: "FK_KehuXinxi_BXJGDataDictionaries_LevelId",
                table: "KehuXinxi");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BXJGMetadata",
                table: "BXJGMetadata");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BXJGFiles",
                table: "BXJGFiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BXJGFeedbacks",
                table: "BXJGFeedbacks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BXJGDataPermission",
                table: "BXJGDataPermission");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BXJGDataDictionaries",
                table: "BXJGDataDictionaries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BXJGAttachments",
                table: "BXJGAttachments");

            migrationBuilder.RenameTable(
                name: "BXJGMetadata",
                newName: "BXJGUtilsMetadata");

            migrationBuilder.RenameTable(
                name: "BXJGFiles",
                newName: "BXJGUtilsFiles");

            migrationBuilder.RenameTable(
                name: "BXJGFeedbacks",
                newName: "BXJGUtilsFeedbacks");

            migrationBuilder.RenameTable(
                name: "BXJGDataPermission",
                newName: "BXJGUtilsDataPermission");

            migrationBuilder.RenameTable(
                name: "BXJGDataDictionaries",
                newName: "BXJGUtilsDataDictionaries");

            migrationBuilder.RenameTable(
                name: "BXJGAttachments",
                newName: "BXJGUtilsAttachments");

            migrationBuilder.RenameIndex(
                name: "IX_BXJGMetadata_ParentId",
                table: "BXJGUtilsMetadata",
                newName: "IX_BXJGUtilsMetadata_ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_BXJGMetadata_Name",
                table: "BXJGUtilsMetadata",
                newName: "IX_BXJGUtilsMetadata_Name");

            migrationBuilder.RenameIndex(
                name: "IX_BXJGMetadata_Code",
                table: "BXJGUtilsMetadata",
                newName: "IX_BXJGUtilsMetadata_Code");

            migrationBuilder.RenameIndex(
                name: "IX_BXJGFiles_Size",
                table: "BXJGUtilsFiles",
                newName: "IX_BXJGUtilsFiles_Size");

            migrationBuilder.RenameIndex(
                name: "IX_BXJGFiles_ResponseContentType",
                table: "BXJGUtilsFiles",
                newName: "IX_BXJGUtilsFiles_ResponseContentType");

            migrationBuilder.RenameIndex(
                name: "IX_BXJGFiles_RealName",
                table: "BXJGUtilsFiles",
                newName: "IX_BXJGUtilsFiles_RealName");

            migrationBuilder.RenameIndex(
                name: "IX_BXJGFeedbacks_CreationTime",
                table: "BXJGUtilsFeedbacks",
                newName: "IX_BXJGUtilsFeedbacks_CreationTime");

            migrationBuilder.RenameIndex(
                name: "IX_BXJGDataPermission_UserOrganizationUnit",
                table: "BXJGUtilsDataPermission",
                newName: "IX_BXJGUtilsDataPermission_UserOrganizationUnit");

            migrationBuilder.RenameIndex(
                name: "IX_BXJGDataPermission_UserId",
                table: "BXJGUtilsDataPermission",
                newName: "IX_BXJGUtilsDataPermission_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_BXJGDataPermission_RoleId",
                table: "BXJGUtilsDataPermission",
                newName: "IX_BXJGUtilsDataPermission_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_BXJGDataPermission_MetaDataId",
                table: "BXJGUtilsDataPermission",
                newName: "IX_BXJGUtilsDataPermission_MetaDataId");

            migrationBuilder.RenameIndex(
                name: "IX_BXJGDataPermission_EntityTypeFullName",
                table: "BXJGUtilsDataPermission",
                newName: "IX_BXJGUtilsDataPermission_EntityTypeFullName");

            migrationBuilder.RenameIndex(
                name: "IX_BXJGDataPermission_DataOrganizationUnit",
                table: "BXJGUtilsDataPermission",
                newName: "IX_BXJGUtilsDataPermission_DataOrganizationUnit");

            migrationBuilder.RenameIndex(
                name: "IX_BXJGDataDictionaries_ParentId",
                table: "BXJGUtilsDataDictionaries",
                newName: "IX_BXJGUtilsDataDictionaries_ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_BXJGDataDictionaries_Name",
                table: "BXJGUtilsDataDictionaries",
                newName: "IX_BXJGUtilsDataDictionaries_Name");

            migrationBuilder.RenameIndex(
                name: "IX_BXJGDataDictionaries_Code",
                table: "BXJGUtilsDataDictionaries",
                newName: "IX_BXJGUtilsDataDictionaries_Code");

            migrationBuilder.RenameIndex(
                name: "IX_BXJGAttachments_PropertyName",
                table: "BXJGUtilsAttachments",
                newName: "IX_BXJGUtilsAttachments_PropertyName");

            migrationBuilder.RenameIndex(
                name: "IX_BXJGAttachments_OrderIndex",
                table: "BXJGUtilsAttachments",
                newName: "IX_BXJGUtilsAttachments_OrderIndex");

            migrationBuilder.RenameIndex(
                name: "IX_BXJGAttachments_FileId",
                table: "BXJGUtilsAttachments",
                newName: "IX_BXJGUtilsAttachments_FileId");

            migrationBuilder.RenameIndex(
                name: "IX_BXJGAttachments_EntityType",
                table: "BXJGUtilsAttachments",
                newName: "IX_BXJGUtilsAttachments_EntityType");

            migrationBuilder.RenameIndex(
                name: "IX_BXJGAttachments_EntityId",
                table: "BXJGUtilsAttachments",
                newName: "IX_BXJGUtilsAttachments_EntityId");

            migrationBuilder.AlterColumn<int>(
                name: "TenantId",
                table: "BXJGUtilsDataPermission",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldComment: "租户id");

            migrationBuilder.AlterColumn<long>(
                name: "DataOrganizationUnit",
                table: "BXJGUtilsDataPermission",
                type: "bigint",
                nullable: true,
                comment: "属于此单位的数据",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true,
                oldComment: "属于此单位的数据 EntityTypeFullName指定数据类型是必须的，在它基础上进一步限定指定单位的数据进行权限控制");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BXJGUtilsMetadata",
                table: "BXJGUtilsMetadata",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BXJGUtilsFiles",
                table: "BXJGUtilsFiles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BXJGUtilsFeedbacks",
                table: "BXJGUtilsFeedbacks",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BXJGUtilsDataPermission",
                table: "BXJGUtilsDataPermission",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BXJGUtilsDataDictionaries",
                table: "BXJGUtilsDataDictionaries",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BXJGUtilsAttachments",
                table: "BXJGUtilsAttachments",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_BXJGUtilsDataPermission_GrantType",
                table: "BXJGUtilsDataPermission",
                column: "GrantType");

            migrationBuilder.AddForeignKey(
                name: "FK_BXJGUtilsAttachments_BXJGUtilsFiles_FileId",
                table: "BXJGUtilsAttachments",
                column: "FileId",
                principalTable: "BXJGUtilsFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BXJGUtilsDataDictionaries_BXJGUtilsDataDictionaries_ParentId",
                table: "BXJGUtilsDataDictionaries",
                column: "ParentId",
                principalTable: "BXJGUtilsDataDictionaries",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BXJGUtilsDataPermission_BXJGUtilsMetadata_MetaDataId",
                table: "BXJGUtilsDataPermission",
                column: "MetaDataId",
                principalTable: "BXJGUtilsMetadata",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BXJGUtilsMetadata_BXJGUtilsMetadata_ParentId",
                table: "BXJGUtilsMetadata",
                column: "ParentId",
                principalTable: "BXJGUtilsMetadata",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_KehuXinxi_BXJGUtilsDataDictionaries_LevelId",
                table: "KehuXinxi",
                column: "LevelId",
                principalTable: "BXJGUtilsDataDictionaries",
                principalColumn: "Id");
        }
    }
}
