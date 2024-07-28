
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
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public int? ItineraryDayId { get; set; }
        public ItineraryDay ItineraryDay { get; set; }
        public ICollection<PlaceTag> PlaceTags { get; set; }
        public ICollection<Favorite> Favorites { get; set; }
    }
}
