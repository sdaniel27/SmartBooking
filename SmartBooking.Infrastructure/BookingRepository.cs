using Microsoft.EntityFrameworkCore;
using SmartBooking.Application.Interfaces;
using SmartBooking.Domain.Entities;

namespace SmartBooking.Infrastructure
{
    public class BookingRepository : IBookingRepository
    {
        private readonly SmartBookingContext _dbContext;
        public BookingRepository(SmartBookingContext smartBookingContext) 
        { 
            _dbContext = smartBookingContext;
        }
        public async Task<List<Hotel>> GetHotelByNameAsync(string name)
        {
            return await _dbContext.Hotels
                .Where(h => h.HotelName.Contains(name))
                .ToListAsync();
        }

        public async Task<List<Room>> GetAvailableRoomsAsync(int hotelId, DateTime checkIn, DateTime checkOut, int guests)
        {
            return await _dbContext.Rooms
                .Where(r => r.HotelId == hotelId && r.Capacity >= guests)
                .Where(r => !_dbContext.Bookings
                    .Any(b => b.RoomId == r.RoomId &&
                              ((checkIn >= b.CheckIn && checkIn < b.CheckOut) ||
                               (checkOut > b.CheckIn && checkOut <= b.CheckOut) ||
                               (checkIn <= b.CheckIn && checkOut >= b.CheckOut))))
                .ToListAsync();
        }

        public async Task<bool> IsRoomAvailableAsync(int roomId, DateTime checkIn, DateTime checkOut, int guests)
        {
            var isAvailable = await _dbContext.Rooms
                .Where(r => r.RoomId == roomId && r.Capacity <= guests)
                .Where(r => !_dbContext.Bookings
                    .Any(b => b.RoomId == r.RoomId &&
                              ((checkIn >= b.CheckIn && checkIn < b.CheckOut) ||
                               (checkOut > b.CheckIn && checkOut <= b.CheckOut) ||
                               (checkIn <= b.CheckIn && checkOut >= b.CheckOut))))
                .AnyAsync();

            return isAvailable;

        }

        public async Task<String> AddBookingAsync(int roomId, DateTime checkIn, DateTime checkOut, int guests)
        {
            var booking = new Booking
            {
                Reference = Guid.NewGuid().ToString("N").ToUpper().Substring(0, 8),
                RoomId = roomId,
                CheckIn = checkIn,
                CheckOut = checkOut,
                Guests = guests
            };

            _dbContext.Bookings.Add(booking);
            await _dbContext.SaveChangesAsync();

            return booking.Reference;
        }        

        public async Task<Booking?> GetBookingByReferenceAsync(string bookingReference)
        {
            return await _dbContext.Bookings
                .Where(b => b.Reference == bookingReference)
                .Select( b => new Booking
                {
                    BookingId = b.BookingId,
                    Reference = b.Reference,
                    CheckIn = b.CheckIn,
                    CheckOut = b.CheckOut,
                    Guests = b.Guests,
                    Room = new Room
                    {
                        RoomNumber = b.Room.RoomNumber,
                        RoomType = b.Room.RoomType,
                        Hotel = new Hotel
                        {
                            HotelName = b.Room.Hotel.HotelName
                        }
                    },
                }).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateBookingAsync(Booking Booking)
        {
            _dbContext.Bookings.Update(Booking);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
