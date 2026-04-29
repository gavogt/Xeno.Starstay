using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Xeno.Starstay.Pages
{
    public class RegisterModel : PageModel
    {
        [BindProperty]
        public RegisterInputModel Input { get; set; } = new();

        [TempData]
        public string? StatusMessage { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            StatusMessage = $"Welcome aboard, {Input.DisplayName}. Your Starstay registration screen is ready for backend account creation.";
            ModelState.Clear();
            Input = new();
            return Page();
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
            [StringLength(100, MinimumLength = 8)]
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

            [Range(typeof(bool), "true", "true", ErrorMessage = "You must accept the traveler charter.")]
            [Display(Name = "I accept the traveler charter and privacy terms")]
            public bool AcceptTerms { get; set; }
        }
    }
}
