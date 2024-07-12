using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Configurations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.Property(m => m.AppUserId).IsRequired();
            builder.Property(m => m.StripePaymentId).IsRequired();
            builder.Property(m => m.Amount).IsRequired();
            builder.Property(m => m.Currency).IsRequired();
            builder.Property(m => m.Status).IsRequired();
        }
    }
}
