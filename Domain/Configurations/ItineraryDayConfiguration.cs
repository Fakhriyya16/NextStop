
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Configurations
{
    public class ItineraryDayConfiguration : IEntityTypeConfiguration<ItineraryDay>
    {
        public void Configure(EntityTypeBuilder<ItineraryDay> builder)
        {
            builder.Property(m=>m.ItineraryId).IsRequired();
            builder.Property(m=>m.DayNumber).IsRequired();
        }
    }
}
