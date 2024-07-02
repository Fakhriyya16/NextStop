
using Domain.Common;

namespace Domain.Entities
{
    public class ItineraryDay : BaseEntity
    {
        public int ItineraryId { get; set; }
        public Itinerary Itinerary { get; set; }
        public int Days { get; set; }
        public ICollection<Place> Places { get; set; }
    }
}
