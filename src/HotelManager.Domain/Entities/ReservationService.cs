using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelManager.Domain.Entities;

public class ReservationService
{
    public int Id { get; set; }

    [Required]
    public int ReservationId { get; set; }

    public Reservation Reservation { get; set; } = null!;

    [Required]
    public int ServiceId { get; set; }

    public Service Service { get; set; } = null!;

    [Range(1, 50)]
    public int Quantity { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal AppliedPrice { get; set; }
}
