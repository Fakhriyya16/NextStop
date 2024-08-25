
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

        public async Task<IEnumerable<CategoryNameDto>> GetAllNamesAsync()
        {
            try
            {
                var categories = await _categoryRepository.GetAllAsync();
                return _mapper.Map<IEnumerable<CategoryNameDto>>(categories);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving categories: {ex.Message}");
            }
        }

        public async Task CreateAsync(CategoryCreateDto model)
        {
            try
            {
                if (await _categoryRepository.IsExist(model.Name))
                {
                    throw new EntityExistsException("Category");
                }

                var category = _mapper.Map<Category>(model);
                await _categoryRepository.CreateAsync(category);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while creating the category: {ex.Message}");
            }
        }

        public async Task DeleteAsync(int? id)
        {
            try
            {
                if (id is null)
                {
                    throw new ArgumentNullException(nameof(id), "Category ID cannot be null.");
                }

                var category = await _categoryRepository.GetById((int)id);

                if (category is null)
                {
                    throw new NotFoundException("Category");
                }

                await _categoryRepository.DeleteAsync(category);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting the category: {ex.Message}");
            }
        }

        public async Task EditAsync(int? id, CategoryEditDto model)
        {
            try
            {
                if (id is null)
                {
                    throw new ArgumentNullException(nameof(id), "Category ID cannot be null.");
                }

                var category = await _categoryRepository.GetById((int)id);

                if (category is null)
                {
                    throw new NotFoundException("Category");
                }

                _mapper.Map(model, category);
                await _categoryRepository.EditAsync(category);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while editing the category: {ex.Message}");
            }
        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            try
            {
                var categories = await _categoryRepository.GetAllWithIncludes(m => m.Places);
                return _mapper.Map<IEnumerable<CategoryDto>>(categories);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving categories: {ex.Message}");
            }
        }

        public async Task<CategoryDto> GetByIdAsync(int id)
        {
            try
            {
                var category = await _categoryRepository.GetByIdWithIncludes(m => m.Id == id, m => m.Places);

                return category == null ? throw new NotFoundException("Category") : _mapper.Map<CategoryDto>(category);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving the category: {ex.Message}");
            }
        }
    }
}
