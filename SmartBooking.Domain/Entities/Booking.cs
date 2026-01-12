using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartBooking.Domain.Entities;

public partial class Booking
{
    [Key]
    public int BookingId { get; set; }

    [StringLength(8)]
    public string Reference { get; set; } = null!;

    public int RoomId { get; set; }

    public DateTime CheckIn { get; set; }

    public DateTime CheckOut { get; set; }

    public int Guests { get; set; }

    [ForeignKey("RoomId")]
    [InverseProperty("Bookings")]
    public virtual Room Room { get; set; } = null!;
}
