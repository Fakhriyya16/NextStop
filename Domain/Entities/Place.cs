
using Domain.Common;

namespace Domain.Entities
{
    public class Place : BaseEntity
    {
        public string Name { get; set; }
        public int CityId { get; set; }
        public City City { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public string Description { get; set; }
        public ICollection<PlaceImage> Images { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public int ItineraryDayId { get; set; }
        public ItineraryDay ItineraryDay { get; set; }
        public ICollection<PlaceTag> PlaceTags { get; set; }
    }
}
