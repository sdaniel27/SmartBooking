using SmartBooking.Application.DTOs;
using SmartBooking.Application.Enums;
using SmartBooking.Application.Interfaces;
using SmartBooking.Domain.Entities;

namespace SmartBooking.Application.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;

        public object Enumerations { get; private set; }

        public BookingService(IBookingRepository bookingRepository) 
        { 
            _bookingRepository = bookingRepository;
        }

        public async Task<List<HotelDto>> SearchHotelsByNameAsync(string name)
        {
            var hotels = await _bookingRepository.GetHotelByNameAsync(name);
            if (hotels.Count == 0) throw new KeyNotFoundException("Hotels with matching name not found");

            var hotelDtos = hotels.Select(h => new HotelDto
            {
                Id = h.HotelId,
                HotelName = h.HotelName
            }).ToList();

            return hotelDtos;
        }

        private void ValidateInputs(DateTime checkIn, DateTime checkOut, int guests)
        {
            if (checkIn < DateTime.Now)
            {
                throw new InvalidOperationException("Check-in date must be in the future.");
            }
            else if (checkIn >= checkOut)
            {
                throw new InvalidOperationException("Check-out date must be after check-in date.");
            }
            else if (guests < 1 || guests > 4)
            {
                throw new InvalidOperationException("Invalid number of guests entered. Please enter 1 to 4 guests");
            }
        }

        public async Task<List<RoomDto>> GetAvailableRoomsAsync(int hotelId, DateTime checkIn, DateTime checkOut, int guests)
        {
            ValidateInputs(checkIn, checkOut, guests);

            var rooms = await _bookingRepository.GetAvailableRoomsAsync(hotelId, checkIn, checkOut, guests);
            if (rooms.Count == 0) throw new InvalidOperationException("No available rooms found for the specified dates");

            var roomDtos = rooms.Select(r => new RoomDto
            {
                RoomId = r.RoomId,
                RoomNumber = r.RoomNumber,
                RoomType = r.RoomType.GetDescription(), 
                Capacity = r.Capacity
            }).ToList();

            return roomDtos;
        }

        public async Task<string> CreateBookingAsync(int roomId, DateTime checkIn, DateTime checkOut, int guests)
        {
            ValidateInputs(checkIn, checkOut, guests);

            var isAvailable = await _bookingRepository.IsRoomAvailableAsync(roomId, checkIn, checkOut, guests);
            if (!isAvailable)
            {
                throw new InvalidOperationException("Room is not available for the selected dates.");
            }

             var bookingReference = await _bookingRepository.AddBookingAsync(roomId, checkIn, checkOut, guests);
            return bookingReference;
        }

        public async Task<BookingDto> GetBookingDetailsAsync(string bookingReference)
        {
            var booking = await _bookingRepository.GetBookingByReferenceAsync(bookingReference);

            return booking == null ? throw new KeyNotFoundException("Booking not found") : new BookingDto
            {
                BookingReference = booking.Reference,
                CheckIn = booking.CheckIn,
                CheckOut = booking.CheckOut,
                Guests = booking.Guests,
                RoomNumber = booking.Room.RoomNumber,
                RoomType = booking.Room.RoomType.GetDescription(),
                HotelName = booking.Room.Hotel.HotelName
            };
        }

        public async Task<bool> UpdateBookingAsync(string bookingReference, int roomId, DateTime newCheckIn, DateTime newCheckOut, int guests)
        {
            ValidateInputs(newCheckIn, newCheckOut, guests);
            
            var booking = await _bookingRepository.GetBookingByReferenceAsync(bookingReference);

            if (booking == null)
            {
                throw new InvalidOperationException("Booking not found");
            }
            else if (DateTime.UtcNow > booking.CheckOut)
            {
                throw new InvalidOperationException("Past Booking cant be updated");
            }
            else if (booking.CheckIn <= DateTime.UtcNow && booking.CheckOut >= DateTime.UtcNow)
            {
                throw new InvalidOperationException("Rooms cannot be changed during stay");
            }
            else if (!await _bookingRepository.IsRoomAvailableAsync(roomId, newCheckIn, newCheckOut, guests))
            {
                throw new InvalidOperationException("Room is not available for the selected dates.");
            }
            else
            {
                var updateBooking = new Booking()
                {
                    BookingId = booking.BookingId,
                    Reference = booking.Reference,
                    RoomId = roomId,
                    CheckIn = newCheckIn,
                    CheckOut = newCheckOut,
                    Guests = guests
                };
                return await _bookingRepository.UpdateBookingAsync(updateBooking);
            }
        }
    }
}
