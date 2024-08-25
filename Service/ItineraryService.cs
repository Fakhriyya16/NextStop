
using Accord.MachineLearning;
using AutoMapper;
using Domain.Entities;
using Repository.Repositories.Interfaces;
using Service.DTOs.Itineraries;
using Service.Helpers.Exceptions;
using Service.Interfaces;

namespace Service
{
    public class ItineraryService : IItineraryService
    {
        private readonly IItineraryRepository _itineraryRepository;
        private readonly IItineraryDayRepository _itineraryDayRepository;
        private readonly IItineraryPlaceRepository _itineraryPlaceRepository;
        private readonly IMapper _mapper;
        private readonly ICityRepository _cityRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IPlaceRepository _placeRepository;

        public ItineraryService(IItineraryRepository itineraryRepository, IItineraryDayRepository itineraryDayRepository,
                                IItineraryPlaceRepository itineraryPlaceRepository, IMapper mapper,
                                ICityRepository cityRepository, ICategoryRepository categoryRepository,
                                IPlaceRepository placeRepository)
        {
            _itineraryRepository = itineraryRepository;
            _itineraryDayRepository = itineraryDayRepository;
            _itineraryPlaceRepository = itineraryPlaceRepository;
            _mapper = mapper;
            _cityRepository = cityRepository;
            _categoryRepository = categoryRepository;
            _placeRepository = placeRepository;
        }

        public async Task<ItineraryResponseDto> GenerateItinerary(ItineraryRequestDto model, string userId)
        {
            var city = await _cityRepository.GetById(model.CityId) ?? throw new NotFoundException("City");
            List<Category> categories = new();
            foreach (var categoryId in model.Categories)
            {
                categories.Add(await _categoryRepository.GetById(categoryId));
            }

            List<Place> places = new();
            if (categories.Count > 0)
            {
                foreach (var category in categories)
                {
                    places.AddRange(await _placeRepository.GetPlacesByCategoryAndCity(category, city));
                }
            }
            else
            {
                places.AddRange(await _placeRepository.GetPlacesByCityForItinerary(city));
            }

            int numberOfClusters = Math.Min(model.NumberOfDays, places.Count);
            var clusteredPlaces = KMeansClustering(places, numberOfClusters);

            List<ItineraryDayDto> itineraryDays = new();
            for (int i = 0; i < model.NumberOfDays; i++)
            {
                var dayPlaces = new List<Place>();

                for (int j = i; j < clusteredPlaces.Count; j += model.NumberOfDays)
                {
                    dayPlaces.AddRange(clusteredPlaces[j]);
                }

                itineraryDays.Add(new ItineraryDayDto
                {
                    DayNumber = i + 1,
                    ItineraryPlaces = dayPlaces.Select(p => new ItineraryPlaceDto
                    {
                        PlaceId = p.Id,
                        PlaceName = p.Name,
                        Category = p.Category?.Name,
                        Latitude = p.Latitude,
                        Longitude = p.Longitude
                    }).ToList()
                });
            }

            var itinerary = new Itinerary
            {
                AppUserId = userId,
                Name = $"Itinerary for {city.Name} - {model.NumberOfDays} days",
                ItineraryDays = itineraryDays.Select(day => new ItineraryDay
                {
                    DayNumber = day.DayNumber,
                    ItineraryPlaces = day.ItineraryPlaces.Select(m => new ItineraryPlace
                    {
                        PlaceId = m.PlaceId,
                    }).ToList()
                }).ToList()
            };

            await _itineraryRepository.CreateAsync(itinerary);

            return new ItineraryResponseDto
            {
                Id = itinerary.Id,
                Name = itinerary.Name,
                ItineraryDays = itineraryDays
            };
        }

        public List<List<Place>> KMeansClustering(List<Place> places, int numberOfClusters)
        {
            var observations = places.Select(p => new double[] { p.Latitude, p.Longitude }).ToArray();

            var kmeans = new KMeans(numberOfClusters);
            var clusters = kmeans.Learn(observations);
            var labels = clusters.Decide(observations);

            var groupedPlaces = new List<List<Place>>();
            for (int i = 0; i < numberOfClusters; i++)
            {
                groupedPlaces.Add(new List<Place>());
            }

            for (int i = 0; i < labels.Length; i++)
            {
                groupedPlaces[labels[i]].Add(places[i]);
            }

            return groupedPlaces;
        }

        public async Task<ItineraryResponseDto> GetById(int id)
        {
            var plan = await _itineraryRepository.GetByIdWithIncludes(
                m => m.Id == id,
                m => m.ItineraryDays,
                m => m.AppUser);

            var itineraryDays = await _itineraryDayRepository.GetAllByItineraryId(id);

            var result = _mapper.Map<ItineraryResponseDto>(plan);

            result.ItineraryDays = _mapper.Map<List<ItineraryDayDto>>(itineraryDays);

            return result;
        }
    }
}

