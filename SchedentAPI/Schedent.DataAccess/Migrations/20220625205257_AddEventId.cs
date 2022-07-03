using Microsoft.EntityFrameworkCore.Migrations;

namespace Schedent.DataAccess.Migrations
{
    public partial class AddEventId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Subgroups_SubgroupId",
                table: "Notifications");

            migrationBuilder.AddColumn<string>(
                name: "EventId",
                table: "Schedules",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SubgroupId",
                table: "Notifications",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ProfessorId",
                table: "Notifications",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_ProfessorId",
                table: "Notifications",
                column: "ProfessorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Professors_ProfessorId",
                table: "Notifications",
                column: "ProfessorId",
                principalTable: "Professors",
                principalColumn: "ProfessorId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Subgroups_SubgroupId",
                table: "Notifications",
                column: "SubgroupId",
                principalTable: "Subgroups",
                principalColumn: "SubgroupId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Professors_ProfessorId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Subgroups_SubgroupId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_ProfessorId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "ProfessorId",
                table: "Notifications");

            migrationBuilder.AlterColumn<int>(
                name: "SubgroupId",
                table: "Notifications",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Subgroups_SubgroupId",
                table: "Notifications",
                column: "SubgroupId",
                principalTable: "Subgroups",
                principalColumn: "SubgroupId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
