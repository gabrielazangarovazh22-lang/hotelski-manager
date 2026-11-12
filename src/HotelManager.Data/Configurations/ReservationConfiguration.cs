using HotelManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelManager.Data.Configurations;

public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Reservation> builder)
    {
        builder.ToTable("Reservations");
        builder.HasKey(reservation => reservation.Id);

        builder.Property(reservation => reservation.TotalPrice)
            .HasPrecision(18, 2);

        builder.Property(reservation => reservation.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.HasMany(reservation => reservation.ReservationServices)
            .WithOne(reservationService => reservationService.Reservation)
            .HasForeignKey(reservationService => reservationService.ReservationId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(reservation => reservation.Payments)
            .WithOne(payment => payment.Reservation)
            .HasForeignKey(payment => payment.ReservationId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
