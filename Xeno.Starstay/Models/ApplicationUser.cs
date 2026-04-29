using Microsoft.AspNetCore.Identity;

namespace Xeno.Starstay.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string DisplayName { get; set; } = string.Empty;

        public string Species { get; set; } = string.Empty;

        public bool IsHost { get; set; }
    }
}
