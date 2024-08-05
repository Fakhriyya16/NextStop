
using Service.DTOs.Itineraries;

namespace Service.Interfaces
{
    public interface IItineraryService
    {
        Task<ItineraryResponseDto> GenerateItinerary(ItineraryRequestDto model, string userId);
    }
}
