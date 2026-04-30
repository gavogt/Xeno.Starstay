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

        public DbSet<StarshipBooking> StarshipBookings => Set<StarshipBooking>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<StarshipListing>()
                .HasOne(listing => listing.HostUser)
                .WithMany()
                .HasForeignKey(listing => listing.HostUserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<StarshipBooking>()
                .HasOne(booking => booking.StarshipListing)
                .WithMany(listing => listing.Bookings)
                .HasForeignKey(booking => booking.StarshipListingId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<StarshipBooking>()
                .HasOne(booking => booking.GuestUser)
                .WithMany(user => user.VoyagerBookings)
                .HasForeignKey(booking => booking.GuestUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
