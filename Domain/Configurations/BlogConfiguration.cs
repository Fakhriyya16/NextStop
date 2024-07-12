
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Configurations
{
    public class BlogConfiguration : IEntityTypeConfiguration<Blog>
    {
        public void Configure(EntityTypeBuilder<Blog> builder)
        {
            builder.Property(m => m.Title).IsRequired();
            builder.Property(m => m.Content).IsRequired();
            builder.Property(m => m.PublishDate).IsRequired();
            builder.Property(m => m.AppUserId).IsRequired();
        }
    }
}
