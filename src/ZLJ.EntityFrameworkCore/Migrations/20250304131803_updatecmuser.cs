using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZLJ.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class updatecmuser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AbpUsers_baseinfo_associated_company_AssociatedCompanyEntityId",
                table: "AbpUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AbpUsers_baseinfo_associated_company_CustomerId",
                table: "AbpUsers");

            migrationBuilder.DropIndex(
                name: "IX_baseinfo_associated_company_AdminId",
                table: "baseinfo_associated_company");

            migrationBuilder.DropIndex(
                name: "IX_AbpUsers_AssociatedCompanyEntityId",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "AssociatedCompanyEntityId",
                table: "AbpUsers");

            migrationBuilder.CreateIndex(
                name: "IX_baseinfo_associated_company_AdminId",
                table: "baseinfo_associated_company",
                column: "AdminId",
                unique: true,
                filter: "[AdminId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AbpUsers_baseinfo_associated_company_CustomerId",
                table: "AbpUsers",
                column: "CustomerId",
                principalTable: "baseinfo_associated_company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AbpUsers_baseinfo_associated_company_CustomerId",
                table: "AbpUsers");

            migrationBuilder.DropIndex(
                name: "IX_baseinfo_associated_company_AdminId",
                table: "baseinfo_associated_company");

            migrationBuilder.AddColumn<long>(
                name: "AssociatedCompanyEntityId",
                table: "AbpUsers",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_baseinfo_associated_company_AdminId",
                table: "baseinfo_associated_company",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpUsers_AssociatedCompanyEntityId",
                table: "AbpUsers",
                column: "AssociatedCompanyEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_AbpUsers_baseinfo_associated_company_AssociatedCompanyEntityId",
                table: "AbpUsers",
                column: "AssociatedCompanyEntityId",
                principalTable: "baseinfo_associated_company",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AbpUsers_baseinfo_associated_company_CustomerId",
                table: "AbpUsers",
                column: "CustomerId",
                principalTable: "baseinfo_associated_company",
                principalColumn: "Id");
        }
    }
}
