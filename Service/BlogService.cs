
using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Repository.Helpers;
using Repository.Repositories.Interfaces;
using Service.DTOs.Blogs;
using Service.Helpers;
using Service.Helpers.Exceptions;
using Service.Helpers.Extensions;
using Service.Interfaces;
using System.Reflection.Metadata;

namespace Service
{
    public class BlogService : IBlogService
    {
        private readonly IBlogRepository _blogRepository;
        private readonly IMapper _mapper;
        private readonly ICloudManagement _cloudManagement;
        private readonly IBlogImageRepository _blogImageRepository;

        public BlogService(IBlogRepository blogRepository, IMapper mapper, ICloudManagement cloudManagement,
                           IBlogImageRepository blogImageRepository)
        {
            _blogRepository = blogRepository;
            _mapper = mapper;
            _cloudManagement = cloudManagement;
            _blogImageRepository = blogImageRepository;
        }

        public async Task CreateAsync(BlogCreateDto model)
        {
            if (await _blogRepository.IsExistByTitle(model.Title)) throw new EntityExistsException("Blog");

            var blog = _mapper.Map<Blog>(model);

            foreach (var image in model.Images)
            {
                if (!image.IsImage()) throw new InvalidImageFormatException("The file is not a valid image or is empty.");

                if (!image.IsValidSize(1000)) throw new FileSizeExceededException("The file size exceeds the maximum allowed limit.");
            }

            await _blogRepository.CreateAsync(blog);

            foreach (var image in model.Images)
            {             
                var fileName = $"{model.Title}_{Guid.NewGuid()}";

                using (var imageStream = image.OpenReadStream())
                {
                    var result = await _cloudManagement.UploadImageWithPublicIdAsync(imageStream, fileName);

                    BlogImage blogImage = new()
                    {
                        BlogId = blog.Id,
                        ImageUrl = result.Url,
                        PublicId = result.PublicId,
                    };

                    await _blogImageRepository.CreateAsync(blogImage);
                }
            }
        }

        public async Task DeleteAsync(int? id)
        {
            if (id is null) throw new ArgumentNullException();

            var blog = await _blogRepository.GetById((int)id);

            if (blog is null) throw new NotFoundException("Blog");

            await _blogRepository.DeleteAsync(blog);
        }

        public async Task EditAsync(int? id, BlogEditDto model)
        {
            if (id is null) throw new ArgumentNullException();

            var blog = await _blogRepository.GetById((int)id);

            if (blog is null) throw new NotFoundException("Blog");

            if (model.NewImages is not null)
            {
                foreach (var image in model.NewImages)
                {
                    if (!image.IsImage()) throw new InvalidImageFormatException("The file is not a valid image or is empty.");

                    if (!image.IsValidSize(500)) throw new FileSizeExceededException("The file size exceeds the maximum allowed limit.");

                    var fileName = $"{model.Title}_{Guid.NewGuid()}";

                    using (var imageStream = image.OpenReadStream())
                    {
                        var result = await _cloudManagement.UploadImageWithPublicIdAsync(imageStream, fileName);

                        BlogImage placeImage = new()
                        {
                            BlogId = blog.Id,
                            ImageUrl = result.Url,
                            PublicId = result.PublicId,
                        };
                    }
                }

                foreach (var oldImage in blog.BlogImages)
                {
                    await _cloudManagement.DeleteImageAsync(oldImage.PublicId);
                }
            }

            await _blogRepository.EditAsync(_mapper.Map(model, blog));
        }

        public async Task<IEnumerable<BlogDto>> GetAllAsync()
        {
            var blogs = await _blogRepository.GetAllWithIncludes(m=>m.BlogImages,m=>m.AppUser);

            return _mapper.Map<IEnumerable<BlogDto>>(blogs);
        }

        public async Task<BlogDto> GetByIdAsync(int id)
        {
            var blog = await _blogRepository.GetByIdWithIncludes(m => m.Id == id, m => m.BlogImages,m=>m.AppUser);

            return _mapper.Map<BlogDto>(blog);
        }

        public async Task<PaginateResponse<BlogDto>> GetPaginatedBlogs(int currentPage, int pageSize)
        {
            var response = await _blogRepository.GetPagination(currentPage, pageSize);

            var result = new PaginateResponse<BlogDto>()
            {
                Data = _mapper.Map<List<BlogDto>>(response.Data),
            };

            return _mapper.Map(response,result);
        }
    }
}
