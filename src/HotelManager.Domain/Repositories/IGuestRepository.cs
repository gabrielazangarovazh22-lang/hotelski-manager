using HotelManager.Domain.Entities;

namespace HotelManager.Domain.Repositories;

public interface IGuestRepository : IRepository<Guest>
{
    Task<IEnumerable<Guest>> SearchGuestsAsync(
        string? phone,
        string? email,
        string? identityDocument);
}
