using HotelManager.Domain.Entities;

namespace HotelManager.Domain.Repositories;

public interface IRoomRepository : IRepository<Room>
{
    Task<IEnumerable<Room>> GetAvailableRoomsAsync(
        DateTime checkIn,
        DateTime checkOut,
        int guestsCount);
}
