
namespace Service.DTOs.Itineraries
{
    public class ItineraryDayDto
    {
        public int DayNumber { get; set; }
        public List<ItineraryPlaceDto> ItineraryPlaces { get; set; }
    }
}
