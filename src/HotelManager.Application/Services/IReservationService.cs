using HotelManager.Domain.Entities;

namespace HotelManager.Application.Services;

public interface IReservationService
{
    Task<Reservation> CreateReservationAsync(
        int guestId,
        int roomId,
        DateTime checkIn,
        DateTime checkOut,
        int guestsCount);

    Task<Reservation> AddServiceToReservationAsync(
        int reservationId,
        int serviceId,
        int quantity);

    Task ConfirmReservationAsync(int reservationId);

    Task CheckInAsync(int reservationId);

    Task CheckOutAsync(int reservationId);

    Task CancelReservationAsync(int reservationId);
}
