using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Schedent.DataAccess.Migrations
{
    public partial class SecondMigrate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "StartsAt",
                table: "Schedules",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "StartsAt",
                table: "Schedules",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
