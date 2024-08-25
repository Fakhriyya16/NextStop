
using AutoMapper;
using Domain.Entities;
using Repository.Repositories.Interfaces;
using Service.DTOs.Countries;
using Service.Helpers.Exceptions;
using Service.Interfaces;

namespace Service
{
    public class CountryService : ICountryService
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;
        private readonly ICityRepository _cityRepository;

        public CountryService(ICountryRepository countryRepository, IMapper mapper, ICityRepository cityRepository)
        {
            _countryRepository = countryRepository;
            _mapper = mapper;
            _cityRepository = cityRepository;
        }

        public async Task CreateAsync(CountryCreateDto model)
        {
            if (await _countryRepository.FindByName(model.Name) is not null) throw new EntityExistsException("Country");

            await _countryRepository.CreateAsync(_mapper.Map<Country>(model));
        }

        public async Task DeleteAsync(int? id)
        {
            if (id is null) throw new ArgumentNullException();

            var country = await _countryRepository.GetByIdWithIncludes(m=>m.Id == id,m=>m.Cities);

            foreach (var city in country.Cities)
            {
                await _cityRepository.DeleteAsync(city);
            }

            if (country is null) throw new NotFoundException("Country was not found");

            await _countryRepository.DeleteAsync(country);
        }

        public async Task EditAsync(int? id, CountryEditDto model)
        {
            if (id is null) throw new ArgumentNullException();

            var country = await _countryRepository.GetById((int)id);

            if (country is null) throw new NotFoundException("Country was not found");

            if (await _countryRepository.FindByName(model.Name) is not null) throw new EntityExistsException("Country");

            _mapper.Map(model,country);

            await _countryRepository.EditAsync(country);
        }

        public async Task<IEnumerable<CountryDto>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<CountryDto>>(await _countryRepository.GetAllWithIncludes(m=>m.Cities));
        }

        public async Task<CountryDto> GetByIdAsync(int id)
        {
            return _mapper.Map<CountryDto>(await _countryRepository.GetByIdWithIncludes(m => m.Id == id, m => m.Cities));
        }
    }
}
