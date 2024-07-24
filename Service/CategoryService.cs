
using AutoMapper;
using Domain.Entities;
using Repository.Repositories.Interfaces;
using Service.DTOs.Categories;
using Service.Helpers.Exceptions;
using Service.Interfaces;

namespace Service
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task CreateAsync(CategoryCreateDto model)
        {
            if (await _categoryRepository.IsExist(model.Name))
            {
                throw new EntityExistsException("Category");
            }

            await _categoryRepository.CreateAsync(_mapper.Map<Category>(model));
        }

        public async Task DeleteAsync(int? id)
        {
            if (id is null) throw new ArgumentNullException();

            var category = await _categoryRepository.GetById((int)id);

            if (category is null) throw new NotFoundException("Category");

            await _categoryRepository.DeleteAsync(category);
        }

        public async Task EditAsync(int? id, CategoryEditDto model)
        {
            if (id is null) throw new ArgumentNullException();

            var category = await _categoryRepository.GetById((int)id);

            if (category is null) throw new NotFoundException("Category");

            await _categoryRepository.EditAsync(_mapper.Map(model, category));
        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<CategoryDto>>(await _categoryRepository.GetAllWithIncludes(m => m.Places));
        }

        public async Task<CategoryDto> GetByIdAsync(int id)
        {
            return _mapper.Map<CategoryDto>(await _categoryRepository.GetByIdWithIncludes(m => m.Id == id, m => m.Places));
        }
    }
}
