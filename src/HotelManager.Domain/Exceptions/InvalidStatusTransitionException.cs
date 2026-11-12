using HotelManager.Domain.Enums;

namespace HotelManager.Domain.Exceptions;

public class InvalidStatusTransitionException : DomainException
{
    public InvalidStatusTransitionException(
        ReservationStatus currentStatus,
        ReservationStatus targetStatus)
        : base($"Cannot change reservation status from {currentStatus} to {targetStatus}.")
    {
        CurrentStatus = currentStatus;
        TargetStatus = targetStatus;
    }

    public ReservationStatus CurrentStatus { get; }

    public ReservationStatus TargetStatus { get; }
}
