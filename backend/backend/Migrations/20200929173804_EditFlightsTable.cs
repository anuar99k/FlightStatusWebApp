using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace backend.Migrations
{
    public partial class EditFlightsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DepartureCity",
                table: "Flights",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "DelayTime",
                table: "Flights",
                nullable: true,
                oldClrType: typeof(TimeSpan),
                oldType: "time");

            migrationBuilder.AlterColumn<string>(
                name: "ArrivalCity",
                table: "Flights",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DepartureCity",
                table: "Flights",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "DelayTime",
                table: "Flights",
                type: "time",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ArrivalCity",
                table: "Flights",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
