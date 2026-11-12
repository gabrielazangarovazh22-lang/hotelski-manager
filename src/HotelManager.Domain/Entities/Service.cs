using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelManager.Domain.Entities;

public class Service
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }

    public ICollection<ReservationService> ReservationServices { get; set; } = new List<ReservationService>();
}
