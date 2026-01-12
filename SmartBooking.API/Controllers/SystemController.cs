using Microsoft.AspNetCore.Mvc;
using SmartBooking.Application.Enums;
using SmartBooking.Domain.Entities;
using SmartBooking.Infrastructure;

namespace SmartBooking.API.Controllers
{
    [ApiController]
    [Route("api/system")]
    public class SystemController : ControllerBase
    {
        private readonly SmartBookingContext _dbcontext;
        public SystemController(SmartBookingContext smartBookingContext)
        { 
            _dbcontext = smartBookingContext;
        }

        [HttpPost("seed")]
        public async Task<IActionResult> Seed()
        {
            var hotelTravelodge = new Hotel { HotelName = "Travelodge" };
            hotelTravelodge.Rooms.AddRange(new[] {
                new Room { RoomNumber = "101", RoomType = RoomType.Single, Capacity = 1 },
                new Room { RoomNumber = "102", RoomType = RoomType.Single, Capacity = 1 },
                new Room { RoomNumber = "201", RoomType = RoomType.Double, Capacity = 2 },
                new Room { RoomNumber = "202", RoomType = RoomType.Double, Capacity = 2 },
                new Room { RoomNumber = "301", RoomType = RoomType.Deluxe, Capacity = 4 },
                new Room { RoomNumber = "302", RoomType = RoomType.Deluxe, Capacity = 4 }
            });

            var hotelPremierInn = new Hotel { HotelName = "Premier Inn" };
            hotelPremierInn.Rooms.AddRange(new[] {
                new Room { RoomNumber = "1001", RoomType = RoomType.Single, Capacity = 1 },
                new Room { RoomNumber = "1002", RoomType = RoomType.Single, Capacity = 1 },
                new Room { RoomNumber = "2001", RoomType = RoomType.Double, Capacity = 2 },
                new Room { RoomNumber = "2002", RoomType = RoomType.Double, Capacity = 2 },
                new Room { RoomNumber = "2003", RoomType = RoomType.Deluxe, Capacity = 2 },
                new Room { RoomNumber = "3001", RoomType = RoomType.Deluxe, Capacity = 4 }
            });
            
            _dbcontext.Hotels.Add(hotelTravelodge);
            _dbcontext.Hotels.Add(hotelPremierInn);
            await _dbcontext.SaveChangesAsync();
            return Ok("Database Seeded");
        }

        [HttpPost("reset")]
        public async Task<IActionResult> Reset()
        {
            _dbcontext.Bookings.RemoveRange(_dbcontext.Bookings);
            _dbcontext.Rooms.RemoveRange(_dbcontext.Rooms);
            _dbcontext.Hotels.RemoveRange(_dbcontext.Hotels);
            await _dbcontext.SaveChangesAsync();
            return Ok("Database Reset");
        }
    }

}
