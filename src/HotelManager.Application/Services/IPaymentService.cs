using HotelManager.Domain.Entities;
using HotelManager.Domain.Enums;

namespace HotelManager.Application.Services;

public interface IPaymentService
{
    Task<Payment> RegisterPaymentAsync(
        int reservationId,
        decimal amount,
        PaymentMethod method);

    Task<decimal> GetRemainingBalanceAsync(int reservationId);
}
