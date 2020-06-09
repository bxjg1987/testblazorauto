using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZLJ.Migrations
{
    public partial class updateArticle3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "PublishEndTime",
                table: "BXJGCMSArticles",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "PublishStartTime",
                table: "BXJGCMSArticles",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Published",
                table: "BXJGCMSArticles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SeoDescription",
                table: "BXJGCMSArticles",
                maxLength: 5000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SeoKeyword",
                table: "BXJGCMSArticles",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SeoTitle",
                table: "BXJGCMSArticles",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Summary",
                table: "BXJGCMSArticles",
                maxLength: 5000,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublishEndTime",
                table: "BXJGCMSArticles");

            migrationBuilder.DropColumn(
                name: "PublishStartTime",
                table: "BXJGCMSArticles");

            migrationBuilder.DropColumn(
                name: "Published",
                table: "BXJGCMSArticles");

            migrationBuilder.DropColumn(
                name: "SeoDescription",
                table: "BXJGCMSArticles");

            migrationBuilder.DropColumn(
                name: "SeoKeyword",
                table: "BXJGCMSArticles");

            migrationBuilder.DropColumn(
                name: "SeoTitle",
                table: "BXJGCMSArticles");

            migrationBuilder.DropColumn(
                name: "Summary",
                table: "BXJGCMSArticles");
        }
    }
}
