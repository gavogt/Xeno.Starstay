using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Xeno.Starstay.Migrations
{
    /// <inheritdoc />
    public partial class AddStarshipBookings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StarshipBookings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StarshipListingId = table.Column<int>(type: "int", nullable: false),
                    GuestUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CheckInDate = table.Column<DateOnly>(type: "date", nullable: false),
                    CheckOutDate = table.Column<DateOnly>(type: "date", nullable: false),
                    CycleCount = table.Column<int>(type: "int", nullable: false),
                    TotalNebulaCredits = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    ArrivalNotes = table.Column<string>(type: "nvarchar(280)", maxLength: 280, nullable: true),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StarshipBookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StarshipBookings_AspNetUsers_GuestUserId",
                        column: x => x.GuestUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StarshipBookings_StarshipListings_StarshipListingId",
                        column: x => x.StarshipListingId,
                        principalTable: "StarshipListings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StarshipBookings_GuestUserId",
                table: "StarshipBookings",
                column: "GuestUserId");

            migrationBuilder.CreateIndex(
                name: "IX_StarshipBookings_StarshipListingId",
                table: "StarshipBookings",
                column: "StarshipListingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StarshipBookings");
        }
    }
}
