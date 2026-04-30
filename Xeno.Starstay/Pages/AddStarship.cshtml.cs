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
    public class AddStarshipModel : PageModel
    {
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
        public AddStarshipInputModel Input { get; set; } = new();

        public List<StarshipListing> Listings { get; set; } = new();

        public string? StatusMessage { get; set; }

        public bool WasSuccessful { get; set; }

        public async Task OnGetAsync()
        {
            await LoadListingsAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                WasSuccessful = false;
                StatusMessage = "Your listing needs a few fixes before it can launch.";
                await LoadListingsAsync();
                return Page();
            }

            if (Input.PhotoFile is null && string.IsNullOrWhiteSpace(Input.PhotoUrl))
            {
                ModelState.AddModelError(string.Empty, "Add either a starship photo upload or a photo URL.");
                WasSuccessful = false;
                StatusMessage = "Your listing needs a photo before it can launch.";
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
                WasSuccessful = false;
                StatusMessage = "Your listing photo needs attention before it can launch.";
                await LoadListingsAsync();
                return Page();
            }

            var listing = new StarshipListing
            {
                VesselName = Input.VesselName,
                AlienLocation = Input.AlienLocation,
                PhotoUrl = photoUrl,
                NightlyRate = Input.NightlyRate,
                Summary = Input.Summary,
                HostUserId = user.Id,
                CreatedUtc = DateTime.UtcNow
            };

            _dbContext.StarshipListings.Add(listing);
            await _dbContext.SaveChangesAsync();

            WasSuccessful = true;
            StatusMessage = $"{listing.VesselName} is now listed on Starstay.";
            Input = new();
            await LoadListingsAsync();
            return Page();
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
                .Where(listing => listing.HostUserId == user.Id)
                .OrderByDescending(listing => listing.CreatedUtc)
                .ToListAsync();
        }

        private async Task<string> ResolvePhotoUrlAsync()
        {
            if (Input.PhotoFile is null)
            {
                return Input.PhotoUrl.Trim();
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

            [Display(Name = "Photo URL")]
            public string PhotoUrl { get; set; } = string.Empty;

            [Display(Name = "Upload photo")]
            public IFormFile? PhotoFile { get; set; }

            [Required]
            [Range(1, 1000000)]
            [Display(Name = "Nightly price")]
            public decimal NightlyRate { get; set; }

            [Required]
            [StringLength(320)]
            [Display(Name = "Listing summary")]
            public string Summary { get; set; } = string.Empty;
        }
    }
}
