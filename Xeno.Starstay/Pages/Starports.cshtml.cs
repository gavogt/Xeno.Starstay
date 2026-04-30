using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Xeno.Starstay.Data;
using Xeno.Starstay.Models;

namespace Xeno.Starstay.Pages
{
    public class StarportsModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public StarportsModel(
            ApplicationDbContext dbContext,
            UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public List<StarshipListing> Listings { get; private set; } = new();

        public List<StarshipBooking> MyBookings { get; private set; } = new();

        public string? StatusMessage { get; private set; }

        public string StatusCssClass { get; private set; } = "auth-status-info";

        [TempData]
        public string? TempStatusMessage { get; set; }

        [TempData]
        public string? TempStatusTone { get; set; }

        public async Task OnGetAsync()
        {
            ApplyQueuedStatus();
            await LoadListingsAsync();
            await LoadMyBookingsAsync();
        }

        private async Task LoadListingsAsync()
        {
            Listings = await _dbContext.StarshipListings
                .AsNoTracking()
                .Include(listing => listing.HostUser)
                .OrderByDescending(listing => listing.CreatedUtc)
                .ToListAsync();
        }

        private async Task LoadMyBookingsAsync()
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrWhiteSpace(userId))
            {
                MyBookings = new List<StarshipBooking>();
                return;
            }

            var today = DateOnly.FromDateTime(DateTime.UtcNow.Date);

            MyBookings = await _dbContext.StarshipBookings
                .AsNoTracking()
                .Include(booking => booking.StarshipListing)
                .Where(booking => booking.GuestUserId == userId && booking.CheckOutDate >= today)
                .OrderBy(booking => booking.CheckInDate)
                .ThenBy(booking => booking.StarshipListing!.VesselName)
                .ToListAsync();
        }

        private void ApplyQueuedStatus()
        {
            if (string.IsNullOrWhiteSpace(TempStatusMessage))
            {
                return;
            }

            StatusMessage = TempStatusMessage;
            StatusCssClass = string.Equals(TempStatusTone, "success", StringComparison.OrdinalIgnoreCase)
                ? "auth-status-success"
                : "auth-status-info";
        }
    }
}
