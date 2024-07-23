
using FluentValidation;

namespace Service.DTOs.Countries
{
    public class CountryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> Cities { get; set; }
    }
}
