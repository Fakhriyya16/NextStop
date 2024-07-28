
using Domain.Entities;

namespace Service.DTOs.Cities
{
    public class CityDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string PublicId { get; set; }
        public List<string> Places { get; set; }
    }
}
