using AutoMapper;
using Domain.Entities;
using Repository.Repositories.Interfaces;
using Service.DTOs.Favorites;
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

        public async Task CreateAsync(Favorite favorite)
        {
            await _favoriteRepository.CreateAsync(favorite);
        }

        public async Task DeleteAsync(int? id)
        {
            if (id is null) throw new ArgumentNullException();

            var favorite = await _favoriteRepository.GetById((int)id);

            if (favorite is null) throw new NotFoundException("Favorite");

            await _favoriteRepository.DeleteAsync(favorite);
        }

        public async Task<IEnumerable<FavoriteDto>> GetAllAsync()
        {
            var favorites = await _favoriteRepository.GetAllWithIncludes(m => m.Place);

            return _mapper.Map<IEnumerable<FavoriteDto>>(favorites);
        }

        public async Task<Favorite> GetByIdAsync(int id)
        {
            var favorite = await _favoriteRepository.GetById((int)id);

            return favorite is null ? throw new NotFoundException("Favorite") : favorite;
        }
    }
}
