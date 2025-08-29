using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZLJ.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class addfeedback : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BXJGUtilsAttachments_EntityType_EntityId_PropertyName",
                table: "BXJGUtilsAttachments");

            migrationBuilder.AlterTable(
                name: "BXJGUtilsFiles",
                comment: "通用的文件表");

            migrationBuilder.AlterTable(
                name: "BXJGUtilsAttachments",
                comment: "通用的实体附件");

            migrationBuilder.AlterColumn<string>(
                name: "ExtensionData",
                table: "BXJGUtilsFiles",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: true,
                comment: "扩展数据",
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldNullable: true,
                oldComment: "扩展数据");

            migrationBuilder.AddColumn<long>(
                name: "Size",
                table: "BXJGUtilsFiles",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<string>(
                name: "PropertyName",
                table: "BXJGUtilsAttachments",
                type: "varchar(100)",
                unicode: false,
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                comment: "属性名，可空 比如工单：字段A表示要处理的问题相关图片，字段B表示处理完成时拍摄的图片，它们都使用附件表，当通过此字段来表示关联的不同的属性",
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldNullable: true,
                oldComment: "属性名，可空 比如工单：字段A表示要处理的问题相关图片，字段B表示处理完成时拍摄的图片，它们都使用附件表，当通过此字段来表示关联的不同的属性");

            migrationBuilder.AddColumn<string>(
                name: "PropertyDisplayName",
                table: "BXJGUtilsAttachments",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: true,
                comment: "属性显示名，在存储时若为空则复制PropertyName");

            migrationBuilder.CreateTable(
                name: "BXJGTag",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    EntityType = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false, comment: "关联实体类型，可以是任意唯一字符串，通常是实体类型.FullTypeName"),
                    EntityId = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false, comment: "关联实体id"),
                    PropertyName = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false, comment: "属性名，可空 比如工单：字段A表示要处理的问题相关tag，字段B表示处理完成时拍摄的tag，它们都使用tag表，当通过此字段来表示关联的不同的属性"),
                    PropertyDisplayName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true, comment: "属性显示名，在存储时若为空则复制PropertyName"),
                    TagName = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false, comment: "标签名称、同一个实体的同一个属性下必须唯一"),
                    TagDisplayName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "标签显示名称"),
                    OrderIndex = table.Column<int>(type: "int", nullable: false),
                    ExtensionData = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true, comment: "json格式的扩展数据"),
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
                    table.PrimaryKey("PK_BXJGTag", x => x.Id);
                },
                comment: "通用实体标签");

            migrationBuilder.CreateTable(
                name: "BXJGUtilsFeedbacks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    ConnectName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, comment: "称呼/姓名"),
                    ConnectInfo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "联系方式 如：手机号17723345454 或者 邮箱 17723345454@163.com "),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "标题"),
                    Content = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true, comment: "内容"),
                    ExtensionData = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true, comment: "扩展数据"),
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
                    table.PrimaryKey("PK_BXJGUtilsFeedbacks", x => x.Id);
                },
                comment: "通用的留言（将来可能关联tag、评论作为回复、图片等）");

            migrationBuilder.CreateIndex(
                name: "IX_BXJGUtilsFiles_RealName",
                table: "BXJGUtilsFiles",
                column: "RealName");

            migrationBuilder.CreateIndex(
                name: "IX_BXJGUtilsFiles_ResponseContentType",
                table: "BXJGUtilsFiles",
                column: "ResponseContentType");

            migrationBuilder.CreateIndex(
                name: "IX_BXJGUtilsFiles_Size",
                table: "BXJGUtilsFiles",
                column: "Size");

            migrationBuilder.CreateIndex(
                name: "IX_BXJGUtilsAttachments_EntityId",
                table: "BXJGUtilsAttachments",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_BXJGUtilsAttachments_EntityType",
                table: "BXJGUtilsAttachments",
                column: "EntityType");

            migrationBuilder.CreateIndex(
                name: "IX_BXJGUtilsAttachments_OrderIndex",
                table: "BXJGUtilsAttachments",
                column: "OrderIndex");

            migrationBuilder.CreateIndex(
                name: "IX_BXJGUtilsAttachments_PropertyName",
                table: "BXJGUtilsAttachments",
                column: "PropertyName");

            migrationBuilder.CreateIndex(
                name: "IX_BXJGTag_EntityId",
                table: "BXJGTag",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_BXJGTag_EntityType",
                table: "BXJGTag",
                column: "EntityType");

            migrationBuilder.CreateIndex(
                name: "IX_BXJGTag_PropertyName",
                table: "BXJGTag",
                column: "PropertyName");

            migrationBuilder.CreateIndex(
                name: "IX_BXJGUtilsFeedbacks_CreationTime",
                table: "BXJGUtilsFeedbacks",
                column: "CreationTime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BXJGTag");

            migrationBuilder.DropTable(
                name: "BXJGUtilsFeedbacks");

            migrationBuilder.DropIndex(
                name: "IX_BXJGUtilsFiles_RealName",
                table: "BXJGUtilsFiles");

            migrationBuilder.DropIndex(
                name: "IX_BXJGUtilsFiles_ResponseContentType",
                table: "BXJGUtilsFiles");

            migrationBuilder.DropIndex(
                name: "IX_BXJGUtilsFiles_Size",
                table: "BXJGUtilsFiles");

            migrationBuilder.DropIndex(
                name: "IX_BXJGUtilsAttachments_EntityId",
                table: "BXJGUtilsAttachments");

            migrationBuilder.DropIndex(
                name: "IX_BXJGUtilsAttachments_EntityType",
                table: "BXJGUtilsAttachments");

            migrationBuilder.DropIndex(
                name: "IX_BXJGUtilsAttachments_OrderIndex",
                table: "BXJGUtilsAttachments");

            migrationBuilder.DropIndex(
                name: "IX_BXJGUtilsAttachments_PropertyName",
                table: "BXJGUtilsAttachments");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "BXJGUtilsFiles");

            migrationBuilder.DropColumn(
                name: "PropertyDisplayName",
                table: "BXJGUtilsAttachments");

            migrationBuilder.AlterTable(
                name: "BXJGUtilsFiles",
                oldComment: "通用的文件表");

            migrationBuilder.AlterTable(
                name: "BXJGUtilsAttachments",
                oldComment: "通用的实体附件");

            migrationBuilder.AlterColumn<string>(
                name: "ExtensionData",
                table: "BXJGUtilsFiles",
                type: "varchar(4000)",
                nullable: true,
                comment: "扩展数据",
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldMaxLength: 4000,
                oldNullable: true,
                oldComment: "扩展数据");

            migrationBuilder.AlterColumn<string>(
                name: "PropertyName",
                table: "BXJGUtilsAttachments",
                type: "varchar(100)",
                nullable: true,
                comment: "属性名，可空 比如工单：字段A表示要处理的问题相关图片，字段B表示处理完成时拍摄的图片，它们都使用附件表，当通过此字段来表示关联的不同的属性",
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldUnicode: false,
                oldMaxLength: 100,
                oldComment: "属性名，可空 比如工单：字段A表示要处理的问题相关图片，字段B表示处理完成时拍摄的图片，它们都使用附件表，当通过此字段来表示关联的不同的属性");

            migrationBuilder.CreateIndex(
                name: "IX_BXJGUtilsAttachments_EntityType_EntityId_PropertyName",
                table: "BXJGUtilsAttachments",
                columns: new[] { "EntityType", "EntityId", "PropertyName" });
        }
    }
}
