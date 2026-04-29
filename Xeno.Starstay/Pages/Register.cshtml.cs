using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
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

        public string? DebugDetails { get; set; }

        public IActionResult OnGet()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToPage("/Index");
            }

            WasSuccessful = false;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            StatusMessage = "POST HANDLER WAS HIT.";
            DebugDetails = $"Posted at {DateTime.Now}";

            try
            {
                if (!ModelState.IsValid)
                {
                    Response.StatusCode = StatusCodes.Status400BadRequest;
                    WasSuccessful = false;
                    StatusMessage = "Registration could not be completed. Review the highlighted fields and try again.";
                    DebugDetails = BuildModelStateDiagnostics("Model validation failed before user creation.");
                    return Page();
                }

                var existingUser = await _userManager.FindByEmailAsync(Input.Email);
                if (existingUser is not null)
                {
                    Response.StatusCode = StatusCodes.Status409Conflict;
                    WasSuccessful = false;
                    StatusMessage = "An account with that email already exists. Try logging in instead.";
                    ModelState.AddModelError(string.Empty, "An account with that email already exists.");
                    DebugDetails = $"A user with email '{Input.Email}' already exists in AspNetUsers.";
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
                    StatusMessage = $"Registration succeeded for {Input.Email}. Your account was created in StarstayAuthDb. You can log in now.";
                    DebugDetails = $"CreateAsync succeeded for '{Input.Email}'. A row should now exist in AspNetUsers.{Environment.NewLine}UserName stored: {user.UserName}{Environment.NewLine}Email stored: {user.Email}{Environment.NewLine}Display name: {user.DisplayName}{Environment.NewLine}Species: {user.Species}{Environment.NewLine}IsHost: {user.IsHost}";
                    ModelState.Clear();
                    Input = new();
                    return Page();
                }

                _logger.LogWarning("Registration failed for {Email} with {ErrorCount} errors.", Input.Email, result.Errors.Count());
                Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
                WasSuccessful = false;
                StatusMessage = "Registration failed. Review the messages below and try again.";
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                DebugDetails = "Identity CreateAsync returned failure:" + Environment.NewLine
                    + string.Join(Environment.NewLine, result.Errors.Select(error =>
                        $"- Code: {error.Code} | Description: {error.Description}"));

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Registration threw an exception for {Email}.", Input.Email);
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                WasSuccessful = false;
                StatusMessage = "Registration hit a server error before the user could be inserted.";
                ModelState.AddModelError(string.Empty, $"Server error during registration: {ex.Message}");
                DebugDetails = $"Exception type: {ex.GetType().FullName}{Environment.NewLine}Message: {ex.Message}{Environment.NewLine}{Environment.NewLine}{ex}";
                return Page();
            }
        }

        private string BuildModelStateDiagnostics(string header)
        {
            var lines = new List<string> { header };

            foreach (var entry in ModelState)
            {
                foreach (var error in entry.Value.Errors)
                {
                    var key = string.IsNullOrWhiteSpace(entry.Key) ? "(model)" : entry.Key;
                    lines.Add($"- {key}: {error.ErrorMessage}");
                }
            }

            return string.Join(Environment.NewLine, lines);
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
