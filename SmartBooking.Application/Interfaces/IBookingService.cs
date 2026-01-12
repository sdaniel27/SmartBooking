using SmartBooking.Application.DTOs;

namespace SmartBooking.Application.Interfaces
{
    public interface IBookingService
    {
        Task<List<HotelDto>> SearchHotelsByNameAsync(string name);

        Task<List<RoomDto>> GetAvailableRoomsAsync(int hotelId, DateTime checkIn, DateTime checkOut, int guests);        

        Task<string> CreateBookingAsync(int roomId, DateTime checkIn, DateTime checkOut, int guests);

        Task<BookingDto> GetBookingDetailsAsync(string bookingReference);

        Task<bool> UpdateBookingAsync(string bookingReference, int roomId, DateTime newCheckIn, DateTime newCheckOut, int guests);

    }
}
