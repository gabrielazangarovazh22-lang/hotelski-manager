using HotelManager.Domain.Entities;

namespace HotelManager.Domain.Repositories;

public interface IReservationRepository : IRepository<Reservation>
{
    Task<Reservation?> GetReservationWithDetailsAsync(int id);
}
