
using FluentValidation;

namespace Service.DTOs.Itineraries
{
    public class ItineraryRequestDto
    {
        public string City { get; set; }
        public int NumberOfDays { get; set; }
        public List<int> Categories { get; set; }
    }

    public class ItineraryRequestDtoValidator : AbstractValidator<ItineraryRequestDto>
    {
        public ItineraryRequestDtoValidator()
        {
            RuleFor(m=>m.City).NotEmpty();
            RuleFor(m => m.NumberOfDays).GreaterThanOrEqualTo(1);
        }
    }
}
