using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HotelManager.Domain.Enums;

namespace HotelManager.Domain.Entities;

public class Payment
{
    public int Id { get; set; }

    [Required]
    public int ReservationId { get; set; }

    public Reservation Reservation { get; set; } = null!;

    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }

    [Required]
    public DateTime PaymentDate { get; set; }

    public PaymentMethod PaymentMethod { get; set; }
}
