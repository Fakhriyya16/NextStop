
using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Reflection.Emit;
using System.Reflection.Metadata;

namespace Repository.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Place> Places { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<BlogTag> BlogTag { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Itinerary> Itineraries { get; set; }
        public DbSet<ItineraryDay> ItineraryDays { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PlaceImage> PlaceImages { get; set; }
        public DbSet<PlaceTag> PlaceTag { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<BlogImage> BlogImages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            builder.Entity<Country>().HasQueryFilter(m => !m.SoftDelete);
            builder.Entity<City>().HasQueryFilter(m => !m.SoftDelete);
            builder.Entity<Category>().HasQueryFilter(m => !m.SoftDelete);
            builder.Entity<Tag>().HasQueryFilter(m => !m.SoftDelete);
            builder.Entity<Place>().HasQueryFilter(m => !m.SoftDelete);
            builder.Entity<PlaceTag>().HasQueryFilter(m => !m.SoftDelete);


            base.OnModelCreating(builder);
        }
    }
}
