using HotelManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelManager.Data.Configurations;

public class ServiceConfiguration : IEntityTypeConfiguration<Service>
{
    public void Configure(EntityTypeBuilder<Service> builder)
    {
        builder.ToTable("Services");
        builder.HasKey(service => service.Id);

        builder.HasIndex(service => service.Name).IsUnique();

        builder.Property(service => service.Price)
            .HasPrecision(18, 2);

        builder.HasMany(service => service.ReservationServices)
            .WithOne(reservationService => reservationService.Service)
            .HasForeignKey(reservationService => reservationService.ServiceId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
