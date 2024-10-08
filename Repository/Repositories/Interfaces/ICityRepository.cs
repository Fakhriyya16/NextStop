﻿
using Domain.Entities;

namespace Repository.Repositories.Interfaces
{
    public interface ICityRepository : IBaseRepository<City>
    {
        Task<bool> IsExist(string name);
        Task<string> GetCountryNameByCityId(int cityId);
        Task<City> GetCityByName(string name);
    }
}
