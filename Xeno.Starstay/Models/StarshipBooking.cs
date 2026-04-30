using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Xeno.Starstay.Models
{
    public class StarshipBooking
    {
        public int Id { get; set; }

        [Required]
        public int StarshipListingId { get; set; }

        public StarshipListing? StarshipListing { get; set; }

        [Required]
        public string GuestUserId { get; set; } = string.Empty;

        public ApplicationUser? GuestUser { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Arrival date")]
        public DateOnly CheckInDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Departure date")]
        public DateOnly CheckOutDate { get; set; }

        [Range(1, 100000)]
        [Display(Name = "Booked cycles")]
        public int CycleCount { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        [Display(Name = "Total nebula credits")]
        public decimal TotalNebulaCredits { get; set; }

        [StringLength(280)]
        [Display(Name = "Arrival protocol notes")]
        public string? ArrivalNotes { get; set; }

        [Display(Name = "Booking cancelled")]
        public bool IsCancelled { get; set; }

        [Display(Name = "Cancelled at")]
        public DateTime? CancelledUtc { get; set; }

        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    }
}
