
using Domain.Common;

namespace Domain.Entities
{
    public class ItineraryPlace : BaseEntity
    {
        public int ItineraryDayId { get; set; }
        public ItineraryDay ItineraryDay { get; set; }

        public int PlaceId { get; set; }
        public Place Place { get; set; }
    }
}
