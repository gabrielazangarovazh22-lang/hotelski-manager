using HotelManager.Domain.Entities;
using HotelManager.Domain.Enums;
using HotelManager.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HotelManager.Data.Repositories;

public class RoomRepository : Repository<Room>, IRoomRepository
{
    public RoomRepository(HotelDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Room>> GetAvailableRoomsAsync(
        DateTime checkIn,
        DateTime checkOut,
        int guestsCount)
    {
        if (guestsCount < 1)
        {
            throw new ArgumentOutOfRangeException(
                nameof(guestsCount),
                "Guest count must be at least 1.");
        }

        var checkInDate = checkIn.Date;
        var checkOutDate = checkOut.Date;

        if (checkOutDate <= checkInDate)
        {
            throw new ArgumentException(
                "Check-out date must be after check-in date.",
                nameof(checkOut));
        }

        return await _context.Rooms
            .AsNoTracking()
            .Include(room => room.RoomType)
            .Where(room =>
                room.RoomType.Capacity >= guestsCount &&
                room.Status != RoomStatus.Maintenance &&
                !room.Reservations.Any(reservation =>
                    reservation.Status != ReservationStatus.Cancelled &&
                    reservation.Status != ReservationStatus.CheckedOut &&
                    reservation.CheckInDate < checkOutDate &&
                    reservation.CheckOutDate > checkInDate))
            .OrderBy(room => room.RoomNumber)
            .ToListAsync();
    }
}
