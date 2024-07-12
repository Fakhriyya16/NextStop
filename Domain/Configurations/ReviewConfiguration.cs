
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Configurations
{
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.Property(m => m.AppUserId).IsRequired();
            builder.Property(m => m.PlaceId).IsRequired();
            builder.Property(m => m.Rating).IsRequired();
            builder.Property(m => m.Comment).IsRequired();
        }
    }
}
