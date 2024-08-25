
using AutoMapper;
using Domain.Entities;
using Repository.Repositories.Interfaces;
using Service.DTOs.Tags;
using Service.Helpers.Exceptions;
using Service.Interfaces;

namespace Service
{
    public class TagService : ITagService
    {
        private readonly ITagRepository _tagRepository;
        private readonly IMapper _mapper;

        public TagService(ITagRepository tagRepository, IMapper mapper)
        {
            _tagRepository = tagRepository;
            _mapper = mapper;
        }

        public async Task CreateAsync(TagCreateDto model)
        {
            if (await _tagRepository.IsExist(model.Name)) throw new EntityExistsException("Tag");

            await _tagRepository.CreateAsync(_mapper.Map<Tag>(model));
        }

        public async Task DeleteAsync(int? id)
        {
            if (id is null) throw new ArgumentNullException();

            var tag = await _tagRepository.GetById((int)id);

            if (tag is null) throw new NotFoundException("Tag");

            await _tagRepository.DeleteAsync(tag);
        }

        public async Task EditAsync(int? id, TagEditDto model)
        {
            if (await _tagRepository.IsExist(model.Name)) throw new EntityExistsException("Tag");

            if (id is null) throw new ArgumentNullException();

            var tag = await _tagRepository.GetById((int)id);

            if (tag is null) throw new NotFoundException("Tag");

            await _tagRepository.EditAsync(_mapper.Map(model, tag));
        }

        public async Task<IEnumerable<TagDto>> GetAllAsync()
        {
            var tags = await _tagRepository.GetAllWithIncludes(
                m => m.PlaceTags
            );

            return _mapper.Map<IEnumerable<TagDto>>(tags);
        }

        public async Task<IEnumerable<TagNameDto>> GetAllNames()
        {
            var tags = await _tagRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<TagNameDto>>(tags);
        }

        public async Task<TagDto> GetByIdAsync(int id)
        {
            return _mapper.Map<TagDto>(await _tagRepository.GetByIdWithIncludes(m => m.Id == id, m => m.PlaceTags));
        }
    }
}
