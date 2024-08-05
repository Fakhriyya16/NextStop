
namespace Service.DTOs.Reviews
{
    public class ReviewDto
    {
        public string User { get; set; }
        public string Place { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public string ReviewDate { get; set; }
    }
}
