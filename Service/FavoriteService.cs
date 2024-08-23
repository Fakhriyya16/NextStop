using AutoMapper;
using Domain.Entities;
using Repository.Repositories;
using Repository.Repositories.Interfaces;
using Service.DTOs.Blogs;
using Service.DTOs.Favorites;
using Service.Helpers;
using Service.Helpers.Exceptions;
using Service.Interfaces;

namespace Service
{
    public class FavoriteService : IFavoriteService
    {
        private readonly IFavoriteRepository _favoriteRepository;
        private readonly IMapper _mapper;

        public FavoriteService(IFavoriteRepository favoriteRepository, IMapper mapper)
        {
            _favoriteRepository = favoriteRepository;
            _mapper = mapper;
        }

        public async Task CreateAsync(FavoriteCreateDto favorite)
        {
            var entity = _mapper.Map<Favorite>(favorite);
            await _favoriteRepository.CreateAsync(entity);
        }

        public async Task<bool> IsFavoriteAsync(string userId, int placeId)
        {
            var favorite = await _favoriteRepository.IsFavorite(f => f.AppUserId == userId && f.PlaceId == placeId);
            return favorite != null;
        }

        public async Task<Favorite> GetByUserAndPlaceId(string userId,int placeId)
        {
            return await _favoriteRepository.IsFavorite(f => f.AppUserId == userId && f.PlaceId == placeId);
        }

        public async Task DeleteAsync(string userId, int placeId)
        {
            var favorite = await GetByUserAndPlaceId(userId, placeId);
            if (favorite is null) throw new NotFoundException("Favorite");
            await _favoriteRepository.DeleteAsync(favorite);
        }

        public async Task<IEnumerable<FavoriteDto>> GetAllAsync()
        {
            var favorites = await _favoriteRepository.GetAllWithIncludes(m => m.Place);

            return _mapper.Map<IEnumerable<FavoriteDto>>(favorites);
        }

        public async Task<PaginateResponse<FavoriteDto>> GetAllPaginated(int currentPage, int pageSize)
        {
            var response = await _favoriteRepository.GetPagination(currentPage, pageSize);

            var result = new PaginateResponse<FavoriteDto>()
            {
                Data = _mapper.Map<List<FavoriteDto>>(response.Data),
            };

            return _mapper.Map(response, result);
        }

        public async Task<PaginateResponse<FavoriteDto>> GetAllPaginatedForUser(int currentPage, int pageSize,string userId)
        {
            var response = await _favoriteRepository.GetPaginationForUser(currentPage, pageSize,userId);

            var result = new PaginateResponse<FavoriteDto>()
            {
                Data = _mapper.Map<List<FavoriteDto>>(response.Data),
            };

            return _mapper.Map(response, result);
        }

        public async Task<Favorite> GetByIdAsync(int id)
        {
            var favorite = await _favoriteRepository.GetById((int)id);

            return favorite is null ? throw new NotFoundException("Favorite") : favorite;
        }
    }
}
