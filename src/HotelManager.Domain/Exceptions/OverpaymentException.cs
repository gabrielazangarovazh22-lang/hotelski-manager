namespace HotelManager.Domain.Exceptions;

public class OverpaymentException : DomainException
{
    public OverpaymentException(decimal attemptedAmount, decimal remainingBalance)
        : base($"Payment amount {attemptedAmount:0.00} exceeds the remaining balance of {remainingBalance:0.00}.")
    {
        AttemptedAmount = attemptedAmount;
        RemainingBalance = remainingBalance;
    }

    public decimal AttemptedAmount { get; }

    public decimal RemainingBalance { get; }
}
