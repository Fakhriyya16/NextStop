
namespace Service.Interfaces
{
    public interface IGeolocaitonSerivce
    {
        Task<(double Latitude, double Longitude)> GetCoordinatesAsync(string placeName, string cityName, string countryName);
    }
}
