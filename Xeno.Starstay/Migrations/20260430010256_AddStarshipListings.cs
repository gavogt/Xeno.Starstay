using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Xeno.Starstay.Migrations
{
    /// <inheritdoc />
    public partial class AddStarshipListings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StarshipListings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VesselName = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    AlienLocation = table.Column<string>(type: "nvarchar(140)", maxLength: 140, nullable: false),
                    PhotoUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NightlyRate = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Summary = table.Column<string>(type: "nvarchar(320)", maxLength: 320, nullable: false),
                    HostUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StarshipListings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StarshipListings_AspNetUsers_HostUserId",
                        column: x => x.HostUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StarshipListings_HostUserId",
                table: "StarshipListings",
                column: "HostUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StarshipListings");
        }
    }
}
