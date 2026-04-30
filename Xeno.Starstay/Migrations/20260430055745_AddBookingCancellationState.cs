using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Xeno.Starstay.Migrations
{
    /// <inheritdoc />
    public partial class AddBookingCancellationState : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CancelledUtc",
                table: "StarshipBookings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCancelled",
                table: "StarshipBookings",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CancelledUtc",
                table: "StarshipBookings");

            migrationBuilder.DropColumn(
                name: "IsCancelled",
                table: "StarshipBookings");
        }
    }
}
