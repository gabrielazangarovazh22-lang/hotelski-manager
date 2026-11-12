using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HotelManager.Domain.Enums;

namespace HotelManager.Domain.Entities;

public class Reservation
{
    public int Id { get; set; }

    [Required]
    public int GuestId { get; set; }

    public Guest Guest { get; set; } = null!;

    [Required]
    public int RoomId { get; set; }

    public Room Room { get; set; } = null!;

    [Required]
    public DateTime CheckInDate { get; set; }

    [Required]
    public DateTime CheckOutDate { get; set; }

    [Range(1, 20)]
    public int NumberOfGuests { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalPrice { get; set; }

    public ReservationStatus Status { get; set; } = ReservationStatus.Pending;

    public ICollection<ReservationService> ReservationServices { get; set; } = new List<ReservationService>();

    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
