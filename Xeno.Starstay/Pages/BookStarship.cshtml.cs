using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Xeno.Starstay.Data;
using Xeno.Starstay.Models;

namespace Xeno.Starstay.Pages
{
    [Authorize]
    public class BookStarshipModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public BookStarshipModel(
            ApplicationDbContext dbContext,
            UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        [BindProperty]
        public BookingInputModel Input { get; set; } = CreateDefaultInput();

        public StarshipListing? Listing { get; private set; }

        public List<StarshipBooking> UpcomingReservedWindows { get; private set; } = new();

        public List<StarshipBooking> MyBookingsForListing { get; private set; } = new();

        public List<StarshipBooking> CancelledBookingsForListing { get; private set; } = new();

        public List<AvailabilityMonthViewModel> AvailabilityCalendars { get; private set; } = new();

        public string? StatusMessage { get; private set; }

        public string StatusCssClass { get; private set; } = "auth-status-info";

        public string CurrentUserId { get; private set; } = string.Empty;

        [TempData]
        public string? TempStatusMessage { get; set; }

        [TempData]
        public string? TempStatusTone { get; set; }

        public bool IsOwnListing => Listing is not null && string.Equals(Listing.HostUserId, CurrentUserId, StringComparison.Ordinal);

        public DateOnly Today => DateOnly.FromDateTime(DateTime.UtcNow.Date);

        public async Task<IActionResult> OnGetAsync(int id)
        {
            CurrentUserId = _userManager.GetUserId(User) ?? string.Empty;
            await LoadPageDataAsync(id);

            if (Listing is null)
            {
                return NotFound();
            }

            ApplyQueuedStatus();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            CurrentUserId = _userManager.GetUserId(User) ?? string.Empty;
            await LoadPageDataAsync(id);

            if (Listing is null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                SetPageStatus("Choose a valid arrival and departure window before you confirm the voyage.");
                return Page();
            }

            if (IsOwnListing)
            {
                SetPageStatus("You cannot book your own starship listing. Switch to a traveler account or choose another stay.");
                return Page();
            }

            if (Input.CheckInDate is null || Input.CheckOutDate is null)
            {
                SetPageStatus("Choose both an arrival date and a departure date.");
                return Page();
            }

            var today = DateOnly.FromDateTime(DateTime.UtcNow.Date);
            var checkInDate = Input.CheckInDate.Value;
            var checkOutDate = Input.CheckOutDate.Value;

            if (checkInDate < today)
            {
                ModelState.AddModelError(nameof(Input.CheckInDate), "Arrival must be today or later.");
            }

            if (checkOutDate <= checkInDate)
            {
                ModelState.AddModelError(nameof(Input.CheckOutDate), "Departure must be after arrival.");
            }

            var cycleCount = checkOutDate.DayNumber - checkInDate.DayNumber;
            if (cycleCount <= 0)
            {
                ModelState.AddModelError(string.Empty, "Your voyage must span at least one full cycle.");
            }

            if (!ModelState.IsValid)
            {
                SetPageStatus("The selected travel window needs a quick correction before booking.");
                return Page();
            }

            var hasOverlap = await _dbContext.StarshipBookings
                .AnyAsync(booking =>
                    booking.StarshipListingId == Listing.Id &&
                    !booking.IsCancelled &&
                    checkInDate < booking.CheckOutDate &&
                    booking.CheckInDate < checkOutDate);

            if (hasOverlap)
            {
                ModelState.AddModelError(string.Empty, "Those cycles are already reserved. Choose a different travel window.");
                SetPageStatus("That orbit window is already booked.");
                await LoadPageDataAsync(id);
                return Page();
            }

            var booking = new StarshipBooking
            {
                StarshipListingId = Listing.Id,
                GuestUserId = CurrentUserId,
                CheckInDate = checkInDate,
                CheckOutDate = checkOutDate,
                CycleCount = cycleCount,
                TotalNebulaCredits = cycleCount * Listing.NightlyRate,
                ArrivalNotes = string.IsNullOrWhiteSpace(Input.ArrivalNotes) ? null : Input.ArrivalNotes.Trim(),
                CreatedUtc = DateTime.UtcNow
            };

            _dbContext.StarshipBookings.Add(booking);
            await _dbContext.SaveChangesAsync();

            TempStatusMessage = $"Voyage confirmed aboard {Listing.VesselName} for {cycleCount} cycle{(cycleCount == 1 ? string.Empty : "s")}.";
            TempStatusTone = "success";
            return RedirectToPage(new { id });
        }

        private async Task LoadPageDataAsync(int listingId)
        {
            Listing = await _dbContext.StarshipListings
                .AsNoTracking()
                .Include(listing => listing.HostUser)
                .FirstOrDefaultAsync(listing => listing.Id == listingId);

            if (Listing is null)
            {
                UpcomingReservedWindows = new List<StarshipBooking>();
                MyBookingsForListing = new List<StarshipBooking>();
                CancelledBookingsForListing = new List<StarshipBooking>();
                AvailabilityCalendars = new List<AvailabilityMonthViewModel>();
                return;
            }

            var today = DateOnly.FromDateTime(DateTime.UtcNow.Date);
            var calendarStart = new DateOnly(today.Year, today.Month, 1);
            var calendarHorizonExclusive = calendarStart.AddMonths(2);

            UpcomingReservedWindows = await _dbContext.StarshipBookings
                .AsNoTracking()
                .Where(booking => booking.StarshipListingId == listingId && !booking.IsCancelled && booking.CheckOutDate >= today)
                .OrderBy(booking => booking.CheckInDate)
                .Take(6)
                .ToListAsync();

            if (!string.IsNullOrWhiteSpace(CurrentUserId))
            {
                MyBookingsForListing = await _dbContext.StarshipBookings
                    .AsNoTracking()
                    .Where(booking =>
                        booking.StarshipListingId == listingId &&
                        booking.GuestUserId == CurrentUserId &&
                        !booking.IsCancelled &&
                        booking.CheckOutDate >= today)
                    .OrderBy(booking => booking.CheckInDate)
                    .ToListAsync();

                CancelledBookingsForListing = await _dbContext.StarshipBookings
                    .AsNoTracking()
                    .Where(booking =>
                        booking.StarshipListingId == listingId &&
                        booking.GuestUserId == CurrentUserId &&
                        booking.IsCancelled)
                    .OrderByDescending(booking => booking.CancelledUtc)
                    .Take(4)
                    .ToListAsync();
            }
            else
            {
                MyBookingsForListing = new List<StarshipBooking>();
                CancelledBookingsForListing = new List<StarshipBooking>();
            }

            var calendarBookings = await _dbContext.StarshipBookings
                .AsNoTracking()
                .Where(booking =>
                    booking.StarshipListingId == listingId &&
                    !booking.IsCancelled &&
                    booking.CheckInDate < calendarHorizonExclusive &&
                    booking.CheckOutDate > calendarStart)
                .OrderBy(booking => booking.CheckInDate)
                .ToListAsync();

            AvailabilityCalendars = BuildAvailabilityCalendars(calendarStart, calendarBookings);
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

        private void SetPageStatus(string message, string tone = "info")
        {
            StatusMessage = message;
            StatusCssClass = string.Equals(tone, "success", StringComparison.OrdinalIgnoreCase)
                ? "auth-status-success"
                : "auth-status-info";
        }

        private static BookingInputModel CreateDefaultInput()
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow.Date);

            return new BookingInputModel
            {
                CheckInDate = today.AddDays(2),
                CheckOutDate = today.AddDays(5)
            };
        }

        private List<AvailabilityMonthViewModel> BuildAvailabilityCalendars(
            DateOnly calendarStart,
            List<StarshipBooking> calendarBookings)
        {
            var calendars = new List<AvailabilityMonthViewModel>();
            var selectedCheckIn = Input.CheckInDate;
            var selectedCheckOut = Input.CheckOutDate;

            for (var monthOffset = 0; monthOffset < 2; monthOffset++)
            {
                var monthStart = calendarStart.AddMonths(monthOffset);
                var nextMonthStart = monthStart.AddMonths(1);
                var days = new List<AvailabilityDayViewModel>();

                for (var blankIndex = 0; blankIndex < (int)monthStart.DayOfWeek; blankIndex++)
                {
                    days.Add(AvailabilityDayViewModel.CreateBlank());
                }

                for (var day = monthStart; day < nextMonthStart; day = day.AddDays(1))
                {
                    var bookingForDay = calendarBookings.FirstOrDefault(booking =>
                        day >= booking.CheckInDate &&
                        day < booking.CheckOutDate);

                    var isPast = day < Today;
                    var isSelected =
                        selectedCheckIn.HasValue &&
                        selectedCheckOut.HasValue &&
                        day >= selectedCheckIn.Value &&
                        day < selectedCheckOut.Value;

                    var isMyBooking = bookingForDay is not null &&
                        string.Equals(bookingForDay.GuestUserId, CurrentUserId, StringComparison.Ordinal);

                    var statusClass = isPast
                        ? "availability-day-past"
                        : isMyBooking
                            ? "availability-day-mine"
                            : bookingForDay is not null
                                ? "availability-day-booked"
                                : "availability-day-open";

                    if (isSelected && !isPast)
                    {
                        statusClass += " availability-day-selected";
                    }

                    var statusLabel = isPast
                        ? "Past date"
                        : isMyBooking
                            ? "Your booked cycle"
                            : bookingForDay is not null
                                ? "Reserved"
                                : "Available";

                    days.Add(new AvailabilityDayViewModel
                    {
                        DayNumber = day.Day,
                        IsoDate = day.ToString("yyyy-MM-dd"),
                        StatusClass = statusClass,
                        StatusLabel = statusLabel,
                        IsInteractive = !isPast
                    });
                }

                calendars.Add(new AvailabilityMonthViewModel
                {
                    MonthLabel = monthStart.ToString("MMMM yyyy"),
                    Days = days
                });
            }

            return calendars;
        }

        public class BookingInputModel
        {
            [Required]
            [DataType(DataType.Date)]
            [Display(Name = "Arrival date")]
            public DateOnly? CheckInDate { get; set; }

            [Required]
            [DataType(DataType.Date)]
            [Display(Name = "Departure date")]
            public DateOnly? CheckOutDate { get; set; }

            [StringLength(280)]
            [Display(Name = "Arrival protocol notes")]
            public string? ArrivalNotes { get; set; }
        }

        public class AvailabilityMonthViewModel
        {
            public string MonthLabel { get; init; } = string.Empty;

            public List<AvailabilityDayViewModel> Days { get; init; } = new();
        }

        public class AvailabilityDayViewModel
        {
            public int? DayNumber { get; init; }

            public string IsoDate { get; init; } = string.Empty;

            public string StatusClass { get; init; } = string.Empty;

            public string StatusLabel { get; init; } = string.Empty;

            public bool IsInteractive { get; init; }

            public bool IsBlank => !DayNumber.HasValue;

            public static AvailabilityDayViewModel CreateBlank()
            {
                return new AvailabilityDayViewModel
                {
                    DayNumber = null,
                    IsoDate = string.Empty,
                    StatusClass = "availability-day-blank",
                    StatusLabel = string.Empty,
                    IsInteractive = false
                };
            }
        }
    }
}
