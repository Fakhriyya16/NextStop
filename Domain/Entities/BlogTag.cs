
using Domain.Common;

namespace Domain.Entities
{
    public class BlogTag : BaseEntity
    {
        public int BlogId { get; set; }
        public Blog Blog { get; set; }

        public int TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
