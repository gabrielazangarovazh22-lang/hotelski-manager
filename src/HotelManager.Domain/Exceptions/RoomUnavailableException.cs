namespace HotelManager.Domain.Exceptions;

public class RoomUnavailableException : DomainException
{
    public RoomUnavailableException(string message)
        : base(message)
    {
    }

    public RoomUnavailableException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
