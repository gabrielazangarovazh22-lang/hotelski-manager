using HotelManager.Data.Seeding;
using HotelManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HotelManager.Data;

public class HotelDbContext : DbContext
{
    public HotelDbContext(DbContextOptions<HotelDbContext> options)
        : base(options)
    {
    }

    public DbSet<Role> Roles => Set<Role>();

    public DbSet<User> Users => Set<User>();

    public DbSet<RoomType> RoomTypes => Set<RoomType>();

    public DbSet<Room> Rooms => Set<Room>();

    public DbSet<Guest> Guests => Set<Guest>();

    public DbSet<Reservation> Reservations => Set<Reservation>();

    public DbSet<Service> Services => Set<Service>();

    public DbSet<ReservationService> ReservationServices => Set<ReservationService>();

    public DbSet<Payment> Payments => Set<Payment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(HotelDbContext).Assembly);
        modelBuilder.SeedData();
    }
}
