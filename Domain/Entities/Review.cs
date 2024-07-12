
using Domain.Common;

namespace Domain.Entities
{
    public class Review : BaseEntity
    {
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public int PlaceId { get; set; }
        public Place Place { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime ReviewDate { get; set; } = DateTime.Now;
    }
}
