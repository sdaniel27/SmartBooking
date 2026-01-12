using SmartBooking.Domain.Entities;

namespace SmartBooking.Application.Interfaces
{
    public interface IBookingRepository
    {
        Task<List<Hotel>> GetHotelByNameAsync(string name);
        Task<List<Room>> GetAvailableRoomsAsync(int hotelId, DateTime checkIn, DateTime checkOut, int guests);
        Task<bool> IsRoomAvailableAsync(int roomId, DateTime checkIn, DateTime checkOut, int guests);
        Task<string> AddBookingAsync(int roomId, DateTime checkIn, DateTime checkOut, int guests);
        Task<Booking?> GetBookingByReferenceAsync(string bookingReference);
        Task<bool> UpdateBookingAsync(Booking booking);
    }
}
