using System.ComponentModel.DataAnnotations;
using HotelManager.Domain.Enums;

namespace HotelManager.Domain.Entities;

public class Room
{
    public int Id { get; set; }

    [Required]
    [MaxLength(10)]
    public string RoomNumber { get; set; } = string.Empty;

    [Range(0, 100)]
    public int Floor { get; set; }

    [Required]
    public int RoomTypeId { get; set; }

    public RoomType RoomType { get; set; } = null!;

    public RoomStatus Status { get; set; } = RoomStatus.Available;

    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
