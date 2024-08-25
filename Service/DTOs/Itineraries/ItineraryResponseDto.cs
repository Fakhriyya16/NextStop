
namespace Service.DTOs.Itineraries
{
    public class ItineraryResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<ItineraryDayDto> ItineraryDays { get; set; }
    }
}
