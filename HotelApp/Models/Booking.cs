using System;

namespace HotelApp.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int RoomId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public DateTime BookingDate { get; set; }

        // Navigation properties
        public Customer Customer { get; set; }
        public Room Room { get; set; }
    }
}
