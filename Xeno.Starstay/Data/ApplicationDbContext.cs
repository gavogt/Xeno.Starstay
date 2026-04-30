using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Xeno.Starstay.Models;

namespace Xeno.Starstay.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<StarshipListing> StarshipListings => Set<StarshipListing>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<StarshipListing>()
                .HasOne(listing => listing.HostUser)
                .WithMany()
                .HasForeignKey(listing => listing.HostUserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
