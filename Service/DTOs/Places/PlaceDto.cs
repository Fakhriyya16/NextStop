
using Domain.Entities;
using Service.DTOs.Reviews;

namespace Service.DTOs.Places
{
    public class PlaceDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public List<string> Images { get; set; }
        public List<ReviewDto>? Reviews { get; set; }
        public List<string> Tags { get; set; }
        public int? Rating { get; set; }
    }
}
