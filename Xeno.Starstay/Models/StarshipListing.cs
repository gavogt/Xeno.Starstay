using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Xeno.Starstay.Models
{
    public class StarshipListing
    {
        public int Id { get; set; }

        [Required]
        [StringLength(120)]
        [Display(Name = "Starship name")]
        public string VesselName { get; set; } = string.Empty;

        [Required]
        [StringLength(140)]
        [Display(Name = "Alien location")]
        public string AlienLocation { get; set; } = string.Empty;

        [Required]
        [StringLength(400)]
        [Display(Name = "Photo URL")]
        public string PhotoUrl { get; set; } = string.Empty;

        [Required]
        [Range(1, 1000000)]
        [Column(TypeName = "decimal(10,2)")]
        [Display(Name = "Nightly nebula credits")]
        public decimal NightlyRate { get; set; }

        [Required]
        [StringLength(320)]
        [Display(Name = "Listing summary")]
        public string Summary { get; set; } = string.Empty;

        [Required]
        [StringLength(80)]
        [Display(Name = "Atmosphere profile")]
        public string AtmosphereProfile { get; set; } = "Oxygen-rich";

        [Required]
        [StringLength(80)]
        [Display(Name = "Gravity profile")]
        public string GravityProfile { get; set; } = "Earthlike pull";

        [Display(Name = "Alien pets allowed")]
        public bool AllowsAlienPets { get; set; }

        [Display(Name = "Oxygen supplied")]
        public bool HasOxygen { get; set; } = true;

        [Display(Name = "Supports silicon lifeforms")]
        public bool SupportsSiliconLifeforms { get; set; }

        [Display(Name = "Quantum dock access")]
        public bool HasQuantumDock { get; set; }

        [Display(Name = "Bioluminescent spa chamber")]
        public bool HasBioluminescentSpa { get; set; }

        [StringLength(280)]
        [Display(Name = "Extra habitat notes")]
        public string? AmenityNotes { get; set; }

        [Required]
        public string HostUserId { get; set; } = string.Empty;

        public ApplicationUser? HostUser { get; set; }

        public ICollection<StarshipBooking> Bookings { get; set; } = new List<StarshipBooking>();

        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    }
}
