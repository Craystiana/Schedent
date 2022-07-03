using Microsoft.EntityFrameworkCore.Migrations;

namespace Schedent.DataAccess.Migrations
{
    public partial class AddEventsSent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_ProfessorId",
                table: "Users");

            migrationBuilder.AddColumn<bool>(
                name: "EventsSent",
                table: "Users",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Users_ProfessorId",
                table: "Users",
                column: "ProfessorId",
                unique: true,
                filter: "[ProfessorId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_ProfessorId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "EventsSent",
                table: "Users");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ProfessorId",
                table: "Users",
                column: "ProfessorId");
        }
    }
}
