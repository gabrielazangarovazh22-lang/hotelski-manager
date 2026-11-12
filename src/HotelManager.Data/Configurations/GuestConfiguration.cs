using HotelManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelManager.Data.Configurations;

public class GuestConfiguration : IEntityTypeConfiguration<Guest>
{
    public void Configure(EntityTypeBuilder<Guest> builder)
    {
        builder.ToTable("Guests");
        builder.HasKey(guest => guest.Id);

        builder.HasIndex(guest => guest.IdentityDocumentNumber).IsUnique();

        builder.HasMany(guest => guest.Reservations)
            .WithOne(reservation => reservation.Guest)
            .HasForeignKey(reservation => reservation.GuestId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
