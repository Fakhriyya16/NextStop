using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Configurations
{
    public class SubscriptionPriceConfiguration : IEntityTypeConfiguration<SubscriptionPrice>
    {
        public void Configure(EntityTypeBuilder<SubscriptionPrice> builder)
        {
            builder.Property(m => m.MonthlyPrice).HasDefaultValue(0);
        }
    }
}
