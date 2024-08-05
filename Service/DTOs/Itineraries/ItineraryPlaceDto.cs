
namespace Service.DTOs.Itineraries
{
    public class ItineraryPlaceDto
    {
        public int PlaceId { get; set; }
        public string PlaceName { get; set; }
        public string Category { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
