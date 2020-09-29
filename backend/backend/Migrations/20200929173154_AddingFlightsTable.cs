using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace backend.Migrations
{
    public partial class AddingFlightsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Flights",
                columns: table => new
                {
                    FlightId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepartureDateTime = table.Column<DateTime>(nullable: false),
                    ArrivalDateTime = table.Column<DateTime>(nullable: false),
                    DepartureCity = table.Column<string>(nullable: true),
                    ArrivalCity = table.Column<string>(nullable: true),
                    DelayTime = table.Column<TimeSpan>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flights", x => x.FlightId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Flights");
        }
    }
}
