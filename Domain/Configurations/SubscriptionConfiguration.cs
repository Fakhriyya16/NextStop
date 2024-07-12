
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Configurations
{
    public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
    {
        public void Configure(EntityTypeBuilder<Subscription> builder)
        {
            builder.Property(m => m.AppUserId).IsRequired();
            builder.Property(m => m.StartDate).IsRequired();
            builder.Property(m => m.EndDate).IsRequired();
            builder.Property(m => m.IsActive).IsRequired();
            builder.Property(m => m.SubscriptionType).IsRequired();

            builder.HasOne(s => s.AppUser)
                   .WithOne(u => u.Subscription)
                   .HasForeignKey<Subscription>(s => s.AppUserId);
        }
    }
}
