using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SmartBooking.Application.Enums;

namespace SmartBooking.Domain.Entities;

public partial class Room
{
    [Key]
    public int RoomId { get; set; }

    public int HotelId { get; set; }

    [StringLength(4)]
    public string RoomNumber { get; set; } = null!;

    public RoomType RoomType { get; set; }

    public int Capacity { get; set; }

    [InverseProperty("Room")]
    public virtual List<Booking> Bookings { get; set; } = new List<Booking>();

    [ForeignKey("HotelId")]
    [InverseProperty("Rooms")]
    public virtual Hotel Hotel { get; set; } = null!;
}
