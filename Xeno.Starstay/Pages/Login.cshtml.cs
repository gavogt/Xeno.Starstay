using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Xeno.Starstay.Models;

namespace Xeno.Starstay.Pages
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            ILogger<LoginModel> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
        }

        [BindProperty]
        public LoginInputModel Input { get; set; } = new();

        public string? StatusMessage { get; set; }

        public IActionResult OnGet(bool? registered = null, string? email = null)
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToPage("/Index");
            }

            if (registered == true)
            {
                StatusMessage = "Registration succeeded. You can log in now.";
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    StatusMessage = "Login could not be submitted. Review the highlighted fields and try again.";
                    return Page();
                }

                var result = await _signInManager.PasswordSignInAsync(
                    Input.Email,
                    Input.Password,
                    Input.RememberMe,
                    lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(Input.Email);
                    if (user is not null)
                    {
                        HttpContext.Session.SetString("DisplayName", user.DisplayName);
                        HttpContext.Session.SetString("UserEmail", user.Email ?? Input.Email);
                    }

                    StatusMessage = "Welcome back to Starstay.";
                    return RedirectToPage("/Index");
                }

                _logger.LogWarning("Login failed for {Email}.", Input.Email);
                StatusMessage = "Login failed. Check your email and password and try again.";
                ModelState.AddModelError(string.Empty, "Login failed. Check your email and password and try again.");
                return Page();
            }
            catch (Exception ex)
            {
                var traceId = HttpContext.TraceIdentifier;
                _logger.LogError(ex, "Login threw an exception for {Email}. TraceId: {TraceId}", Input.Email, traceId);
                StatusMessage = $"Login hit a server error. Reference ID: {traceId}";
                ModelState.AddModelError(string.Empty, "We couldn't complete login because of a server error. Please try again.");
                return Page();
            }
        }

        public class LoginInputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; } = string.Empty;

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; } = string.Empty;

            [Display(Name = "Keep me logged in")]
            public bool RememberMe { get; set; }
        }
    }
}
