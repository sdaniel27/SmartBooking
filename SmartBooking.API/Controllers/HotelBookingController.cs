using Microsoft.AspNetCore.Mvc;
using SmartBooking.API.ViewModels;
using SmartBooking.Application.Interfaces;

namespace SmartBooking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelBookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        public HotelBookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet("search-hotels")]
        public async Task<IActionResult> SearchHotelsByName([FromQuery] string name)
        {
            try
            {
                var hotels = await _bookingService.SearchHotelsByNameAsync(name);
                return Ok(hotels);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while processing your request.", Details = ex.Message });
            }
        }

        [HttpGet("available-rooms")]
        public async Task<IActionResult> GetAvailableRooms(int hotelId, DateTime checkIn, DateTime checkOut, int guests)
        {
            try
            {
                var rooms = await _bookingService.GetAvailableRoomsAsync(hotelId, checkIn,checkOut, guests);
                return Ok(rooms);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { Message = ex.Message });
            }            
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while processing your request.", Details = ex.Message });
            }
        }


        [HttpPost("book")]
        public async Task<IActionResult> CreateBooking(BookingViewModel viewModel)
        {
            try
            {
                var bookingReference = await _bookingService.CreateBookingAsync(viewModel.RoomId, viewModel.CheckInDate, viewModel.CheckOutDate, viewModel.Guests);
                return Ok(bookingReference);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while processing your request.", Details = ex.Message });
            }
        }

        [HttpGet("reference/{reference}")]
        public async Task<IActionResult> GetByReference(string reference)
        {
            try
            {
                var booking = await _bookingService.GetBookingDetailsAsync(reference);
                return Ok(booking);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while processing your request.", Details = ex.Message });
            }
        }

        [HttpPut("booking")]
        public async Task<IActionResult> UpdateBooking(UpdateBookingViewModel viewModel)
        {
            try
            {
                var status = await _bookingService.UpdateBookingAsync(viewModel.BookingReference, viewModel.RoomId, 
                    viewModel.CheckInDate, viewModel.CheckOutDate, viewModel.Guests);
                return Ok(status);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while processing your request.", Details = ex.Message });
            }
        }
    }
}
