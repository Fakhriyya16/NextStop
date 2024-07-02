
using Domain.Common;

namespace Domain.Entities
{
    public class Tag : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<BlogTag> BlogTags { get; set; }
        public ICollection<PlaceTag> PlaceTags { get; set; }
    }
}
