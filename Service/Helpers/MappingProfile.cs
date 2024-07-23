
using AutoMapper;
using Domain.Entities;
using Service.DTOs.Cities;
using Service.DTOs.Countries;

namespace Service.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Country, CountryDto>().ForMember(dest => dest.Cities, opt => opt.MapFrom(m => m.Cities.Select(m => m.Name).ToList()));
            CreateMap<CountryCreateDto, Country>();
            CreateMap<CountryEditDto, Country>();

            CreateMap<City, CityDto>().ForMember(dest => dest.Country, opt => opt.MapFrom(m => m.Country.Name));
            CreateMap<CityCreateDto, City>();
            CreateMap<CityEditDto, City>();
        }
    }
}
