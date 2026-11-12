using HotelManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelManager.Data.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles");
        builder.HasKey(role => role.Id);

        builder.HasIndex(role => role.Name).IsUnique();

        builder.HasMany(role => role.Users)
            .WithOne(user => user.Role)
            .HasForeignKey(user => user.RoleId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
