using HotelManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelManager.Data.Configurations;

public class RoomConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.ToTable("Rooms");
        builder.HasKey(room => room.Id);

        builder.HasIndex(room => room.RoomNumber).IsUnique();

        builder.Property(room => room.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.HasMany(room => room.Reservations)
            .WithOne(reservation => reservation.Room)
            .HasForeignKey(reservation => reservation.RoomId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
