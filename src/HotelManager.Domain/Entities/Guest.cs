using System.ComponentModel.DataAnnotations;

namespace HotelManager.Domain.Entities;

public class Guest
{
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [MaxLength(30)]
    [Phone]
    public string Phone { get; set; } = string.Empty;

    [MaxLength(150)]
    [EmailAddress]
    public string? Email { get; set; }

    [Required]
    [MaxLength(30)]
    public string IdentityDocumentNumber { get; set; } = string.Empty;

    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
