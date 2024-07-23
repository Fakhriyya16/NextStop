
using Domain.Common;

namespace Domain.Entities
{
    public class BlogImage : BaseEntity
    {
        public int BlogId { get; set; }
        public Blog Blog { get; set; }
        public string PublicId { get; set; }
        public string ImageUrl { get; set; }
    }
}
