using HotelManager.Domain.Entities;
using HotelManager.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace HotelManager.Data.Seeding;

public static class SeedDataExtensions
{
    public static void SeedData(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>().HasData(
            new { Id = 1, Name = "Admin" },
            new { Id = 2, Name = "Receptionist" },
            new { Id = 3, Name = "Manager" });

        modelBuilder.Entity<RoomType>().HasData(
            new { Id = 1, Name = "Single", BasePricePerNight = 75.00m, Capacity = 1 },
            new { Id = 2, Name = "Double", BasePricePerNight = 110.00m, Capacity = 2 },
            new { Id = 3, Name = "Twin", BasePricePerNight = 120.00m, Capacity = 2 },
            new { Id = 4, Name = "Apartment", BasePricePerNight = 180.00m, Capacity = 4 },
            new { Id = 5, Name = "Suite", BasePricePerNight = 260.00m, Capacity = 4 });

        modelBuilder.Entity<Room>().HasData(
            new { Id = 1, RoomNumber = "101", Floor = 1, RoomTypeId = 1, Status = RoomStatus.Available },
            new { Id = 2, RoomNumber = "102", Floor = 1, RoomTypeId = 2, Status = RoomStatus.Available },
            new { Id = 3, RoomNumber = "201", Floor = 2, RoomTypeId = 3, Status = RoomStatus.Available },
            new { Id = 4, RoomNumber = "301", Floor = 3, RoomTypeId = 4, Status = RoomStatus.Available },
            new { Id = 5, RoomNumber = "401", Floor = 4, RoomTypeId = 5, Status = RoomStatus.Available });

        modelBuilder.Entity<Service>().HasData(
            new { Id = 1, Name = "Breakfast", Price = 12.00m, Description = "Continental breakfast served in the restaurant." },
            new { Id = 2, Name = "Parking", Price = 8.00m, Description = "On-site parking, charged per night." },
            new { Id = 3, Name = "Transfer", Price = 35.00m, Description = "One-way airport transfer." },
            new { Id = 4, Name = "SPA", Price = 50.00m, Description = "Access to spa and wellness facilities." },
            new { Id = 5, Name = "Room Service", Price = 15.00m, Description = "In-room dining delivery." });
    }
}
