
using Domain.Entities;

namespace Service.DTOs.Tags
{
    public class TagDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Place> Places { get; set; }
        public ICollection<Blog> Blogs { get; set; }
    }
}
