using Domain.Entities;

namespace Repository.Repositories.Interfaces
{
    public interface IPlaceRepository : IBaseRepository<Place>
    {
        Task<bool> IsExist(string name);
        Task<IEnumerable<Place>> GetPlacesByCategoryAndCity(Category category,City city);
        Task<IEnumerable<Place>> GetPlacesByCityForItinerary(City city);
        Task<IEnumerable<Place>> SortBy(string property, string order);
    }
}
