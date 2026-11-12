using HotelManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelManager.Data.Configurations;

public class RoomTypeConfiguration : IEntityTypeConfiguration<RoomType>
{
    public void Configure(EntityTypeBuilder<RoomType> builder)
    {
        builder.ToTable("RoomTypes");
        builder.HasKey(roomType => roomType.Id);

        builder.HasIndex(roomType => roomType.Name).IsUnique();

        builder.Property(roomType => roomType.BasePricePerNight)
            .HasPrecision(18, 2);

        builder.HasMany(roomType => roomType.Rooms)
            .WithOne(room => room.RoomType)
            .HasForeignKey(room => room.RoomTypeId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
