
using Domain.Common;

namespace Domain.Entities
{
    public class PlaceImage : BaseEntity
    {
        public int AttractionId { get; set; }
        public Place Attraction { get; set; }
        public string ImageUrl { get; set; }
    }
}
