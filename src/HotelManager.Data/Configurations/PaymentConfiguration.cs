using HotelManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelManager.Data.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("Payments");
        builder.HasKey(payment => payment.Id);

        builder.Property(payment => payment.Amount)
            .HasPrecision(18, 2);

        builder.Property(payment => payment.PaymentMethod)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();
    }
}
