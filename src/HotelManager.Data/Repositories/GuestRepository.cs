using HotelManager.Domain.Entities;
using HotelManager.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HotelManager.Data.Repositories;

public class GuestRepository : Repository<Guest>, IGuestRepository
{
    public GuestRepository(HotelDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Guest>> SearchGuestsAsync(
        string? phone,
        string? email,
        string? identityDocument)
    {
        IQueryable<Guest> query = _context.Guests.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(phone))
        {
            var phoneFilter = phone.Trim();
            query = query.Where(guest => guest.Phone.Contains(phoneFilter));
        }

        if (!string.IsNullOrWhiteSpace(email))
        {
            var emailFilter = email.Trim().ToLower();
            query = query.Where(guest =>
                guest.Email != null &&
                guest.Email!.ToLower().Contains(emailFilter));
        }

        if (!string.IsNullOrWhiteSpace(identityDocument))
        {
            var documentFilter = identityDocument.Trim().ToLower();
            query = query.Where(guest =>
                guest.IdentityDocumentNumber.ToLower().Contains(documentFilter));
        }

        return await query
            .OrderBy(guest => guest.LastName)
            .ThenBy(guest => guest.FirstName)
            .ToListAsync();
    }
}
