
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Service.Interfaces;

namespace Service
{
    public class GeolocationService : IGeolocationService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public GeolocationService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient();
            _apiKey = configuration["PositionStack:ApiKey"];
        }

        public async Task<(double Latitude, double Longitude)> GetCoordinatesAsync(string placeName, string cityName, string countryName)
        {
            var query = $"{placeName}, {cityName}, {countryName}";
            var requestUri = $"http://api.positionstack.com/v1/forward?access_key={_apiKey}&query={Uri.EscapeDataString(query)}";

            var response = await _httpClient.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(jsonResponse);

            var data = json["data"]?.FirstOrDefault();
            if (data == null)
            {
                throw new Exception("Unable to retrieve geolocation data");
            }

            var latitude = data["latitude"].Value<double>();
            var longitude = data["longitude"].Value<double>();

            return (latitude, longitude);
        }
    }
}
