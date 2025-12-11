using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZLJ.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class update4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "psi_Warehouse");

            migrationBuilder.DropTable(
                name: "ShebeiXinxi");

            migrationBuilder.DropTable(
                name: "psi_ProductCategory");

            migrationBuilder.DropColumn(
                name: "AreaName",
                table: "KehuXinxi");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "KehuXinxi");

            migrationBuilder.DropColumn(
                name: "ExtensionData",
                table: "KehuXinxi");

            migrationBuilder.DropColumn(
                name: "LevelName",
                table: "KehuXinxi");

            migrationBuilder.DropColumn(
                name: "ManagerId",
                table: "KehuXinxi");

            migrationBuilder.DropColumn(
                name: "ManagerName",
                table: "KehuXinxi");

            migrationBuilder.AlterTable(
                name: "KehuXinxi",
                oldComment: "往来单位实体");

            migrationBuilder.AlterColumn<int>(
                name: "TenantId",
                table: "ZLJ_TestTree",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "ZLJ_TestTree",
                type: "varchar(95)",
                unicode: false,
                maxLength: 95,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(95)",
                oldMaxLength: 95);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ZLJ_TestTree",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Remark",
                table: "ZLJ_TestTree",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TenantId",
                table: "KehuXinxi",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "租户id");

            migrationBuilder.AlterColumn<string>(
                name: "TaxNo",
                table: "KehuXinxi",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(128)",
                oldUnicode: false,
                oldMaxLength: 128,
                oldNullable: true,
                oldComment: "税号");

            migrationBuilder.AlterColumn<string>(
                name: "Pinyin",
                table: "KehuXinxi",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(256)",
                oldUnicode: false,
                oldMaxLength: 256,
                oldNullable: true,
                oldComment: "拼音简码");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "KehuXinxi",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256,
                oldComment: "公司名称");

            migrationBuilder.AlterColumn<decimal>(
                name: "Lng",
                table: "KehuXinxi",
                type: "decimal(18,6)",
                precision: 18,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,15)",
                oldPrecision: 18,
                oldScale: 15,
                oldNullable: true,
                oldComment: "经度");

            migrationBuilder.AlterColumn<string>(
                name: "LinkPhone",
                table: "KehuXinxi",
                type: "nvarchar(16)",
                maxLength: 16,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldUnicode: false,
                oldMaxLength: 200,
                oldNullable: true,
                oldComment: "联系电话");

            migrationBuilder.AlterColumn<string>(
                name: "LinkManPinyin",
                table: "KehuXinxi",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldUnicode: false,
                oldMaxLength: 100,
                oldNullable: true,
                oldComment: "联系人拼音");

            migrationBuilder.AlterColumn<string>(
                name: "LinkMan",
                table: "KehuXinxi",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true,
                oldComment: "联系人");

            migrationBuilder.AlterColumn<long>(
                name: "LevelId",
                table: "KehuXinxi",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true,
                oldComment: "客户等级Id");

            migrationBuilder.AlterColumn<decimal>(
                name: "Lat",
                table: "KehuXinxi",
                type: "decimal(18,6)",
                precision: 18,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,15)",
                oldPrecision: 18,
                oldScale: 15,
                oldNullable: true,
                oldComment: "纬度");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "KehuXinxi",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true,
                oldComment: "是否启用");

            migrationBuilder.AlterColumn<long>(
                name: "AreaId",
                table: "KehuXinxi",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true,
                oldComment: "所属区域");

            migrationBuilder.AlterColumn<string>(
                name: "AddressPinyin",
                table: "KehuXinxi",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(500)",
                oldUnicode: false,
                oldMaxLength: 500,
                oldNullable: true,
                oldComment: "地址拼音");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "KehuXinxi",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true,
                oldComment: "详细地址");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "KehuXinxi",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "唯一id，主键")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "TenantId",
                table: "BXJGUtilsDataDictionaries",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "BXJGUtilsDataDictionaries",
                type: "varchar(100)",
                unicode: false,
                maxLength: 100,
                nullable: true,
                comment: "如：pinpai  表示品牌节点，不同租户下此字段值一样。使用场景：在数据字典功能中，前端下拉框绑定时可以通过此字段绑定指定节点类型",
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldUnicode: false,
                oldMaxLength: 100,
                oldNullable: true,
                oldComment: "如：pingpai  表示品牌节点，不同租户下此字段值一样。使用场景：在数据字典功能中，前端下拉框绑定时可以通过此字段绑定指定节点类型");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "BXJGUtilsDataDictionaries",
                type: "varchar(95)",
                unicode: false,
                maxLength: 95,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(95)",
                oldMaxLength: 95);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "BXJGUtilsDataDictionaries",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Remark",
                table: "BXJGUtilsDataDictionaries",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TenantId",
                table: "baseinfo_administrative",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "baseinfo_administrative",
                type: "varchar(100)",
                unicode: false,
                maxLength: 100,
                nullable: true,
                comment: "如：pinpai  表示品牌节点，不同租户下此字段值一样。使用场景：在数据字典功能中，前端下拉框绑定时可以通过此字段绑定指定节点类型",
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldUnicode: false,
                oldMaxLength: 100,
                oldNullable: true,
                oldComment: "如：pingpai  表示品牌节点，不同租户下此字段值一样。使用场景：在数据字典功能中，前端下拉框绑定时可以通过此字段绑定指定节点类型");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "baseinfo_administrative",
                type: "varchar(95)",
                unicode: false,
                maxLength: 95,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(95)",
                oldMaxLength: 95);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "baseinfo_administrative",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Remark",
                table: "baseinfo_administrative",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BXJGUtilsMetadata",
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
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Code = table.Column<string>(type: "varchar(95)", unicode: false, maxLength: 95, nullable: false),
                    ChildrenCount = table.Column<int>(type: "int", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ExtensionData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true, comment: "如：pinpai  表示品牌节点，不同租户下此字段值一样。使用场景：在数据字典功能中，前端下拉框绑定时可以通过此字段绑定指定节点类型"),
                    Remark = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BXJGUtilsMetadata", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BXJGUtilsMetadata_BXJGUtilsMetadata_ParentId",
                        column: x => x.ParentId,
                        principalTable: "BXJGUtilsMetadata",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TestTreeEntity_TenantId",
                table: "ZLJ_TestTree",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_AssociatedCompanyEntity_TenantId",
                table: "KehuXinxi",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_FileEntity_TenantId",
                table: "BXJGUtilsFiles",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_FeedbackEntity_TenantId",
                table: "BXJGUtilsFeedbacks",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_DataDictionaryEntity_TenantId",
                table: "BXJGUtilsDataDictionaries",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_AttachmentEntity_TenantId",
                table: "BXJGUtilsAttachments",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_TagEntity_TenantId",
                table: "BXJGTag",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_AdministrativeEntity_TenantId",
                table: "baseinfo_administrative",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_WebhookSendAttempt_TenantId",
                table: "AbpWebhookSendAttempts",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_WebhookEvent_TenantId",
                table: "AbpWebhookEvents",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_UserToken_TenantId",
                table: "AbpUserTokens",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_User_TenantId",
                table: "AbpUsers",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_TenantId",
                table: "AbpUserRoles",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_UserOrganizationUnit_TenantId",
                table: "AbpUserOrganizationUnits",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_UserNotificationInfo_TenantId",
                table: "AbpUserNotifications",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogin_TenantId",
                table: "AbpUserLogins",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLoginAttempt_TenantId",
                table: "AbpUserLoginAttempts",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaim_TenantId",
                table: "AbpUserClaims",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Setting_TenantId",
                table: "AbpSettings",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Role_TenantId",
                table: "AbpRoles",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaim_TenantId",
                table: "AbpRoleClaims",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionSetting_TenantId",
                table: "AbpPermissions",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationUnit_TenantId",
                table: "AbpOrganizationUnits",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationUnitRole_TenantId",
                table: "AbpOrganizationUnitRoles",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationSubscriptionInfo_TenantId",
                table: "AbpNotificationSubscriptions",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationLanguageText_TenantId",
                table: "AbpLanguageTexts",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationLanguage_TenantId",
                table: "AbpLanguages",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_FeatureSetting_TenantId",
                table: "AbpFeatures",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityPropertyChange_TenantId",
                table: "AbpEntityPropertyChanges",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityChangeSet_TenantId",
                table: "AbpEntityChangeSets",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityChange_TenantId",
                table: "AbpEntityChanges",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_DynamicPropertyValue_TenantId",
                table: "AbpDynamicPropertyValues",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_DynamicProperty_TenantId",
                table: "AbpDynamicProperties",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_DynamicEntityPropertyValue_TenantId",
                table: "AbpDynamicEntityPropertyValues",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_DynamicEntityProperty_TenantId",
                table: "AbpDynamicEntityProperties",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLog_TenantId",
                table: "AbpAuditLogs",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_BXJGUtilsMetadata_ParentId",
                table: "BXJGUtilsMetadata",
                column: "ParentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BXJGUtilsMetadata");

            migrationBuilder.DropIndex(
                name: "IX_TestTreeEntity_TenantId",
                table: "ZLJ_TestTree");

            migrationBuilder.DropIndex(
                name: "IX_AssociatedCompanyEntity_TenantId",
                table: "KehuXinxi");

            migrationBuilder.DropIndex(
                name: "IX_FileEntity_TenantId",
                table: "BXJGUtilsFiles");

            migrationBuilder.DropIndex(
                name: "IX_FeedbackEntity_TenantId",
                table: "BXJGUtilsFeedbacks");

            migrationBuilder.DropIndex(
                name: "IX_DataDictionaryEntity_TenantId",
                table: "BXJGUtilsDataDictionaries");

            migrationBuilder.DropIndex(
                name: "IX_AttachmentEntity_TenantId",
                table: "BXJGUtilsAttachments");

            migrationBuilder.DropIndex(
                name: "IX_TagEntity_TenantId",
                table: "BXJGTag");

            migrationBuilder.DropIndex(
                name: "IX_AdministrativeEntity_TenantId",
                table: "baseinfo_administrative");

            migrationBuilder.DropIndex(
                name: "IX_WebhookSendAttempt_TenantId",
                table: "AbpWebhookSendAttempts");

            migrationBuilder.DropIndex(
                name: "IX_WebhookEvent_TenantId",
                table: "AbpWebhookEvents");

            migrationBuilder.DropIndex(
                name: "IX_UserToken_TenantId",
                table: "AbpUserTokens");

            migrationBuilder.DropIndex(
                name: "IX_User_TenantId",
                table: "AbpUsers");

            migrationBuilder.DropIndex(
                name: "IX_UserRole_TenantId",
                table: "AbpUserRoles");

            migrationBuilder.DropIndex(
                name: "IX_UserOrganizationUnit_TenantId",
                table: "AbpUserOrganizationUnits");

            migrationBuilder.DropIndex(
                name: "IX_UserNotificationInfo_TenantId",
                table: "AbpUserNotifications");

            migrationBuilder.DropIndex(
                name: "IX_UserLogin_TenantId",
                table: "AbpUserLogins");

            migrationBuilder.DropIndex(
                name: "IX_UserLoginAttempt_TenantId",
                table: "AbpUserLoginAttempts");

            migrationBuilder.DropIndex(
                name: "IX_UserClaim_TenantId",
                table: "AbpUserClaims");

            migrationBuilder.DropIndex(
                name: "IX_Setting_TenantId",
                table: "AbpSettings");

            migrationBuilder.DropIndex(
                name: "IX_Role_TenantId",
                table: "AbpRoles");

            migrationBuilder.DropIndex(
                name: "IX_RoleClaim_TenantId",
                table: "AbpRoleClaims");

            migrationBuilder.DropIndex(
                name: "IX_PermissionSetting_TenantId",
                table: "AbpPermissions");

            migrationBuilder.DropIndex(
                name: "IX_OrganizationUnit_TenantId",
                table: "AbpOrganizationUnits");

            migrationBuilder.DropIndex(
                name: "IX_OrganizationUnitRole_TenantId",
                table: "AbpOrganizationUnitRoles");

            migrationBuilder.DropIndex(
                name: "IX_NotificationSubscriptionInfo_TenantId",
                table: "AbpNotificationSubscriptions");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationLanguageText_TenantId",
                table: "AbpLanguageTexts");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationLanguage_TenantId",
                table: "AbpLanguages");

            migrationBuilder.DropIndex(
                name: "IX_FeatureSetting_TenantId",
                table: "AbpFeatures");

            migrationBuilder.DropIndex(
                name: "IX_EntityPropertyChange_TenantId",
                table: "AbpEntityPropertyChanges");

            migrationBuilder.DropIndex(
                name: "IX_EntityChangeSet_TenantId",
                table: "AbpEntityChangeSets");

            migrationBuilder.DropIndex(
                name: "IX_EntityChange_TenantId",
                table: "AbpEntityChanges");

            migrationBuilder.DropIndex(
                name: "IX_DynamicPropertyValue_TenantId",
                table: "AbpDynamicPropertyValues");

            migrationBuilder.DropIndex(
                name: "IX_DynamicProperty_TenantId",
                table: "AbpDynamicProperties");

            migrationBuilder.DropIndex(
                name: "IX_DynamicEntityPropertyValue_TenantId",
                table: "AbpDynamicEntityPropertyValues");

            migrationBuilder.DropIndex(
                name: "IX_DynamicEntityProperty_TenantId",
                table: "AbpDynamicEntityProperties");

            migrationBuilder.DropIndex(
                name: "IX_AuditLog_TenantId",
                table: "AbpAuditLogs");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ZLJ_TestTree");

            migrationBuilder.DropColumn(
                name: "Remark",
                table: "ZLJ_TestTree");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "BXJGUtilsDataDictionaries");

            migrationBuilder.DropColumn(
                name: "Remark",
                table: "BXJGUtilsDataDictionaries");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "baseinfo_administrative");

            migrationBuilder.DropColumn(
                name: "Remark",
                table: "baseinfo_administrative");

            migrationBuilder.AlterTable(
                name: "KehuXinxi",
                comment: "往来单位实体");

            migrationBuilder.AlterColumn<int>(
                name: "TenantId",
                table: "ZLJ_TestTree",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "ZLJ_TestTree",
                type: "nvarchar(95)",
                maxLength: 95,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(95)",
                oldUnicode: false,
                oldMaxLength: 95);

            migrationBuilder.AlterColumn<int>(
                name: "TenantId",
                table: "KehuXinxi",
                type: "int",
                nullable: false,
                comment: "租户id",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "TaxNo",
                table: "KehuXinxi",
                type: "varchar(128)",
                unicode: false,
                maxLength: 128,
                nullable: true,
                comment: "税号",
                oldClrType: typeof(string),
                oldType: "nvarchar(32)",
                oldMaxLength: 32,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Pinyin",
                table: "KehuXinxi",
                type: "varchar(256)",
                unicode: false,
                maxLength: 256,
                nullable: true,
                comment: "拼音简码",
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "KehuXinxi",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                comment: "公司名称",
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<decimal>(
                name: "Lng",
                table: "KehuXinxi",
                type: "decimal(18,15)",
                precision: 18,
                scale: 15,
                nullable: true,
                comment: "经度",
                oldClrType: typeof(decimal),
                oldType: "decimal(18,6)",
                oldPrecision: 18,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LinkPhone",
                table: "KehuXinxi",
                type: "varchar(200)",
                unicode: false,
                maxLength: 200,
                nullable: true,
                comment: "联系电话",
                oldClrType: typeof(string),
                oldType: "nvarchar(16)",
                oldMaxLength: 16,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LinkManPinyin",
                table: "KehuXinxi",
                type: "varchar(100)",
                unicode: false,
                maxLength: 100,
                nullable: true,
                comment: "联系人拼音",
                oldClrType: typeof(string),
                oldType: "nvarchar(32)",
                oldMaxLength: 32,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LinkMan",
                table: "KehuXinxi",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                comment: "联系人",
                oldClrType: typeof(string),
                oldType: "nvarchar(32)",
                oldMaxLength: 32,
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "LevelId",
                table: "KehuXinxi",
                type: "bigint",
                nullable: true,
                comment: "客户等级Id",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Lat",
                table: "KehuXinxi",
                type: "decimal(18,15)",
                precision: 18,
                scale: 15,
                nullable: true,
                comment: "纬度",
                oldClrType: typeof(decimal),
                oldType: "decimal(18,6)",
                oldPrecision: 18,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "KehuXinxi",
                type: "bit",
                nullable: false,
                defaultValue: true,
                comment: "是否启用",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<long>(
                name: "AreaId",
                table: "KehuXinxi",
                type: "bigint",
                nullable: true,
                comment: "所属区域",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AddressPinyin",
                table: "KehuXinxi",
                type: "varchar(500)",
                unicode: false,
                maxLength: 500,
                nullable: true,
                comment: "地址拼音",
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "KehuXinxi",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                comment: "详细地址",
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "KehuXinxi",
                type: "bigint",
                nullable: false,
                comment: "唯一id，主键",
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "AreaName",
                table: "KehuXinxi",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                comment: "所属区域名称");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "KehuXinxi",
                type: "nvarchar(34)",
                maxLength: 34,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ExtensionData",
                table: "KehuXinxi",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: true,
                comment: "扩展字段");

            migrationBuilder.AddColumn<string>(
                name: "LevelName",
                table: "KehuXinxi",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: true,
                comment: "客户等级名称");

            migrationBuilder.AddColumn<long>(
                name: "ManagerId",
                table: "KehuXinxi",
                type: "bigint",
                nullable: true,
                comment: "负责人id");

            migrationBuilder.AddColumn<string>(
                name: "ManagerName",
                table: "KehuXinxi",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                comment: "负责人姓名");

            migrationBuilder.AlterColumn<int>(
                name: "TenantId",
                table: "BXJGUtilsDataDictionaries",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "BXJGUtilsDataDictionaries",
                type: "varchar(100)",
                unicode: false,
                maxLength: 100,
                nullable: true,
                comment: "如：pingpai  表示品牌节点，不同租户下此字段值一样。使用场景：在数据字典功能中，前端下拉框绑定时可以通过此字段绑定指定节点类型",
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldUnicode: false,
                oldMaxLength: 100,
                oldNullable: true,
                oldComment: "如：pinpai  表示品牌节点，不同租户下此字段值一样。使用场景：在数据字典功能中，前端下拉框绑定时可以通过此字段绑定指定节点类型");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "BXJGUtilsDataDictionaries",
                type: "nvarchar(95)",
                maxLength: 95,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(95)",
                oldUnicode: false,
                oldMaxLength: 95);

            migrationBuilder.AlterColumn<int>(
                name: "TenantId",
                table: "baseinfo_administrative",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "baseinfo_administrative",
                type: "varchar(100)",
                unicode: false,
                maxLength: 100,
                nullable: true,
                comment: "如：pingpai  表示品牌节点，不同租户下此字段值一样。使用场景：在数据字典功能中，前端下拉框绑定时可以通过此字段绑定指定节点类型",
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldUnicode: false,
                oldMaxLength: 100,
                oldNullable: true,
                oldComment: "如：pinpai  表示品牌节点，不同租户下此字段值一样。使用场景：在数据字典功能中，前端下拉框绑定时可以通过此字段绑定指定节点类型");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "baseinfo_administrative",
                type: "nvarchar(95)",
                maxLength: 95,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(95)",
                oldUnicode: false,
                oldMaxLength: 95);

            migrationBuilder.CreateTable(
                name: "psi_ProductCategory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentId = table.Column<long>(type: "bigint", nullable: true),
                    ChildrenCount = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(95)", maxLength: 95, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DisplayName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ExtensionData = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true, comment: "扩展字段"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true, comment: "如：pingpai  表示品牌节点，不同租户下此字段值一样。使用场景：在数据字典功能中，前端下拉框绑定时可以通过此字段绑定指定节点类型"),
                    TenantId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_psi_ProductCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_psi_ProductCategory_psi_ProductCategory_ParentId",
                        column: x => x.ParentId,
                        principalTable: "psi_ProductCategory",
                        principalColumn: "Id");
                },
                comment: "产品分类实体");

            migrationBuilder.CreateTable(
                name: "psi_Warehouse",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "唯一id，主键"),
                    OrganizationUnitId = table.Column<long>(type: "bigint", nullable: true, comment: "所属组织机构id"),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "仓库地址"),
                    AddressPinyin = table.Column<string>(type: "varchar(95)", unicode: false, maxLength: 95, nullable: true, comment: "地址拼音简码"),
                    AreaId = table.Column<long>(type: "bigint", nullable: true, comment: "所属省市区县id"),
                    AreaName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true, comment: "省市区县名称"),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExtensionData = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true, comment: "扩展字段"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true, comment: "是否启用"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsPersonal = table.Column<bool>(type: "bit", nullable: false, comment: "是否是个人仓库"),
                    IsVirtual = table.Column<bool>(type: "bit", nullable: false, comment: "是否是虚拟仓库"),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    Latitude = table.Column<decimal>(type: "decimal(18,15)", precision: 18, scale: 15, nullable: true, comment: "纬度，用于地理位置定位"),
                    Longitude = table.Column<decimal>(type: "decimal(18,15)", precision: 18, scale: 15, nullable: true, comment: "经度，用于地理位置定位"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false, comment: "仓库名称"),
                    Phone = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true, comment: "联系电话"),
                    Pinyin = table.Column<string>(type: "varchar(95)", unicode: false, maxLength: 95, nullable: true, comment: "拼音简码"),
                    Remark = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true, comment: "备注"),
                    SquareMeasure = table.Column<int>(type: "int", nullable: false, comment: "面积，㎡"),
                    TenantId = table.Column<int>(type: "int", nullable: false, comment: "租户id"),
                    UserId = table.Column<long>(type: "bigint", nullable: true, comment: "负责人id"),
                    UserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "负责人姓名"),
                    Volume = table.Column<int>(type: "int", nullable: false, comment: "体积 m³"),
                    WarehouseType = table.Column<long>(type: "bigint", nullable: false, comment: "仓库类型")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_psi_Warehouse", x => x.Id);
                    table.ForeignKey(
                        name: "FK_psi_Warehouse_AbpOrganizationUnits_OrganizationUnitId",
                        column: x => x.OrganizationUnitId,
                        principalTable: "AbpOrganizationUnits",
                        principalColumn: "Id");
                },
                comment: "仓库档案实体");

            migrationBuilder.CreateTable(
                name: "ShebeiXinxi",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(64)", unicode: false, maxLength: 64, nullable: false, comment: "唯一id，主键"),
                    CategoryId = table.Column<long>(type: "bigint", nullable: false, comment: "商品类别id"),
                    OrganizationUnitId = table.Column<long>(type: "bigint", nullable: true, comment: "所属组织机构id"),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExtensionData = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true, comment: "扩展字段"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, comment: "是否启用"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsVirtual = table.Column<bool>(type: "bit", nullable: false, comment: "是否是虚拟产品"),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false, comment: "商品名称"),
                    Pinyin = table.Column<string>(type: "varchar(256)", unicode: false, maxLength: 256, nullable: true, comment: "商品名称拼音简码"),
                    Remark = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true, comment: "备注"),
                    SpecModel = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true, comment: "商品规格型号"),
                    TenantId = table.Column<int>(type: "int", nullable: false, comment: "租户id"),
                    Unit = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false, comment: "计量单位")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShebeiXinxi", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShebeiXinxi_AbpOrganizationUnits_OrganizationUnitId",
                        column: x => x.OrganizationUnitId,
                        principalTable: "AbpOrganizationUnits",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ShebeiXinxi_psi_ProductCategory_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "psi_ProductCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "商品档案实体");

            migrationBuilder.CreateIndex(
                name: "IX_psi_ProductCategory_Code",
                table: "psi_ProductCategory",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_psi_ProductCategory_ParentId",
                table: "psi_ProductCategory",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_psi_Warehouse_OrganizationUnitId",
                table: "psi_Warehouse",
                column: "OrganizationUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_ShebeiXinxi_CategoryId",
                table: "ShebeiXinxi",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ShebeiXinxi_OrganizationUnitId",
                table: "ShebeiXinxi",
                column: "OrganizationUnitId");
        }
    }
}
