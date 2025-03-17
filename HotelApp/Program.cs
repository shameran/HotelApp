using System;
using HotelApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace HotelApp
{
    class Program
    {
        static void Main(string[] args)
        {
            bool running = true;

            while (running)
            {
                Console.Clear();
                Console.WriteLine("Välkommen till Shameran Hotellappen!");
                Console.WriteLine("Välj ett alternativ:");
                Console.WriteLine("1. Lägg till en kund");
                Console.WriteLine("2. Lägg till ett rum");
                Console.WriteLine("3. Lägg till en bokning");
                Console.WriteLine("4. Visa alla bokningar");
                Console.WriteLine("5. Uppdatera en bokning");
                Console.WriteLine("6. Ta bort en bokning");
                Console.WriteLine("7. Rensa alla rum och kunder");
                Console.WriteLine("8. Avsluta");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddCustomer();
                        break;

                    case "2":
                        AddRoom();
                        break;

                    case "3":
                        AddBooking();
                        break;

                    case "4":
                        ShowBookings();
                        break;

                    case "5":
                        UpdateBooking();
                        break;

                    case "6":
                        DeleteBooking();
                        break;

                    case "7":
                        ClearAllRoomsAndCustomers();
                        break;

                    case "8":
                        running = false;
                        break;

                    default:
                        Console.WriteLine("Ogiltigt alternativ. Försök igen.");
                        break;
                }

                Console.WriteLine("\nTryck på en tangent för att fortsätta...");
                Console.ReadLine();
            }
        }

        // Lägg till en kund
        public static void AddCustomer()
        {
            Console.WriteLine("Ange kundens namn:");
            string name = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Kundens namn kan inte vara tomt.");
                return;
            }

            using (var context = new HotelDbContext())
            {
                var customer = new Customer { Name = name };
                context.Customers.Add(customer);
                context.SaveChanges();
                Console.WriteLine($"Kund {name} tillagd!");
            }
        }

        // Lägg till ett rum
        public static void AddRoom()
        {
            Console.WriteLine("Ange rumnummer:");
            string roomNumber = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(roomNumber))
            {
                Console.WriteLine("Rumnumret kan inte vara tomt.");
                return;
            }

            Console.WriteLine("Ange rumtyp (t.ex. Enkelrum, Dubbelrum):");
            string roomType = Console.ReadLine();

            Console.WriteLine("Ange kapacitet (antal personer):");
            if (!int.TryParse(Console.ReadLine(), out int capacity) || capacity <= 0)
            {
                Console.WriteLine("Ogiltig kapacitet.");
                return;
            }

            using (var context = new HotelDbContext())
            {
                var room = new Room
                {
                    RoomNumber = roomNumber,
                    RoomType = roomType,
                    Capacity = capacity
                };
                context.Rooms.Add(room);
                context.SaveChanges();
                Console.WriteLine($"Rum {roomNumber} tillagt!");
            }
        }

        // Lägg till en bokning
        public static void AddBooking()
        {
            try
            {
                // Visar listan över kunder
                using (var context = new HotelDbContext())
                {
                    var customers = context.Customers.ToList();
                    Console.WriteLine("Tillgängliga kunder:");
                    foreach (var customer in customers)
                    {
                        Console.WriteLine($"ID: {customer.Id}, Namn: {customer.Name}");
                    }
                }

                Console.WriteLine("Ange kundens ID (från lista ovan):");
                if (!int.TryParse(Console.ReadLine(), out int customerId))
                {
                    Console.WriteLine("Ogiltigt kund-ID.");
                    return;
                }

                // Visar listan över rum
                using (var context = new HotelDbContext())
                {
                    var rooms = context.Rooms.ToList();
                    Console.WriteLine("Tillgängliga rum:");
                    foreach (var room in rooms)
                    {
                        Console.WriteLine($"ID: {room.Id}, Rumnummer: {room.RoomNumber}, Typ: {room.RoomType}");
                    }
                }

                Console.WriteLine("Ange rums-ID (från lista ovan):");
                if (!int.TryParse(Console.ReadLine(), out int roomId))
                {
                    Console.WriteLine("Ogiltigt rums-ID.");
                    return;
                }

                Console.WriteLine("Ange incheckningsdatum (yyyy-mm-dd):");
                if (!DateTime.TryParse(Console.ReadLine(), out DateTime checkIn))
                {
                    Console.WriteLine("Ogiltigt incheckningsdatum.");
                    return;
                }

                Console.WriteLine("Ange utcheckningsdatum (yyyy-mm-dd):");
                if (!DateTime.TryParse(Console.ReadLine(), out DateTime checkOut))
                {
                    Console.WriteLine("Ogiltigt utcheckningsdatum.");
                    return;
                }

                using (var context = new HotelDbContext())
                {
                    var customer = context.Customers.Find(customerId);
                    var room = context.Rooms.Find(roomId);

                    if (customer == null)
                    {
                        Console.WriteLine("Kund med angivet ID hittades inte.");
                        return;
                    }

                    if (room == null)
                    {
                        Console.WriteLine("Rum med angivet ID hittades inte.");
                        return;
                    }

                    var booking = new Booking
                    {
                        CustomerId = customerId,
                        RoomId = roomId,
                        CheckInDate = checkIn,
                        CheckOutDate = checkOut,
                        BookingDate = DateTime.Now
                    };

                    context.Bookings.Add(booking);
                    context.SaveChanges();
                    Console.WriteLine("Bokning tillagd!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ett fel uppstod: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
            }
        }

        public static void ShowBookings()
        {
            using (var context = new HotelDbContext())
            {
                var bookings = context.Bookings
                                       .Include(b => b.Customer)  // Ladda relaterad kund
                                       .Include(b => b.Room)      // Ladda relaterat rum
                                       .ToList();

                Console.WriteLine("Alla bokningar:");
                foreach (var booking in bookings)
                {
                    Console.WriteLine($"Bokning ID: {booking.Id}, Kund: {booking.Customer.Name}, Rum: {booking.Room.RoomNumber}, Incheckning: {booking.CheckInDate}, Utcheckning: {booking.CheckOutDate}");
                }
            }
        }

        public static void ClearAllRoomsAndCustomers()
        {
            using (var context = new HotelDbContext())
            {
                // Hämtar alla rum och kunder från databasen
                var rooms = context.Rooms.ToList();
                var customers = context.Customers.ToList();

                // Kontrollera om det finns några rum eller kunder
                if (rooms.Count == 0 && customers.Count == 0)
                {
                    Console.WriteLine("Det finns inga rum eller kunder att ta bort.");
                    return; 
                }

                // Visar en varning och be om bekräftelse
                Console.WriteLine("VARNING: Detta kommer att ta bort ALLA rum och kunder.");
                Console.WriteLine("Antal rum som kommer att raderas: " + rooms.Count);
                Console.WriteLine("Antal kunder som kommer att raderas: " + customers.Count);
                Console.WriteLine("Är du säker på att du vill fortsätta? (ja/nej)");
                string confirmation = Console.ReadLine();

                if (confirmation.ToLower() == "ja")
                {
                   
                    context.Rooms.RemoveRange(rooms);
                    context.Customers.RemoveRange(customers);
                    context.SaveChanges();
                    Console.WriteLine("Alla rum och kunder har tagits bort.");
                }
                else
                {
                    Console.WriteLine("Åtgärd avbruten. Inga rum eller kunder har tagits bort.");
                }
            }
        }

        public static void UpdateBooking()
        {
            using (var context = new HotelDbContext())
            {
                // Hämta alla bokningar från databasen
                var bookings = context.Bookings
                                      .Include(b => b.Customer)
                                      .Include(b => b.Room)
                                      .ToList();

                // Kontrollera om det finns några bokningar
                if (bookings.Count == 0)
                {
                    Console.WriteLine("Det finns inga bokningar att uppdatera.");
                    return; 
                }

                // Visar alla bokningar
                Console.WriteLine("Alla bokningar:");
                foreach (var booking in bookings)
                {
                    Console.WriteLine($"Bokning ID: {booking.Id}, Kund: {booking.Customer.Name}, Rum: {booking.Room.RoomNumber}, Incheckning: {booking.CheckInDate}, Utcheckning: {booking.CheckOutDate}");
                }

                //  användaren ange ID för bokningen som ska uppdateras
                Console.WriteLine("Ange bokningens ID för uppdatering:");
                if (!int.TryParse(Console.ReadLine(), out int bookingId))
                {
                    Console.WriteLine("Ogiltigt boknings-ID.");
                    return;
                }

                // Hittar bokningen med det angivna ID:t
                var bookingToUpdate = bookings.FirstOrDefault(b => b.Id == bookingId);
                if (bookingToUpdate == null)
                {
                    Console.WriteLine("Bokning med angivet ID hittades inte.");
                    return;
                }

                // användaren ange nytt incheckningsdatum
                Console.WriteLine("Ange nytt incheckningsdatum (yyyy-mm-dd):");
                if (!DateTime.TryParse(Console.ReadLine(), out DateTime newCheckIn))
                {
                    Console.WriteLine("Ogiltigt incheckningsdatum.");
                    return;
                }

                // användaren ange nytt utcheckningsdatum
                Console.WriteLine("Ange nytt utcheckningsdatum (yyyy-mm-dd):");
                if (!DateTime.TryParse(Console.ReadLine(), out DateTime newCheckOut))
                {
                    Console.WriteLine("Ogiltigt utcheckningsdatum.");
                    return;
                }

                
                bookingToUpdate.CheckInDate = newCheckIn;
                bookingToUpdate.CheckOutDate = newCheckOut;

                
                context.SaveChanges();
                Console.WriteLine($"Bokning {bookingId} uppdaterad!");
            }
        }

        //  Ta bort en bokning
        public static void DeleteBooking()
        {
            Console.WriteLine("Ange bokningens ID för borttagning:");
            if (!int.TryParse(Console.ReadLine(), out int bookingId))
            {
                Console.WriteLine("Ogiltigt boknings-ID.");
                return;
            }

            using (var context = new HotelDbContext())
            {
                var booking = context.Bookings.FirstOrDefault(b => b.Id == bookingId);
                if (booking != null)
                {
                    context.Bookings.Remove(booking);
                    context.SaveChanges();
                    Console.WriteLine($"Bokning {bookingId} raderad!");
                }
                else
                {
                    Console.WriteLine("Bokning inte hittad.");
                }
            }
        }
    }
}
