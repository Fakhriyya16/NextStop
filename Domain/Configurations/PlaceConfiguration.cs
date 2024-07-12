
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Configurations
{
    public class PlaceConfiguration : IEntityTypeConfiguration<Place>
    {
        public void Configure(EntityTypeBuilder<Place> builder)
        {
            builder.Property(m => m.Name).IsRequired();
            builder.Property(m => m.CityId).IsRequired();
            builder.Property(m => m.CategoryId).IsRequired();
            builder.Property(m => m.Description).IsRequired();
            builder.Property(m => m.Latitude).IsRequired();
            builder.Property(m => m.Longitude).IsRequired();
        }
    }
}
