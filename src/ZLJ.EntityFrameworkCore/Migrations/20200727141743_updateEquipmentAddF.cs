using Microsoft.EntityFrameworkCore.Migrations;

namespace ZLJ.Migrations
{
    public partial class updateEquipmentAddF : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "AreaId",
                table: "BXJGEquipmentInfo",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HardwareCode",
                table: "BXJGEquipmentInfo",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BXJGEquipmentInfo_AreaId",
                table: "BXJGEquipmentInfo",
                column: "AreaId");

            migrationBuilder.AddForeignKey(
                name: "FK_BXJGEquipmentInfo_BXJGBaseInfoAdministratives_AreaId",
                table: "BXJGEquipmentInfo",
                column: "AreaId",
                principalTable: "BXJGBaseInfoAdministratives",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BXJGEquipmentInfo_BXJGBaseInfoAdministratives_AreaId",
                table: "BXJGEquipmentInfo");

            migrationBuilder.DropIndex(
                name: "IX_BXJGEquipmentInfo_AreaId",
                table: "BXJGEquipmentInfo");

            migrationBuilder.DropColumn(
                name: "AreaId",
                table: "BXJGEquipmentInfo");

            migrationBuilder.DropColumn(
                name: "HardwareCode",
                table: "BXJGEquipmentInfo");
        }
    }
}
