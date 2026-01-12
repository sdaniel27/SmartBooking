using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartBooking.Domain.Entities;

public partial class Hotel
{
    [Key]
    public int HotelId { get; set; }

    [StringLength(100)]
    public string HotelName { get; set; } = null!;

    [InverseProperty("Hotel")]
    public virtual List<Room> Rooms { get; set; } = new List<Room>();
}
