﻿
using AutoMapper;
using Domain.Entities;
using Repository.Repositories.Interfaces;
using Service.DTOs.Places;
using Service.Helpers.Exceptions;
using Service.Helpers.Extensions;
using Service.Interfaces;

namespace Service
{
    public class PlaceService : IPlaceService
    {
        private readonly IPlaceRepository _placeRepository;
        private readonly IMapper _mapper;
        private readonly ICityRepository _cityRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ITagRepository _tagRepository;
        private readonly ICloudManagement _cloudManagement;
        private readonly IGeolocationService _geolocationService;
        private readonly IPlaceTagRepository _placeTagRepository;
        private readonly IPlaceImageRepository _placeImageRepository;

        public PlaceService(IPlaceRepository placeRepository, IMapper mapper, 
                            ICityRepository cityRepository, ICategoryRepository categoryRepository, 
                            ITagRepository tagRepository, ICloudManagement cloudManagement,
                            IGeolocationService geolocationService, IPlaceTagRepository placeTagRepository,
                            IPlaceImageRepository placeImageRepository)
        {
            _placeRepository = placeRepository;
            _mapper = mapper;
            _cityRepository = cityRepository;
            _categoryRepository = categoryRepository;
            _tagRepository = tagRepository;
            _cloudManagement = cloudManagement;
            _geolocationService = geolocationService;
            _placeTagRepository = placeTagRepository;
            _placeImageRepository = placeImageRepository;
        }

        public async Task CreateAsync(PlaceCreateDto model)
        {
            if (await _placeRepository.IsExist(model.Name)) throw new EntityExistsException("Place");

            var city = await _cityRepository.GetByIdWithIncludes(m=>m.Id == model.CityId,m=>m.Country) ?? throw new NotFoundException("City");

            var countryName = await _cityRepository.GetCountryNameByCityId(city.Id);

            if (!await _categoryRepository.IsExistById(model.CategoryId)) throw new NotFoundException("Category");

            Place newPlace = _mapper.Map<Place>(model);

            var coordinates = await _geolocationService.GetCoordinatesAsync(newPlace.Name, city.Name, countryName);
            newPlace.Longitude = coordinates.Longitude;
            newPlace.Latitude = coordinates.Latitude;

            foreach (var image in model.Images)
            {
                if (!image.IsImage()) throw new InvalidImageFormatException("The file is not a valid image or is empty.");

                if (!image.IsValidSize(500)) throw new FileSizeExceededException("The file size exceeds the maximum allowed limit.");
            }

            await _placeRepository.CreateAsync(newPlace);

            foreach (var image in model.Images)
            {           
                var fileName = $"{model.Name}_{Guid.NewGuid()}";

                using (var imageStream = image.OpenReadStream())
                {
                    var result = await _cloudManagement.UploadImageWithPublicIdAsync(imageStream, fileName);

                    PlaceImage placeImage = new()
                    {
                        PlaceId = newPlace.Id,
                        ImageUrl = result.Url,
                        PublicId = result.PublicId,
                    };

                    await _placeImageRepository.CreateAsync(placeImage);
                }
            }

            foreach (var tagId in model.TagIds)
            {
                var tag = await _tagRepository.GetById(tagId);
                if (tag is not null)
                {
                    PlaceTag placeTag = new()
                    {
                        TagId = tagId,
                        PlaceId = newPlace.Id
                    };

                    await _placeTagRepository.CreateAsync(placeTag);
                };
            }

            await _placeRepository.EditAsync(newPlace);
        }

        public async Task DeleteAsync(int? id)
        {
            if (id is null) throw new ArgumentNullException();

            var place = await _placeRepository.GetById((int)id);

            if (place is null) throw new NotFoundException("Place");

            await _placeRepository.DeleteAsync(place);
        }

        public async Task EditAsync(int? id, PlaceEditDto model)
        {
            if (id is null) throw new ArgumentNullException();

            var place = await _placeRepository.GetByIdWithIncludes(m=>m.Id == id,m=>m.PlaceTags);

            if (place is null) throw new NotFoundException("Place");

            if (await _placeRepository.IsExist(model.Name)) throw new EntityExistsException("Place");

            if (!await _categoryRepository.IsExistById(model.CategoryId)) throw new NotFoundException("Category");

            if (model.NewImages is not null)
            {
                foreach (var image in model.NewImages)
                {
                    if (!image.IsImage()) throw new InvalidImageFormatException("The file is not a valid image or is empty.");

                    if (!image.IsValidSize(500)) throw new FileSizeExceededException("The file size exceeds the maximum allowed limit.");

                    var fileName = $"{model.Name}_{Guid.NewGuid()}";

                    using (var imageStream = image.OpenReadStream())
                    {
                        var result = await _cloudManagement.UploadImageWithPublicIdAsync(imageStream, fileName);

                        PlaceImage placeImage = new()
                        {
                            PlaceId = place.Id,
                            ImageUrl = result.Url,
                            PublicId = result.PublicId,
                        };
                    }
                }

                foreach (var oldImage in place.Images)
                {
                    await _cloudManagement.DeleteImageAsync(oldImage.PublicId);
                }
            }

            foreach (var tag in place.PlaceTags)
            {
                await _placeTagRepository.DeleteAsync(tag);
            }

            foreach (var tagId in model.TagIds)
            {
                var tag = await _tagRepository.GetById(tagId);
                if (tag is not null)
                {
                    PlaceTag placeTag = new()
                    {
                        TagId = tagId,
                        PlaceId = place.Id
                    };
                };
            }

            await _placeRepository.EditAsync(place);
        }

        public async Task<IEnumerable<PlaceDto>> GetAllAsync()
        {
            var places = await _placeRepository.GetAllWithIncludes(m => m.PlaceTags, m => m.Category,
                                                                   m => m.City.Country, m => m.Reviews,
                                                                   m => m.Images);

            return _mapper.Map<IEnumerable<PlaceDto>>(places);
        }

        public async Task<PlaceDto> GetByIdAsync(int id)
        {
            var place = await _placeRepository.GetByIdWithIncludes(m=>m.Id == id,m => m.PlaceTags, m => m.Category,
                                                       m => m.City, m => m.Reviews,
                                                       m => m.Images);

            return _mapper.Map<PlaceDto>(place);
        }
    }
}
