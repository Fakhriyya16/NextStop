
namespace Service.DTOs.Blogs
{
    public class BlogDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string PublishDate { get; set; }
        public string Author { get; set; }
        public List<string> Images { get; set; }
    }
}
