
using Domain.Common;

namespace Domain.Entities
{
    public class Favorite : BaseEntity
    {
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public int PlaceId { get; set; }    
        public Place Place { get; set; }
    }
}
