using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Xeno.Starstay.Migrations
{
    /// <inheritdoc />
    public partial class SeedStarterStarstayListings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "DisplayName", "Email", "EmailConfirmed", "IsHost", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "Species", "TwoFactorEnabled", "UserName" },
                values: new object[] { "starstay-seed-host-curator", 0, "a4c140f0-1e42-4830-ac92-aa3c36d4f1d3", "Starstay Curator", "curator@starstay.local", true, true, false, null, "CURATOR@STARSTAY.LOCAL", "CURATOR@STARSTAY.LOCAL", null, null, false, "5e7da812-b0b3-4f01-b10e-582c415acaa1", "Zeta Reticulan", false, "curator@starstay.local" });

            migrationBuilder.InsertData(
                table: "StarshipListings",
                columns: new[] { "Id", "AlienLocation", "AllowsAlienPets", "AmenityNotes", "AtmosphereProfile", "CreatedUtc", "GravityProfile", "HasBioluminescentSpa", "HasOxygen", "HasQuantumDock", "HostUserId", "NightlyRate", "PhotoUrl", "Summary", "SupportsSiliconLifeforms", "VesselName" },
                values: new object[,]
                {
                    { 1001, "Orbit of Velora Prime", true, "Pet alcoves, dew-fed greenhouse bar, and dusk-cycle observation petals.", "Oxygen-rich", new DateTime(2026, 1, 5, 18, 0, 0, 0, DateTimeKind.Utc), "Earthlike pull", true, true, false, "starstay-seed-host-curator", 428.00m, "/images/seed-listings/velora-orchid-ring.png", "A living garden suite suspended inside an orchid halo with breathable air and moonlit lounge petals.", false, "The Velora Orchid Ring" },
                    { 1002, "Shard Caverns of Khepri-9", false, "Mineral-safe surfaces, resonance sleeping pod, and fracture-light meditation gallery.", "Variable blend filters", new DateTime(2026, 1, 6, 18, 0, 0, 0, DateTimeKind.Utc), "Low-gravity drift", false, false, false, "starstay-seed-host-curator", 512.00m, "/images/seed-listings/prism-silica-vault.png", "Crystal-carved luxury for mineral-based travelers who want reflective stillness and gemstone acoustics.", true, "The Prism Silica Vault" },
                    { 1003, "Methane Crown above Oort Lysithea", false, "Warp-lift docking perch, pressure-calibrated lounge, and thunderwatch observation wall.", "Methane-friendly", new DateTime(2026, 1, 7, 18, 0, 0, 0, DateTimeKind.Utc), "Earthlike pull", false, false, true, "starstay-seed-host-curator", 389.00m, "/images/seed-listings/amber-gaslight-pod.png", "A pressurized refinery-lounge retreat bathed in amber haze and gas giant lightning beyond the glass.", false, "The Amber Gaslight Pod" },
                    { 1004, "Meridian Dock Nine", false, "Hover lounge, grav-tuned sleep deck, and concierge docking beacon access.", "Oxygen-rich", new DateTime(2026, 1, 8, 18, 0, 0, 0, DateTimeKind.Utc), "Adjustable grav-field", false, true, true, "starstay-seed-host-curator", 601.00m, "/images/seed-listings/halo-drift-observatory.png", "A floating observatory loft with adjustable gravity and front-row views of premium docking traffic.", false, "The Halo Drift Observatory" },
                    { 1005, "Moonpool Crescent of Naia V", false, "Glow-tide spa basin, shell resonance audio, and waterglass meditation corridor.", "Oxygen-rich", new DateTime(2026, 1, 9, 18, 0, 0, 0, DateTimeKind.Utc), "Moonlight float", true, true, false, "starstay-seed-host-curator", 544.00m, "/images/seed-listings/biolume-tide-conservatory.png", "A translucent marine biosuite with breathable sea-light, moonpool rituals, and luminous rest decks.", false, "The Biolume Tide Conservatory" },
                    { 1006, "Rift Forge of Volkris", false, "Magma-view parlor, basalt sleep dais, and ritual dining chamber carved into the forge wall.", "Variable blend filters", new DateTime(2026, 1, 10, 18, 0, 0, 0, DateTimeKind.Utc), "Earthlike pull", false, false, false, "starstay-seed-host-curator", 468.00m, "/images/seed-listings/obsidian-ember-sanctum.png", "An obsidian refuge carved into a volcanic chasm for travelers who like their luxury ceremonial and dark.", true, "The Obsidian Ember Sanctum" },
                    { 1007, "Cloudreef Belts of Aurel", true, "Aerial companion roosts, cloud deck tea rail, and featherweight hammock canopy.", "Oxygen-rich", new DateTime(2026, 1, 11, 18, 0, 0, 0, DateTimeKind.Utc), "Low-gravity drift", false, true, false, "starstay-seed-host-curator", 497.00m, "/images/seed-listings/aurelian-cloudreef-nest.png", "A sky-lodge above luminous cloud reefs with floating gardens and pet-friendly perch zones.", false, "The Aurelian Cloudreef Nest" },
                    { 1008, "Crystal Dunes of Sereph-4", false, "Dune prism terrace, mineral-safe concierge core, and warp-side arcology docking spine.", "Variable blend filters", new DateTime(2026, 1, 12, 18, 0, 0, 0, DateTimeKind.Utc), "Earthlike pull", false, true, true, "starstay-seed-host-curator", 533.00m, "/images/seed-listings/glass-dune-arcology.png", "A crystalline desert suite inside a megadome where mineral architecture glows at golden dusk.", true, "The Glass Dune Arcology" },
                    { 1009, "Frost Halo of Ilya Station", false, "Aurora bath chamber, thermal cocoon bedding, and glacier-lit reading pod.", "Oxygen-rich", new DateTime(2026, 1, 13, 18, 0, 0, 0, DateTimeKind.Utc), "Earthlike pull", true, true, false, "starstay-seed-host-curator", 457.00m, "/images/seed-listings/polar-aurora-refuge.png", "An icy observatory refuge with aurora windows, heat-woven textiles, and restorative wellness alcoves.", false, "The Polar Aurora Refuge" },
                    { 1010, "Living Reef of Thalassa Gate", true, "Coral bloom canopy, companion tide pool, and reef-charged ambient wellness chamber.", "Oxygen-rich", new DateTime(2026, 1, 14, 18, 0, 0, 0, DateTimeKind.Utc), "Earthlike pull", true, true, false, "starstay-seed-host-curator", 578.00m, "/images/seed-listings/coral-sun-atrium.png", "A warm coral-vessel suite grown into a living reef canopy with golden tide light and bioengineered comfort.", false, "The Coral Sun Atrium" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "StarshipListings",
                keyColumn: "Id",
                keyValue: 1001);

            migrationBuilder.DeleteData(
                table: "StarshipListings",
                keyColumn: "Id",
                keyValue: 1002);

            migrationBuilder.DeleteData(
                table: "StarshipListings",
                keyColumn: "Id",
                keyValue: 1003);

            migrationBuilder.DeleteData(
                table: "StarshipListings",
                keyColumn: "Id",
                keyValue: 1004);

            migrationBuilder.DeleteData(
                table: "StarshipListings",
                keyColumn: "Id",
                keyValue: 1005);

            migrationBuilder.DeleteData(
                table: "StarshipListings",
                keyColumn: "Id",
                keyValue: 1006);

            migrationBuilder.DeleteData(
                table: "StarshipListings",
                keyColumn: "Id",
                keyValue: 1007);

            migrationBuilder.DeleteData(
                table: "StarshipListings",
                keyColumn: "Id",
                keyValue: 1008);

            migrationBuilder.DeleteData(
                table: "StarshipListings",
                keyColumn: "Id",
                keyValue: 1009);

            migrationBuilder.DeleteData(
                table: "StarshipListings",
                keyColumn: "Id",
                keyValue: 1010);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "starstay-seed-host-curator");
        }
    }
}
