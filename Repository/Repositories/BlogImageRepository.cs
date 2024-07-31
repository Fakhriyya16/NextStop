
using Domain.Entities;
using Repository.Data;
using Repository.Repositories.Interfaces;

namespace Repository.Repositories
{
    public class BlogImageRepository : BaseRepository<BlogImage>, IBlogImageRepository
    {
        public BlogImageRepository(AppDbContext context) : base(context)
        {
        }
    }
}
