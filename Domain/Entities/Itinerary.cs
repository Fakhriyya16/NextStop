
using Domain.Common;

namespace Domain.Entities
{
    public class Itinerary : BaseEntity
    {
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public string Name { get; set; }
        public ICollection<ItineraryDay> ItineraryDays { get; set; }
    }
}
