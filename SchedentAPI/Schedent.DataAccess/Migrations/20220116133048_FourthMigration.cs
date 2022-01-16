using Microsoft.EntityFrameworkCore.Migrations;

namespace Schedent.DataAccess.Migrations
{
    public partial class FourthMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProfessorId",
                table: "Users",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_ProfessorId",
                table: "Users",
                column: "ProfessorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Professors_ProfessorId",
                table: "Users",
                column: "ProfessorId",
                principalTable: "Professors",
                principalColumn: "ProfessorId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Professors_ProfessorId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_ProfessorId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ProfessorId",
                table: "Users");
        }
    }
}
