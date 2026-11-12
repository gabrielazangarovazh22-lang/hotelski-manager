using HotelManager.Domain.Entities;
using HotelManager.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HotelManager.Data.Repositories;

public class ReservationRepository : Repository<Reservation>, IReservationRepository
{
    public ReservationRepository(HotelDbContext context) : base(context)
    {
    }

    public async Task<Reservation?> GetReservationWithDetailsAsync(int id)
    {
        return await _context.Reservations
            .AsSplitQuery()
            .Include(reservation => reservation.Guest)
            .Include(reservation => reservation.Room)
                .ThenInclude(room => room.RoomType)
            .Include(reservation => reservation.ReservationServices)
                .ThenInclude(reservationService => reservationService.Service)
            .Include(reservation => reservation.Payments)
            .FirstOrDefaultAsync(reservation => reservation.Id == id);
    }
}
