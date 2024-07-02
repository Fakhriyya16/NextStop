
namespace Domain.Entities
{
    public class AppUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<Itinerary> Itineraries { get; set; }
        public Subscription Subscription { get; set; }
    }
}
