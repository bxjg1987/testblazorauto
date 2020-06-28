using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZLJ.Migrations
{
    public partial class addShopitemcls : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BXJGShopItemCategories",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    Code = table.Column<string>(maxLength: 95, nullable: false),
                    ParentId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    DisplayName = table.Column<string>(maxLength: 128, nullable: false),
                    ExtensionData = table.Column<string>(nullable: true),
                    Icon = table.Column<string>(type: "varchar(200)", nullable: true),
                    Image1 = table.Column<string>(type: "varchar(200)", nullable: true),
                    Image2 = table.Column<string>(type: "varchar(200)", nullable: true),
                    ShowInHome = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BXJGShopItemCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BXJGShopItemCategories_BXJGShopItemCategories_ParentId",
                        column: x => x.ParentId,
                        principalTable: "BXJGShopItemCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BXJGShopItemCategories_ParentId",
                table: "BXJGShopItemCategories",
                column: "ParentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BXJGShopItemCategories");
        }
    }
}
