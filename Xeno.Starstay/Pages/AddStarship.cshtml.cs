using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Xeno.Starstay.Data;
using Xeno.Starstay.Models;

namespace Xeno.Starstay.Pages
{
    [Authorize]
    public class AddStarshipModel : PageModel
    {
        private static readonly string[] AtmosphereChoices =
        {
            "Oxygen-rich",
            "Variable blend filters",
            "Methane-friendly",
            "Helium mist lounge",
            "Vacuum-sealed excursion"
        };

        private static readonly string[] GravityChoices =
        {
            "Earthlike pull",
            "Low-gravity drift",
            "Adjustable grav-field",
            "Zero-G tether lounge",
            "Moonlight float"
        };

        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _environment;

        public AddStarshipModel(
            ApplicationDbContext dbContext,
            UserManager<ApplicationUser> userManager,
            IWebHostEnvironment environment)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _environment = environment;
        }

        [BindProperty]
        public AddStarshipInputModel Input { get; set; } = CreateDefaultInput();

        public List<StarshipListing> Listings { get; set; } = new();

        public string? StatusMessage { get; private set; }

        public string StatusCssClass { get; private set; } = "auth-status-info";

        [TempData]
        public string? TempStatusMessage { get; set; }

        [TempData]
        public string? TempStatusTone { get; set; }

        public IReadOnlyList<SelectListItem> AtmosphereOptions => BuildOptions(AtmosphereChoices, Input.AtmosphereProfile);

        public IReadOnlyList<SelectListItem> GravityOptions => BuildOptions(GravityChoices, Input.GravityProfile);

        public async Task OnGetAsync()
        {
            ApplyQueuedStatus();
            await LoadListingsAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                SetPageStatus("Your listing needs a few fixes before it can launch.");
                await LoadListingsAsync();
                return Page();
            }

            if (Input.PhotoFile is null && string.IsNullOrWhiteSpace(Input.PhotoUrl))
            {
                ModelState.AddModelError(string.Empty, "Add either a starship photo upload or a photo URL.");
                SetPageStatus("Your listing needs a photo before it can launch.");
                await LoadListingsAsync();
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user is null)
            {
                return Challenge();
            }

            var photoUrl = await ResolvePhotoUrlAsync();
            if (!ModelState.IsValid)
            {
                SetPageStatus("Your listing photo needs attention before it can launch.");
                await LoadListingsAsync();
                return Page();
            }

            var listing = new StarshipListing
            {
                VesselName = Input.VesselName.Trim(),
                AlienLocation = Input.AlienLocation.Trim(),
                PhotoUrl = photoUrl,
                NightlyRate = Input.NightlyRate,
                Summary = Input.Summary.Trim(),
                AtmosphereProfile = Input.AtmosphereProfile,
                GravityProfile = Input.GravityProfile,
                AllowsAlienPets = Input.AllowsAlienPets,
                HasOxygen = Input.HasOxygen,
                SupportsSiliconLifeforms = Input.SupportsSiliconLifeforms,
                HasQuantumDock = Input.HasQuantumDock,
                HasBioluminescentSpa = Input.HasBioluminescentSpa,
                AmenityNotes = string.IsNullOrWhiteSpace(Input.AmenityNotes) ? null : Input.AmenityNotes.Trim(),
                HostUserId = user.Id,
                CreatedUtc = DateTime.UtcNow
            };

            _dbContext.StarshipListings.Add(listing);
            await _dbContext.SaveChangesAsync();

            QueueStatus($"{listing.VesselName} is now listed on Starstay.", "success");
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int listingId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
            {
                return Challenge();
            }

            var listing = await _dbContext.StarshipListings
                .FirstOrDefaultAsync(item => item.Id == listingId && item.HostUserId == user.Id);

            if (listing is null)
            {
                QueueStatus("That listing could not be found or is no longer yours to manage.");
                return RedirectToPage();
            }

            var hasBookings = await _dbContext.StarshipBookings
                .AnyAsync(booking => booking.StarshipListingId == listing.Id);

            if (hasBookings)
            {
                QueueStatus("This listing already has traveler bookings in orbit and cannot be deleted until those voyages are resolved.");
                return RedirectToPage();
            }

            DeleteUploadedPhoto(listing.PhotoUrl);

            _dbContext.StarshipListings.Remove(listing);
            await _dbContext.SaveChangesAsync();

            QueueStatus($"{listing.VesselName} was removed from Starstay. This action cannot be undone.", "success");
            return RedirectToPage();
        }

        private void ApplyQueuedStatus()
        {
            if (string.IsNullOrWhiteSpace(TempStatusMessage))
            {
                return;
            }

            StatusMessage = TempStatusMessage;
            StatusCssClass = IsSuccessTone(TempStatusTone) ? "auth-status-success" : "auth-status-info";
        }

        private void SetPageStatus(string message, string tone = "info")
        {
            StatusMessage = message;
            StatusCssClass = IsSuccessTone(tone) ? "auth-status-success" : "auth-status-info";
        }

        private void QueueStatus(string message, string tone = "info")
        {
            TempStatusMessage = message;
            TempStatusTone = tone;
        }

        private async Task LoadListingsAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
            {
                Listings = new List<StarshipListing>();
                return;
            }

            Listings = await _dbContext.StarshipListings
                .AsNoTracking()
                .Where(listing => listing.HostUserId == user.Id)
                .OrderByDescending(listing => listing.CreatedUtc)
                .ToListAsync();
        }

        private async Task<string> ResolvePhotoUrlAsync()
        {
            if (Input.PhotoFile is null)
            {
                var photoUrl = Input.PhotoUrl.Trim();

                if (photoUrl.StartsWith("/", StringComparison.Ordinal))
                {
                    return photoUrl;
                }

                if (!Uri.TryCreate(photoUrl, UriKind.Absolute, out var uri) ||
                    (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
                {
                    ModelState.AddModelError(nameof(Input.PhotoUrl), "Use a full http:// or https:// image URL.");
                    return string.Empty;
                }

                return photoUrl;
            }

            var extension = Path.GetExtension(Input.PhotoFile.FileName).ToLowerInvariant();
            var allowedExtensions = new HashSet<string> { ".jpg", ".jpeg", ".png", ".webp" };

            if (!allowedExtensions.Contains(extension))
            {
                ModelState.AddModelError(nameof(Input.PhotoFile), "Use a JPG, PNG, or WEBP image.");
                return string.Empty;
            }

            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "starships");
            Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid():N}{extension}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            await using var stream = System.IO.File.Create(filePath);
            await Input.PhotoFile.CopyToAsync(stream);

            return $"/uploads/starships/{fileName}";
        }

        private void DeleteUploadedPhoto(string photoUrl)
        {
            const string localUploadPrefix = "/uploads/starships/";

            if (!photoUrl.StartsWith(localUploadPrefix, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            var relativePath = photoUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
            var fullPath = Path.Combine(_environment.WebRootPath, relativePath);

            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
        }

        private static AddStarshipInputModel CreateDefaultInput()
        {
            return new AddStarshipInputModel
            {
                AtmosphereProfile = AtmosphereChoices[0],
                GravityProfile = GravityChoices[0],
                HasOxygen = true
            };
        }

        private static IReadOnlyList<SelectListItem> BuildOptions(IEnumerable<string> values, string selectedValue)
        {
            return values
                .Select(value => new SelectListItem(value, value, string.Equals(value, selectedValue, StringComparison.Ordinal)))
                .ToList();
        }

        private static bool IsSuccessTone(string? tone)
        {
            return string.Equals(tone, "success", StringComparison.OrdinalIgnoreCase);
        }

        public class AddStarshipInputModel
        {
            [Required]
            [StringLength(120)]
            [Display(Name = "Starship name")]
            public string VesselName { get; set; } = string.Empty;

            [Required]
            [StringLength(140)]
            [Display(Name = "Alien location")]
            public string AlienLocation { get; set; } = string.Empty;

            [Display(Name = "Optional external image URL")]
            public string PhotoUrl { get; set; } = string.Empty;

            [Display(Name = "Upload photo")]
            public IFormFile? PhotoFile { get; set; }

            [Required]
            [Range(1, 1000000)]
            [Display(Name = "Nightly nebula credits")]
            public decimal NightlyRate { get; set; }

            [Required]
            [StringLength(320)]
            [Display(Name = "Listing summary")]
            public string Summary { get; set; } = string.Empty;

            [Required]
            [StringLength(80)]
            [Display(Name = "Atmosphere profile")]
            public string AtmosphereProfile { get; set; } = AtmosphereChoices[0];

            [Required]
            [StringLength(80)]
            [Display(Name = "Gravity profile")]
            public string GravityProfile { get; set; } = GravityChoices[0];

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
        }
    }
}
