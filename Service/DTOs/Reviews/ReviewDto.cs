
namespace Service.DTOs.Reviews
{
    public class ReviewDto
    {
        public int Id { get; set; }
        public string User { get; set; }
        public string AppUserId { get; set; }   
        public string Place { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public string ReviewDate { get; set; }
    }
}
