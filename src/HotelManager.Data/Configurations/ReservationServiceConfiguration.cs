using HotelManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelManager.Data.Configurations;

public class ReservationServiceConfiguration : IEntityTypeConfiguration<ReservationService>
{
    public void Configure(EntityTypeBuilder<ReservationService> builder)
    {
        builder.ToTable("ReservationServices");
        builder.HasKey(reservationService => reservationService.Id);

        builder.HasIndex(reservationService => new
            {
                reservationService.ReservationId,
                reservationService.ServiceId
            })
            .IsUnique();

        builder.Property(reservationService => reservationService.AppliedPrice)
            .HasPrecision(18, 2);
    }
}
