using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Xeno.Starstay.Data;
using Xeno.Starstay.Models;

namespace Xeno.Starstay.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;

        public RegisterModel(UserManager<ApplicationUser> userManager, ILogger<RegisterModel> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        [BindProperty]
        public RegisterInputModel Input { get; set; } = new();

        public string? StatusMessage { get; set; }

        public bool WasSuccessful { get; set; }

        public string? SupportReference { get; set; }

        public IReadOnlyList<SelectListItem> SpeciesOptions => SpeciesCatalog.BuildOptions(Input.Species, "Select your species");

        public IActionResult OnGet()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToPage("/Index");
            }

            WasSuccessful = false;
            SupportReference = null;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (!SpeciesCatalog.SpeciesOptions.Contains(Input.Species, StringComparer.Ordinal))
                {
                    ModelState.AddModelError(nameof(Input.Species), "Choose a species from the Starstay registry.");
                }

                if (!ModelState.IsValid)
                {
                    Response.StatusCode = StatusCodes.Status400BadRequest;
                    WasSuccessful = false;
                    SupportReference = null;
                    StatusMessage = "Registration could not be completed. Review the highlighted fields and try again.";
                    return Page();
                }

                var existingUser = await _userManager.FindByEmailAsync(Input.Email);
                if (existingUser is not null)
                {
                    Response.StatusCode = StatusCodes.Status409Conflict;
                    WasSuccessful = false;
                    SupportReference = null;
                    StatusMessage = "An account with that email already exists. Try logging in instead.";
                    ModelState.AddModelError(string.Empty, "An account with that email already exists.");
                    return Page();
                }

                var user = new ApplicationUser
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    DisplayName = Input.DisplayName,
                    Species = Input.Species,
                    IsHost = Input.IsHost
                };

                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("Registration succeeded for {Email}.", Input.Email);
                    WasSuccessful = true;
                    SupportReference = null;
                    StatusMessage = "Registration succeeded. You can log in now.";
                    ModelState.Clear();
                    Input = new();
                    return Page();
                }

                _logger.LogWarning("Registration failed for {Email} with {ErrorCount} errors.", Input.Email, result.Errors.Count());
                Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
                WasSuccessful = false;
                SupportReference = null;
                StatusMessage = "Registration failed. Review the messages below and try again.";
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return Page();
            }
            catch (Exception ex)
            {
                SupportReference = HttpContext.TraceIdentifier;
                _logger.LogError(ex, "Registration threw an exception for {Email}. TraceId: {TraceId}", Input.Email, SupportReference);
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                WasSuccessful = false;
                StatusMessage = "Registration hit a server error. Please try again.";
                ModelState.AddModelError(string.Empty, "We couldn't complete registration because of a server error. Please try again.");
                return Page();
            }
        }

        public class RegisterInputModel
        {
            [Required]
            [Display(Name = "Display name")]
            public string DisplayName { get; set; } = string.Empty;

            [Required]
            [Display(Name = "Species")]
            public string Species { get; set; } = string.Empty;

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; } = string.Empty;

            [Required]
            [StringLength(100, MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; } = string.Empty;

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare(nameof(Password))]
            public string ConfirmPassword { get; set; } = string.Empty;

            [Display(Name = "I want to host stays on Starstay")]
            public bool IsHost { get; set; }

            [MustBeTrue(ErrorMessage = "You must accept the traveler charter.")]
            [Display(Name = "I accept the traveler charter and privacy terms")]
            public bool AcceptTerms { get; set; }
        }
    }
}
