namespace SmartBooking.API.ViewModels
{
    public class UpdateBookingViewModel
    {
        public string BookingReference { get; set; } = string.Empty;
        public int RoomId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int Guests { get; set; }
    }
}
