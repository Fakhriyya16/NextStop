
using AutoMapper;
using Domain.Entities;
using Repository.Repositories.Interfaces;
using Service.DTOs.Cities;
using Service.Helpers.Exceptions;
using Service.Helpers.Extensions;
using Service.Interfaces;

namespace Service
{
    public class CityService : ICityService
    {
        private readonly ICityRepository _cityRepository;
        private readonly IMapper _mapper;
        private readonly ICountryRepository _countryRepository;
        private readonly ICloudManagement _cloudManagement;

        public CityService(ICityRepository cityRepository,IMapper mapper,
                           ICountryRepository countryRepository, ICloudManagement cloudManagement)
        {
            _cityRepository = cityRepository;
            _mapper = mapper;
            _countryRepository = countryRepository;
            _cloudManagement = cloudManagement;
        }

        public async Task CreateAsync(CityCreateDto model)
        {
            var country = await _countryRepository.FindByName(model.CountryName);

            if (country is null)
            {
                throw new NotFoundException("Country was not found");
            }
            
            if (!model.Image.IsImage()) throw new InvalidImageFormatException("The file is not a valid image or is empty.");

            if(!model.Image.IsValidSize(500)) throw new FileSizeExceededException("The file size exceeds the maximum allowed limit.");

            var fileName = $"{model.Name}_{Guid.NewGuid()}";

            using (var imageStream = model.Image.OpenReadStream())
            {
                var result = await _cloudManagement.UploadImageWithPublicIdAsync(imageStream,fileName);

                City newCity = new City();

                newCity.CountryId = country.Id;
                newCity.PublicId = result.PublicId;
                newCity.ImageUrl = result.Url;

                _mapper.Map(model, newCity);

                await _cityRepository.CreateAsync(newCity);
            }
        }

        public async Task DeleteAsync(int? id)
        {
            if (id is null) throw new ArgumentNullException();

            var city = await _cityRepository.GetById((int)id);

            if (city is null) throw new NotFoundException("City was not found");

            await _cloudManagement.DeleteImageAsync(city.PublicId);

            await _cityRepository.DeleteAsync(city);
        }

        public async Task EditAsync(int? id, CityEditDto model)
        {
            if (id is null) throw new ArgumentNullException();

            var city = await _cityRepository.GetById((int)id);

            if (city is null) throw new NotFoundException("City was not found");

            if (await _countryRepository.FindByName(model.Country) is null)
            {
                throw new NotFoundException("Country was not found");
            }

            if (model.Image is not null)
            {
                if (!model.Image.IsImage()) throw new InvalidImageFormatException();
                if (!model.Image.IsValidSize(500)) throw new FileSizeExceededException();

                await _cloudManagement.DeleteImageAsync(city.PublicId);

                var fileName = $"{model.Name}_{Guid.NewGuid()}";

                using (var imageStream = model.Image.OpenReadStream())
                {
                    var result = await _cloudManagement.UploadImageWithPublicIdAsync(imageStream, fileName);

                    City updatedCity = _mapper.Map(model, city);

                    updatedCity.PublicId = result.PublicId;
                    updatedCity.ImageUrl = result.Url;

                    await _cityRepository.EditAsync(updatedCity);
                }
            }

            await _cityRepository.EditAsync(_mapper.Map(model, city));
        }

        public async Task<IEnumerable<CityDto>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<CityDto>>(await _cityRepository.GetAllWithIncludes(m => m.Places));
        }

        public async Task<CityDto> GetByIdAsync(int id)
        {
            return _mapper.Map<CityDto>(await _cityRepository.GetByIdWithIncludes(m=>m.Id == id,m => m.Places));
        }
    }
}
