namespace SmartBooking.Application.DTOs
{
    public class BookingDto
    {
        public string BookingReference { get; set; } = string.Empty;
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public int Guests { get; set; }
        public string RoomNumber { get; set; } = string.Empty;
        public string RoomType { get; set; } = string.Empty;
        public string HotelName { get; set; } = string.Empty;
    }
}
