using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineStore.Core.Entities;

namespace OnlineStore.Infrastructure.Data.Configurations;

public class PromoCodeConfiguration : IEntityTypeConfiguration<PromoCode>
{
    public void Configure(EntityTypeBuilder<PromoCode> builder)
    {
        builder.HasKey(pc => pc.Id);

        builder.Property(pc => pc.Code)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(pc => pc.DiscountPercentage)
            .HasColumnType("decimal(5,2)")
            .IsRequired();

        builder.HasIndex(pc => pc.Code)
            .IsUnique();
    }
}




