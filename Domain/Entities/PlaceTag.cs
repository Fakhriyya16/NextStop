
using Domain.Common;

namespace Domain.Entities
{
    public class PlaceTag : BaseEntity
    {
        public int PlaceId { get; set; }
        public Place Place { get; set; }

        public int TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
