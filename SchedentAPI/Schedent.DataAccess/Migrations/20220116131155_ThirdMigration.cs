using Microsoft.EntityFrameworkCore.Migrations;

namespace Schedent.DataAccess.Migrations
{
    public partial class ThirdMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Subgroups_SubgroupId",
                table: "Users");

            migrationBuilder.AlterColumn<int>(
                name: "SubgroupId",
                table: "Users",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Subgroups_SubgroupId",
                table: "Users",
                column: "SubgroupId",
                principalTable: "Subgroups",
                principalColumn: "SubgroupId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Subgroups_SubgroupId",
                table: "Users");

            migrationBuilder.AlterColumn<int>(
                name: "SubgroupId",
                table: "Users",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Subgroups_SubgroupId",
                table: "Users",
                column: "SubgroupId",
                principalTable: "Subgroups",
                principalColumn: "SubgroupId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
