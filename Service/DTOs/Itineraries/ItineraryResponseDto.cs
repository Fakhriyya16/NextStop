
namespace Service.DTOs.Itineraries
{
    public class ItineraryResponseDto
    {
        public string Name { get; set; }
        public List<ItineraryDayDto> ItineraryDays { get; set; }
    }
}
