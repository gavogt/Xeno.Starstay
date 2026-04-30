using Xeno.Starstay.Models;

namespace Xeno.Starstay.Data
{
    public static class StarstaySeedCatalog
    {
        public const string SeedHostId = "starstay-seed-host-curator";

        public static ApplicationUser SeedHost => new()
        {
            Id = SeedHostId,
            UserName = "curator@starstay.local",
            NormalizedUserName = "CURATOR@STARSTAY.LOCAL",
            Email = "curator@starstay.local",
            NormalizedEmail = "CURATOR@STARSTAY.LOCAL",
            EmailConfirmed = true,
            DisplayName = "Starstay Curator",
            Species = "Zeta Reticulan",
            IsHost = true,
            SecurityStamp = "5e7da812-b0b3-4f01-b10e-582c415acaa1",
            ConcurrencyStamp = "a4c140f0-1e42-4830-ac92-aa3c36d4f1d3",
            PasswordHash = null,
            PhoneNumberConfirmed = false,
            TwoFactorEnabled = false,
            LockoutEnabled = false,
            AccessFailedCount = 0
        };

        public static IEnumerable<StarshipListing> SeedListings()
        {
            return new[]
            {
                new StarshipListing
                {
                    Id = 1001,
                    VesselName = "The Velora Orchid Ring",
                    AlienLocation = "Orbit of Velora Prime",
                    PhotoUrl = "/images/seed-listings/velora-orchid-ring.png",
                    NightlyRate = 428.00m,
                    Summary = "A living garden suite suspended inside an orchid halo with breathable air and moonlit lounge petals.",
                    AtmosphereProfile = "Oxygen-rich",
                    GravityProfile = "Earthlike pull",
                    AllowsAlienPets = true,
                    HasOxygen = true,
                    SupportsSiliconLifeforms = false,
                    HasQuantumDock = false,
                    HasBioluminescentSpa = true,
                    AmenityNotes = "Pet alcoves, dew-fed greenhouse bar, and dusk-cycle observation petals.",
                    HostUserId = SeedHostId,
                    CreatedUtc = new DateTime(2026, 1, 5, 18, 0, 0, DateTimeKind.Utc)
                },
                new StarshipListing
                {
                    Id = 1002,
                    VesselName = "The Prism Silica Vault",
                    AlienLocation = "Shard Caverns of Khepri-9",
                    PhotoUrl = "/images/seed-listings/prism-silica-vault.png",
                    NightlyRate = 512.00m,
                    Summary = "Crystal-carved luxury for mineral-based travelers who want reflective stillness and gemstone acoustics.",
                    AtmosphereProfile = "Variable blend filters",
                    GravityProfile = "Low-gravity drift",
                    AllowsAlienPets = false,
                    HasOxygen = false,
                    SupportsSiliconLifeforms = true,
                    HasQuantumDock = false,
                    HasBioluminescentSpa = false,
                    AmenityNotes = "Mineral-safe surfaces, resonance sleeping pod, and fracture-light meditation gallery.",
                    HostUserId = SeedHostId,
                    CreatedUtc = new DateTime(2026, 1, 6, 18, 0, 0, DateTimeKind.Utc)
                },
                new StarshipListing
                {
                    Id = 1003,
                    VesselName = "The Amber Gaslight Pod",
                    AlienLocation = "Methane Crown above Oort Lysithea",
                    PhotoUrl = "/images/seed-listings/amber-gaslight-pod.png",
                    NightlyRate = 389.00m,
                    Summary = "A pressurized refinery-lounge retreat bathed in amber haze and gas giant lightning beyond the glass.",
                    AtmosphereProfile = "Methane-friendly",
                    GravityProfile = "Earthlike pull",
                    AllowsAlienPets = false,
                    HasOxygen = false,
                    SupportsSiliconLifeforms = false,
                    HasQuantumDock = true,
                    HasBioluminescentSpa = false,
                    AmenityNotes = "Warp-lift docking perch, pressure-calibrated lounge, and thunderwatch observation wall.",
                    HostUserId = SeedHostId,
                    CreatedUtc = new DateTime(2026, 1, 7, 18, 0, 0, DateTimeKind.Utc)
                },
                new StarshipListing
                {
                    Id = 1004,
                    VesselName = "The Halo Drift Observatory",
                    AlienLocation = "Meridian Dock Nine",
                    PhotoUrl = "/images/seed-listings/halo-drift-observatory.png",
                    NightlyRate = 601.00m,
                    Summary = "A floating observatory loft with adjustable gravity and front-row views of premium docking traffic.",
                    AtmosphereProfile = "Oxygen-rich",
                    GravityProfile = "Adjustable grav-field",
                    AllowsAlienPets = false,
                    HasOxygen = true,
                    SupportsSiliconLifeforms = false,
                    HasQuantumDock = true,
                    HasBioluminescentSpa = false,
                    AmenityNotes = "Hover lounge, grav-tuned sleep deck, and concierge docking beacon access.",
                    HostUserId = SeedHostId,
                    CreatedUtc = new DateTime(2026, 1, 8, 18, 0, 0, DateTimeKind.Utc)
                },
                new StarshipListing
                {
                    Id = 1005,
                    VesselName = "The Biolume Tide Conservatory",
                    AlienLocation = "Moonpool Crescent of Naia V",
                    PhotoUrl = "/images/seed-listings/biolume-tide-conservatory.png",
                    NightlyRate = 544.00m,
                    Summary = "A translucent marine biosuite with breathable sea-light, moonpool rituals, and luminous rest decks.",
                    AtmosphereProfile = "Oxygen-rich",
                    GravityProfile = "Moonlight float",
                    AllowsAlienPets = false,
                    HasOxygen = true,
                    SupportsSiliconLifeforms = false,
                    HasQuantumDock = false,
                    HasBioluminescentSpa = true,
                    AmenityNotes = "Glow-tide spa basin, shell resonance audio, and waterglass meditation corridor.",
                    HostUserId = SeedHostId,
                    CreatedUtc = new DateTime(2026, 1, 9, 18, 0, 0, DateTimeKind.Utc)
                },
                new StarshipListing
                {
                    Id = 1006,
                    VesselName = "The Obsidian Ember Sanctum",
                    AlienLocation = "Rift Forge of Volkris",
                    PhotoUrl = "/images/seed-listings/obsidian-ember-sanctum.png",
                    NightlyRate = 468.00m,
                    Summary = "An obsidian refuge carved into a volcanic chasm for travelers who like their luxury ceremonial and dark.",
                    AtmosphereProfile = "Variable blend filters",
                    GravityProfile = "Earthlike pull",
                    AllowsAlienPets = false,
                    HasOxygen = false,
                    SupportsSiliconLifeforms = true,
                    HasQuantumDock = false,
                    HasBioluminescentSpa = false,
                    AmenityNotes = "Magma-view parlor, basalt sleep dais, and ritual dining chamber carved into the forge wall.",
                    HostUserId = SeedHostId,
                    CreatedUtc = new DateTime(2026, 1, 10, 18, 0, 0, DateTimeKind.Utc)
                },
                new StarshipListing
                {
                    Id = 1007,
                    VesselName = "The Aurelian Cloudreef Nest",
                    AlienLocation = "Cloudreef Belts of Aurel",
                    PhotoUrl = "/images/seed-listings/aurelian-cloudreef-nest.png",
                    NightlyRate = 497.00m,
                    Summary = "A sky-lodge above luminous cloud reefs with floating gardens and pet-friendly perch zones.",
                    AtmosphereProfile = "Oxygen-rich",
                    GravityProfile = "Low-gravity drift",
                    AllowsAlienPets = true,
                    HasOxygen = true,
                    SupportsSiliconLifeforms = false,
                    HasQuantumDock = false,
                    HasBioluminescentSpa = false,
                    AmenityNotes = "Aerial companion roosts, cloud deck tea rail, and featherweight hammock canopy.",
                    HostUserId = SeedHostId,
                    CreatedUtc = new DateTime(2026, 1, 11, 18, 0, 0, DateTimeKind.Utc)
                },
                new StarshipListing
                {
                    Id = 1008,
                    VesselName = "The Glass Dune Arcology",
                    AlienLocation = "Crystal Dunes of Sereph-4",
                    PhotoUrl = "/images/seed-listings/glass-dune-arcology.png",
                    NightlyRate = 533.00m,
                    Summary = "A crystalline desert suite inside a megadome where mineral architecture glows at golden dusk.",
                    AtmosphereProfile = "Variable blend filters",
                    GravityProfile = "Earthlike pull",
                    AllowsAlienPets = false,
                    HasOxygen = true,
                    SupportsSiliconLifeforms = true,
                    HasQuantumDock = true,
                    HasBioluminescentSpa = false,
                    AmenityNotes = "Dune prism terrace, mineral-safe concierge core, and warp-side arcology docking spine.",
                    HostUserId = SeedHostId,
                    CreatedUtc = new DateTime(2026, 1, 12, 18, 0, 0, DateTimeKind.Utc)
                },
                new StarshipListing
                {
                    Id = 1009,
                    VesselName = "The Polar Aurora Refuge",
                    AlienLocation = "Frost Halo of Ilya Station",
                    PhotoUrl = "/images/seed-listings/polar-aurora-refuge.png",
                    NightlyRate = 457.00m,
                    Summary = "An icy observatory refuge with aurora windows, heat-woven textiles, and restorative wellness alcoves.",
                    AtmosphereProfile = "Oxygen-rich",
                    GravityProfile = "Earthlike pull",
                    AllowsAlienPets = false,
                    HasOxygen = true,
                    SupportsSiliconLifeforms = false,
                    HasQuantumDock = false,
                    HasBioluminescentSpa = true,
                    AmenityNotes = "Aurora bath chamber, thermal cocoon bedding, and glacier-lit reading pod.",
                    HostUserId = SeedHostId,
                    CreatedUtc = new DateTime(2026, 1, 13, 18, 0, 0, DateTimeKind.Utc)
                },
                new StarshipListing
                {
                    Id = 1010,
                    VesselName = "The Coral Sun Atrium",
                    AlienLocation = "Living Reef of Thalassa Gate",
                    PhotoUrl = "/images/seed-listings/coral-sun-atrium.png",
                    NightlyRate = 578.00m,
                    Summary = "A warm coral-vessel suite grown into a living reef canopy with golden tide light and bioengineered comfort.",
                    AtmosphereProfile = "Oxygen-rich",
                    GravityProfile = "Earthlike pull",
                    AllowsAlienPets = true,
                    HasOxygen = true,
                    SupportsSiliconLifeforms = false,
                    HasQuantumDock = false,
                    HasBioluminescentSpa = true,
                    AmenityNotes = "Coral bloom canopy, companion tide pool, and reef-charged ambient wellness chamber.",
                    HostUserId = SeedHostId,
                    CreatedUtc = new DateTime(2026, 1, 14, 18, 0, 0, DateTimeKind.Utc)
                }
            };
        }
    }
}
