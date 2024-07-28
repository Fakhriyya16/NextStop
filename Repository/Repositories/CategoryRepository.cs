
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Repositories.Interfaces;
using System.Xml.Linq;

namespace Repository.Repositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<bool> IsExist(string name)
        {
            return await _entities.AnyAsync(e => e.Name == name);
        }

        public async Task<bool> IsExistById(int id)
        {
            return await _entities.AnyAsync(e => e.Id == id);
        }
    }
}
