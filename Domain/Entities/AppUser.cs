
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class AppUser : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<Itinerary> Itineraries { get; set; }
        public int SubscriptionId { get; set; }
        public Subscription Subscription { get; set; }
        public ICollection<Blog> Blogs { get; set; }
        public ICollection<Favorite> Favorites { get; set; }
    }
}
