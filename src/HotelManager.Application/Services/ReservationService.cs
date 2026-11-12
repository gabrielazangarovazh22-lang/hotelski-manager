using HotelManager.Domain.Entities;
using HotelManager.Domain.Enums;
using HotelManager.Domain.Exceptions;
using HotelManager.Domain.Repositories;
using ReservationServiceEntity = HotelManager.Domain.Entities.ReservationService;

namespace HotelManager.Application.Services;

public class ReservationService : IReservationService
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IGuestRepository _guestRepository;
    private readonly IRepository<Service> _serviceRepository;

    public ReservationService(
        IReservationRepository reservationRepository,
        IRoomRepository roomRepository,
        IGuestRepository guestRepository,
        IRepository<Service> serviceRepository)
    {
        _reservationRepository = reservationRepository
            ?? throw new ArgumentNullException(nameof(reservationRepository));
        _roomRepository = roomRepository
            ?? throw new ArgumentNullException(nameof(roomRepository));
        _guestRepository = guestRepository
            ?? throw new ArgumentNullException(nameof(guestRepository));
        _serviceRepository = serviceRepository
            ?? throw new ArgumentNullException(nameof(serviceRepository));
    }

    public async Task<Reservation> CreateReservationAsync(
        int guestId,
        int roomId,
        DateTime checkIn,
        DateTime checkOut,
        int guestsCount)
    {
        if (guestsCount < 1)
        {
            throw new DomainException("Guest count must be at least 1.");
        }

        var checkInDate = checkIn.Date;
        var checkOutDate = checkOut.Date;

        if (checkOutDate <= checkInDate)
        {
            throw new DomainException("Check-out date must be after check-in date.");
        }

        if (await _guestRepository.GetByIdAsync(guestId) is null)
        {
            throw new DomainException($"Guest {guestId} was not found.");
        }

        var availableRoom = (await _roomRepository.GetAvailableRoomsAsync(
                checkInDate,
                checkOutDate,
                guestsCount))
            .FirstOrDefault(room => room.Id == roomId);

        if (availableRoom is null)
        {
            if (await _roomRepository.GetByIdAsync(roomId) is null)
            {
                throw new DomainException($"Room {roomId} was not found.");
            }

            var availableForOneGuest = (await _roomRepository.GetAvailableRoomsAsync(
                    checkInDate,
                    checkOutDate,
                    1))
                .Any(room => room.Id == roomId);

            if (availableForOneGuest)
            {
                throw new RoomUnavailableException(
                    $"Room {roomId} cannot accommodate {guestsCount} guests.");
            }

            throw new RoomUnavailableException(
                $"Room {roomId} is unavailable for the requested dates.");
        }

        var nights = (checkOutDate - checkInDate).Days;

        var reservation = new Reservation
        {
            GuestId = guestId,
            RoomId = roomId,
            CheckInDate = checkInDate,
            CheckOutDate = checkOutDate,
            NumberOfGuests = guestsCount,
            TotalPrice = RoundMoney(nights * availableRoom.RoomType.BasePricePerNight),
            Status = ReservationStatus.Pending
        };

        await _reservationRepository.AddAsync(reservation);
        return reservation;
    }

    public async Task<Reservation> AddServiceToReservationAsync(
        int reservationId,
        int serviceId,
        int quantity)
    {
        if (quantity < 1)
        {
            throw new DomainException("Service quantity must be at least 1.");
        }

        var reservation = await _reservationRepository.GetReservationWithDetailsAsync(reservationId)
            ?? throw new DomainException($"Reservation {reservationId} was not found.");

        if (reservation.Status is ReservationStatus.CheckedOut or ReservationStatus.Cancelled)
        {
            throw new DomainException(
                $"Services cannot be added to a reservation with status {reservation.Status}.");
        }

        var service = await _serviceRepository.GetByIdAsync(serviceId)
            ?? throw new DomainException($"Service {serviceId} was not found.");

        var existingLine = reservation.ReservationServices
            .FirstOrDefault(item => item.ServiceId == serviceId);

        if (existingLine is not null)
        {
            existingLine.Quantity += quantity;
        }
        else
        {
            reservation.ReservationServices.Add(new ReservationServiceEntity
            {
                ReservationId = reservation.Id,
                ServiceId = service.Id,
                Quantity = quantity,
                AppliedPrice = service.Price
            });
        }

        var nights = (reservation.CheckOutDate.Date - reservation.CheckInDate.Date).Days;
        var roomTotal = nights * reservation.Room.RoomType.BasePricePerNight;
        var servicesTotal = reservation.ReservationServices
            .Sum(item => item.Quantity * item.AppliedPrice);

        reservation.TotalPrice = RoundMoney(roomTotal + servicesTotal);

        await _reservationRepository.UpdateAsync(reservation);
        return reservation;
    }

    public async Task ConfirmReservationAsync(int reservationId)
    {
        var reservation = await LoadReservationAsync(reservationId);
        EnsureTransition(reservation, ReservationStatus.Confirmed);

        reservation.Status = ReservationStatus.Confirmed;
        await _reservationRepository.UpdateAsync(reservation);
    }

    public async Task CheckInAsync(int reservationId)
    {
        var reservation = await LoadReservationAsync(reservationId);
        EnsureTransition(reservation, ReservationStatus.CheckedIn);

        reservation.Status = ReservationStatus.CheckedIn;
        await _reservationRepository.UpdateAsync(reservation);

        var room = await _roomRepository.GetByIdAsync(reservation.RoomId)
            ?? throw new DomainException($"Room {reservation.RoomId} was not found.");

        room.Status = RoomStatus.Occupied;
        await _roomRepository.UpdateAsync(room);
    }

    public async Task CheckOutAsync(int reservationId)
    {
        var reservation = await LoadReservationAsync(reservationId);
        EnsureTransition(reservation, ReservationStatus.CheckedOut);

        reservation.Status = ReservationStatus.CheckedOut;
        await _reservationRepository.UpdateAsync(reservation);

        var room = await _roomRepository.GetByIdAsync(reservation.RoomId)
            ?? throw new DomainException($"Room {reservation.RoomId} was not found.");

        room.Status = RoomStatus.Available;
        await _roomRepository.UpdateAsync(room);
    }

    public async Task CancelReservationAsync(int reservationId)
    {
        var reservation = await LoadReservationAsync(reservationId);
        EnsureTransition(reservation, ReservationStatus.Cancelled);

        reservation.Status = ReservationStatus.Cancelled;
        await _reservationRepository.UpdateAsync(reservation);
    }

    private async Task<Reservation> LoadReservationAsync(int reservationId)
    {
        return await _reservationRepository.GetByIdAsync(reservationId)
            ?? throw new DomainException($"Reservation {reservationId} was not found.");
    }

    private static void EnsureTransition(Reservation reservation, ReservationStatus targetStatus)
    {
        var isAllowed = targetStatus switch
        {
            ReservationStatus.Confirmed => reservation.Status == ReservationStatus.Pending,
            ReservationStatus.CheckedIn => reservation.Status == ReservationStatus.Confirmed,
            ReservationStatus.CheckedOut => reservation.Status == ReservationStatus.CheckedIn,
            ReservationStatus.Cancelled => reservation.Status is
                ReservationStatus.Pending or ReservationStatus.Confirmed,
            _ => false
        };

        if (!isAllowed)
        {
            throw new InvalidStatusTransitionException(reservation.Status, targetStatus);
        }
    }

    private static decimal RoundMoney(decimal amount)
        => decimal.Round(amount, 2, MidpointRounding.AwayFromZero);
}
