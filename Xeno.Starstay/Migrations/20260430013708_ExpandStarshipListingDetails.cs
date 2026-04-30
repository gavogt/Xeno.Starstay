using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Xeno.Starstay.Migrations
{
    /// <inheritdoc />
    public partial class ExpandStarshipListingDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PhotoUrl",
                table: "StarshipListings",
                type: "nvarchar(400)",
                maxLength: 400,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<bool>(
                name: "AllowsAlienPets",
                table: "StarshipListings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "AmenityNotes",
                table: "StarshipListings",
                type: "nvarchar(280)",
                maxLength: 280,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AtmosphereProfile",
                table: "StarshipListings",
                type: "nvarchar(80)",
                maxLength: 80,
                nullable: false,
                defaultValue: "Oxygen-rich");

            migrationBuilder.AddColumn<string>(
                name: "GravityProfile",
                table: "StarshipListings",
                type: "nvarchar(80)",
                maxLength: 80,
                nullable: false,
                defaultValue: "Earthlike pull");

            migrationBuilder.AddColumn<bool>(
                name: "HasBioluminescentSpa",
                table: "StarshipListings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasOxygen",
                table: "StarshipListings",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasQuantumDock",
                table: "StarshipListings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SupportsSiliconLifeforms",
                table: "StarshipListings",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllowsAlienPets",
                table: "StarshipListings");

            migrationBuilder.DropColumn(
                name: "AmenityNotes",
                table: "StarshipListings");

            migrationBuilder.DropColumn(
                name: "AtmosphereProfile",
                table: "StarshipListings");

            migrationBuilder.DropColumn(
                name: "GravityProfile",
                table: "StarshipListings");

            migrationBuilder.DropColumn(
                name: "HasBioluminescentSpa",
                table: "StarshipListings");

            migrationBuilder.DropColumn(
                name: "HasOxygen",
                table: "StarshipListings");

            migrationBuilder.DropColumn(
                name: "HasQuantumDock",
                table: "StarshipListings");

            migrationBuilder.DropColumn(
                name: "SupportsSiliconLifeforms",
                table: "StarshipListings");

            migrationBuilder.AlterColumn<string>(
                name: "PhotoUrl",
                table: "StarshipListings",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(400)",
                oldMaxLength: 400);
        }
    }
}
