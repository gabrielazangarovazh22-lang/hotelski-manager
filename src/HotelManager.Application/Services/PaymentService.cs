using HotelManager.Domain.Entities;
using HotelManager.Domain.Enums;
using HotelManager.Domain.Exceptions;
using HotelManager.Domain.Repositories;

namespace HotelManager.Application.Services;

public class PaymentService : IPaymentService
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IRepository<Payment> _paymentRepository;

    public PaymentService(
        IReservationRepository reservationRepository,
        IRepository<Payment> paymentRepository)
    {
        _reservationRepository = reservationRepository
            ?? throw new ArgumentNullException(nameof(reservationRepository));
        _paymentRepository = paymentRepository
            ?? throw new ArgumentNullException(nameof(paymentRepository));
    }

    public async Task<Payment> RegisterPaymentAsync(
        int reservationId,
        decimal amount,
        PaymentMethod method)
    {
        amount = RoundMoney(amount);

        if (amount <= 0)
        {
            throw new DomainException("Payment amount must be greater than zero.");
        }

        var reservation = await _reservationRepository.GetReservationWithDetailsAsync(reservationId)
            ?? throw new DomainException($"Reservation {reservationId} was not found.");

        if (reservation.Status == ReservationStatus.Cancelled)
        {
            throw new DomainException(
                $"Payments cannot be registered for a reservation with status {reservation.Status}.");
        }

        var paid = reservation.Payments.Sum(payment => payment.Amount);
        var remainingBalance = RoundMoney(reservation.TotalPrice - paid);

        if (amount > remainingBalance)
        {
            throw new OverpaymentException(amount, remainingBalance);
        }

        var payment = new Payment
        {
            ReservationId = reservationId,
            Amount = amount,
            PaymentDate = DateTime.UtcNow,
            PaymentMethod = method
        };

        await _paymentRepository.AddAsync(payment);
        return payment;
    }

    public async Task<decimal> GetRemainingBalanceAsync(int reservationId)
    {
        var reservation = await _reservationRepository.GetReservationWithDetailsAsync(reservationId)
            ?? throw new DomainException($"Reservation {reservationId} was not found.");

        var paid = reservation.Payments.Sum(payment => payment.Amount);
        return RoundMoney(reservation.TotalPrice - paid);
    }

    private static decimal RoundMoney(decimal amount)
        => decimal.Round(amount, 2, MidpointRounding.AwayFromZero);
}
