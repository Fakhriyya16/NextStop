
using Domain.Common;

namespace Domain.Entities
{
    public class PlaceImage : BaseEntity
    {
        public int PlaceId { get; set; }
        public Place Place { get; set; }
        public string ImageUrl { get; set; }
        public string PublicId { get; set; }
    }
}
