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
        [Url]
        [Display(Name = "Photo URL")]
        public string PhotoUrl { get; set; } = string.Empty;

        [Required]
        [Range(1, 1000000)]
        [Column(TypeName = "decimal(10,2)")]
        [Display(Name = "Nightly price")]
        public decimal NightlyRate { get; set; }

        [Required]
        [StringLength(320)]
        [Display(Name = "Listing summary")]
        public string Summary { get; set; } = string.Empty;

        [Required]
        public string HostUserId { get; set; } = string.Empty;

        public ApplicationUser? HostUser { get; set; }

        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    }
}
