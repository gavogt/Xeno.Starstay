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

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; } = string.Empty;

        [BindProperty(SupportsGet = true)]
        public string SelectedLocation { get; set; } = string.Empty;

        [BindProperty(SupportsGet = true)]
        public bool RequiresOxygen { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool RequiresAlienPets { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool RequiresSiliconSupport { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool RequiresQuantumDock { get; set; }

        public List<StarshipListing> Listings { get; private set; } = new();

        public List<TravelerBookingViewModel> UpcomingBookings { get; private set; } = new();

        public List<TravelerBookingViewModel> BookingHistory { get; private set; } = new();

        public List<string> AvailableLocations { get; private set; } = new();

        public string? StatusMessage { get; private set; }

        public string StatusCssClass { get; private set; } = "auth-status-info";

        public bool HasActiveFilters =>
            !string.IsNullOrWhiteSpace(SearchTerm) ||
            !string.IsNullOrWhiteSpace(SelectedLocation) ||
            RequiresOxygen ||
            RequiresAlienPets ||
            RequiresSiliconSupport ||
            RequiresQuantumDock;

        [TempData]
        public string? TempStatusMessage { get; set; }

        [TempData]
        public string? TempStatusTone { get; set; }

        public async Task OnGetAsync()
        {
            ApplyQueuedStatus();
            await LoadListingsAsync();
            await LoadTravelerBookingsAsync();
        }

        public async Task<IActionResult> OnPostCancelBookingAsync(int bookingId)
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Challenge();
            }

            var booking = await _dbContext.StarshipBookings
                .Include(item => item.StarshipListing)
                .FirstOrDefaultAsync(item => item.Id == bookingId && item.GuestUserId == userId);

            if (booking is null)
            {
                TempStatusMessage = "That booking could not be found on your traveler manifest.";
                TempStatusTone = "info";
                return RedirectToPage(new
                {
                    SearchTerm,
                    SelectedLocation,
                    RequiresOxygen,
                    RequiresAlienPets,
                    RequiresSiliconSupport,
                    RequiresQuantumDock
                });
            }

            var today = DateOnly.FromDateTime(DateTime.UtcNow.Date);

            if (booking.IsCancelled)
            {
                TempStatusMessage = "That voyage was already cancelled.";
                TempStatusTone = "info";
                return RedirectToPage(new
                {
                    SearchTerm,
                    SelectedLocation,
                    RequiresOxygen,
                    RequiresAlienPets,
                    RequiresSiliconSupport,
                    RequiresQuantumDock
                });
            }

            if (booking.CheckInDate <= today)
            {
                TempStatusMessage = "Only future voyages can be cancelled from the traveler deck.";
                TempStatusTone = "info";
                return RedirectToPage(new
                {
                    SearchTerm,
                    SelectedLocation,
                    RequiresOxygen,
                    RequiresAlienPets,
                    RequiresSiliconSupport,
                    RequiresQuantumDock
                });
            }

            booking.IsCancelled = true;
            booking.CancelledUtc = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync();

            TempStatusMessage = $"{booking.StarshipListing?.VesselName ?? "Your voyage"} was cancelled and moved into booking history.";
            TempStatusTone = "success";

            return RedirectToPage(new
            {
                SearchTerm,
                SelectedLocation,
                RequiresOxygen,
                RequiresAlienPets,
                RequiresSiliconSupport,
                RequiresQuantumDock
            });
        }

        private async Task LoadListingsAsync()
        {
            AvailableLocations = await _dbContext.StarshipListings
                .AsNoTracking()
                .OrderBy(listing => listing.AlienLocation)
                .Select(listing => listing.AlienLocation)
                .Distinct()
                .ToListAsync();

            var listingsQuery = _dbContext.StarshipListings
                .AsNoTracking()
                .Include(listing => listing.HostUser)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                var term = SearchTerm.Trim();
                var normalizedTerm = term.ToLowerInvariant();
                var matchesOxygenKeyword = normalizedTerm.Contains("oxygen") || normalizedTerm.Contains("breathable");
                var matchesPetsKeyword = normalizedTerm.Contains("pet") || normalizedTerm.Contains("pets") || normalizedTerm.Contains("drake") || normalizedTerm.Contains("drakes") || normalizedTerm.Contains("companion");
                var matchesSiliconKeyword = normalizedTerm.Contains("silicon") || normalizedTerm.Contains("mineral");
                var matchesQuantumKeyword = normalizedTerm.Contains("quantum") || normalizedTerm.Contains("dock") || normalizedTerm.Contains("docking") || normalizedTerm.Contains("warp");
                var matchesSpaKeyword = normalizedTerm.Contains("spa") || normalizedTerm.Contains("glow") || normalizedTerm.Contains("bioluminescent");

                listingsQuery = listingsQuery.Where(listing =>
                    listing.VesselName.Contains(term) ||
                    listing.HostUser!.DisplayName.Contains(term) ||
                    listing.AlienLocation.Contains(term) ||
                    listing.Summary.Contains(term) ||
                    listing.AtmosphereProfile.Contains(term) ||
                    listing.GravityProfile.Contains(term) ||
                    (listing.AmenityNotes != null && listing.AmenityNotes.Contains(term)) ||
                    (matchesOxygenKeyword && listing.HasOxygen) ||
                    (matchesPetsKeyword && listing.AllowsAlienPets) ||
                    (matchesSiliconKeyword && listing.SupportsSiliconLifeforms) ||
                    (matchesQuantumKeyword && listing.HasQuantumDock) ||
                    (matchesSpaKeyword && listing.HasBioluminescentSpa));
            }

            if (!string.IsNullOrWhiteSpace(SelectedLocation))
            {
                var location = SelectedLocation.Trim();
                listingsQuery = listingsQuery.Where(listing => listing.AlienLocation == location);
            }

            if (RequiresOxygen)
            {
                listingsQuery = listingsQuery.Where(listing => listing.HasOxygen);
            }

            if (RequiresAlienPets)
            {
                listingsQuery = listingsQuery.Where(listing => listing.AllowsAlienPets);
            }

            if (RequiresSiliconSupport)
            {
                listingsQuery = listingsQuery.Where(listing => listing.SupportsSiliconLifeforms);
            }

            if (RequiresQuantumDock)
            {
                listingsQuery = listingsQuery.Where(listing => listing.HasQuantumDock);
            }

            Listings = await listingsQuery
                .OrderByDescending(listing => listing.CreatedUtc)
                .ToListAsync();
        }

        private async Task LoadTravelerBookingsAsync()
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrWhiteSpace(userId))
            {
                UpcomingBookings = new List<TravelerBookingViewModel>();
                BookingHistory = new List<TravelerBookingViewModel>();
                return;
            }

            var today = DateOnly.FromDateTime(DateTime.UtcNow.Date);

            var travelerBookings = await _dbContext.StarshipBookings
                .AsNoTracking()
                .Include(booking => booking.StarshipListing)
                .Where(booking => booking.GuestUserId == userId)
                .OrderByDescending(booking => booking.CheckInDate)
                .ThenByDescending(booking => booking.CreatedUtc)
                .ToListAsync();

            UpcomingBookings = travelerBookings
                .Where(booking => !booking.IsCancelled && booking.CheckOutDate >= today)
                .OrderBy(booking => booking.CheckInDate)
                .Select(booking => ToViewModel(booking, today))
                .ToList();

            BookingHistory = travelerBookings
                .Where(booking => booking.IsCancelled || booking.CheckOutDate < today)
                .Select(booking => ToViewModel(booking, today))
                .ToList();
        }

        private static TravelerBookingViewModel ToViewModel(StarshipBooking booking, DateOnly today)
        {
            var status = booking.IsCancelled
                ? "Cancelled"
                : booking.CheckOutDate < today
                    ? "Completed"
                    : booking.CheckInDate <= today
                        ? "In Progress"
                        : "Confirmed";

            var toneClass = booking.IsCancelled
                ? "voyage-status-cancelled"
                : booking.CheckOutDate < today
                    ? "voyage-status-completed"
                    : booking.CheckInDate <= today
                        ? "voyage-status-active"
                        : "voyage-status-confirmed";

            var canCancel = !booking.IsCancelled && booking.CheckInDate > today;

            return new TravelerBookingViewModel
            {
                Booking = booking,
                StatusLabel = status,
                StatusClass = toneClass,
                CanCancel = canCancel
            };
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

        public class TravelerBookingViewModel
        {
            public StarshipBooking Booking { get; init; } = null!;

            public string StatusLabel { get; init; } = string.Empty;

            public string StatusClass { get; init; } = string.Empty;

            public bool CanCancel { get; init; }
        }
    }
}
