using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace backend.Migrations
{
    public partial class EditFlightsTableChangeDelayColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DelayTime",
                table: "Flights");

            migrationBuilder.AddColumn<string>(
                name: "DelayInfo",
                table: "Flights",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DelayInfo",
                table: "Flights");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "DelayTime",
                table: "Flights",
                type: "time",
                nullable: true);
        }
    }
}
