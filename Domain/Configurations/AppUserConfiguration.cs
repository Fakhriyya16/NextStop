
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Configurations
{
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.HasOne(u => u.Subscription)
                   .WithOne(s => s.AppUser)
                   .HasForeignKey<Subscription>(s => s.AppUserId);

            builder.Property(m=>m.Name).IsRequired();
            builder.Property(m=>m.Surname).IsRequired();
        }
    }
}
