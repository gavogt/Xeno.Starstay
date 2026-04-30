using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Xeno.Starstay.Migrations
{
    /// <inheritdoc />
    public partial class AddSpeciesCompatibilityToListings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SpeciesCompatibility",
                table: "StarshipListings",
                type: "nvarchar(80)",
                maxLength: 80,
                nullable: false,
                defaultValue: "Zeta Reticulan");

            migrationBuilder.UpdateData(
                table: "StarshipListings",
                keyColumn: "Id",
                keyValue: 1001,
                column: "SpeciesCompatibility",
                value: "Nordic Envoy");

            migrationBuilder.UpdateData(
                table: "StarshipListings",
                keyColumn: "Id",
                keyValue: 1002,
                column: "SpeciesCompatibility",
                value: "Mantis Collective");

            migrationBuilder.UpdateData(
                table: "StarshipListings",
                keyColumn: "Id",
                keyValue: 1003,
                column: "SpeciesCompatibility",
                value: "Reptilian Diplomat");

            migrationBuilder.UpdateData(
                table: "StarshipListings",
                keyColumn: "Id",
                keyValue: 1004,
                column: "SpeciesCompatibility",
                value: "Time Traveler");

            migrationBuilder.UpdateData(
                table: "StarshipListings",
                keyColumn: "Id",
                keyValue: 1005,
                column: "SpeciesCompatibility",
                value: "Pleiadian Voyager");

            migrationBuilder.UpdateData(
                table: "StarshipListings",
                keyColumn: "Id",
                keyValue: 1006,
                column: "SpeciesCompatibility",
                value: "Zeta Reticulan");

            migrationBuilder.UpdateData(
                table: "StarshipListings",
                keyColumn: "Id",
                keyValue: 1007,
                column: "SpeciesCompatibility",
                value: "Grey Hybrid");

            migrationBuilder.UpdateData(
                table: "StarshipListings",
                keyColumn: "Id",
                keyValue: 1008,
                column: "SpeciesCompatibility",
                value: "Martian Colonist");

            migrationBuilder.UpdateData(
                table: "StarshipListings",
                keyColumn: "Id",
                keyValue: 1009,
                column: "SpeciesCompatibility",
                value: "Vulcan Scholar");

            migrationBuilder.UpdateData(
                table: "StarshipListings",
                keyColumn: "Id",
                keyValue: 1010,
                column: "SpeciesCompatibility",
                value: "Asgard Wayfarer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SpeciesCompatibility",
                table: "StarshipListings");
        }
    }
}
