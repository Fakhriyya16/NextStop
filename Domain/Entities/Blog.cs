
using Domain.Common;

namespace Domain.Entities
{
    public class Blog : BaseEntity
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime PublishDate { get; set; } = DateTime.Now;
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public ICollection<BlogTag> BlogTags { get; set; }
    }
}
