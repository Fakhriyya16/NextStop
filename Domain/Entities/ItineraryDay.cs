
using Domain.Common;

namespace Domain.Entities
{
    public class ItineraryDay : BaseEntity
    {
        public int ItineraryId { get; set; }
        public Itinerary Itinerary { get; set; }
        public int DayNumber { get; set; }
        public ICollection<ItineraryPlace> ItineraryPlaces { get; set; }
    }
}
