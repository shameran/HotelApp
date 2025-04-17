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
            using (var context = new HotelDbContext())
            {
                context.Database.Migrate();
            }

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
                Console.WriteLine("8. Uppdatera en kund");
                Console.WriteLine("9. Avsluta");

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
                        UpdateCustomer();
                        break;
                    case "9":
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

        public static void AddCustomer()
        {
            Console.WriteLine("Ange kundens förnamn:");
            string firstName = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(firstName))
            {
                Console.WriteLine("Förnamn kan inte vara tomt.");
                return;
            }

            Console.WriteLine("Ange kundens efternamn:");
            string lastName = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(lastName))
            {
                Console.WriteLine("Efternamn kan inte vara tomt.");
                return;
            }

            Console.WriteLine("Ange kundens e-postadress:");
            string email = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@") || !email.Contains("."))
            {
                Console.WriteLine("Ogiltig e-postadress.");
                return;
            }

            Console.WriteLine("Ange kundens stad:");
            string city = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(city))
            {
                Console.WriteLine("Stad kan inte vara tom.");
                return;
            }

            using (var context = new HotelDbContext())
            {
                var customer = new Customer
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    City = city
                };

                context.Customers.Add(customer);
                context.SaveChanges();

                Console.WriteLine($"Kund {customer.Name} från {city} tillagd!");
            }
        }

        public static void AddRoom()
        {
            Console.WriteLine("Ange rumnummer:");
            string roomNumber = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(roomNumber))
            {
                Console.WriteLine("Rumnumret kan inte vara tomt.");
                return;
            }

            Console.WriteLine("Ange rumtyp (t.ex. Enkelrum, Dubbelrum, Suite):");
            string roomType = Console.ReadLine();

            Console.WriteLine("Ange kapacitet (antal personer):");
            if (!int.TryParse(Console.ReadLine(), out int capacity) || capacity <= 0)
            {
                Console.WriteLine("Ogiltig kapacitet.");
                return;
            }

            // Om det är ett enkelrum, sätt max kapacitet till 1
            if (roomType.Equals("Enkelrum", StringComparison.OrdinalIgnoreCase) && capacity > 1)
            {
                Console.WriteLine("För enkelrum kan max kapacitet vara 1 person.");
                return;
            }

            // Om det är en suite, sätt max kapacitet till 4
            if (roomType.Equals("Suite", StringComparison.OrdinalIgnoreCase) && capacity > 4)
            {
                Console.WriteLine("För suite kan max kapacitet vara 4 personer.");
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

        public static void AddBooking()
        {
            try
            {
                // Visar listan över kunder
                using (var context = new HotelDbContext())
                {
                    var customers = context.Customers.ToList();
                    if (customers.Count == 0)
                    {
                        Console.WriteLine("Det finns inga kunder i databasen.");
                        return;
                    }

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
                    if (rooms.Count == 0)
                    {
                        Console.WriteLine("Det finns inga rum i databasen.");
                        return;
                    }

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

                // Kontrollera om datumet är i framtiden
                if (checkIn < DateTime.Now.Date)
                {
                    Console.WriteLine("Du kan inte boka ett rum på ett datum som redan har passerat.");
                    return;
                }

                Console.WriteLine("Ange utcheckningsdatum (yyyy-mm-dd):");
                if (!DateTime.TryParse(Console.ReadLine(), out DateTime checkOut))
                {
                    Console.WriteLine("Ogiltigt utcheckningsdatum.");
                    return;
                }

                // Kontrollera att utcheckningsdatumet inte är tidigare än incheckningsdatumet
                if (checkOut <= checkIn)
                {
                    Console.WriteLine("Utcheckningsdatumet kan inte vara före eller samma som incheckningsdatumet.");
                    return;
                }

                using (var context = new HotelDbContext())
                {
                    // Kontrollera om kunden finns i databasen
                    var customer = context.Customers.SingleOrDefault(c => c.Id == customerId);
                    if (customer == null)
                    {
                        Console.WriteLine("Kund med angivet ID hittades inte.");
                        return;
                    }

                    // Kontrollera om rummet finns i databasen
                    var room = context.Rooms.SingleOrDefault(r => r.Id == roomId);
                    if (room == null)
                    {
                        Console.WriteLine("Rum med angivet ID hittades inte.");
                        return;
                    }

                    // Kontrollera om rummet redan är bokat på de angivna datumen
                    var existingBooking = context.Bookings
                        .Where(b => b.RoomId == roomId &&
                                    ((checkIn >= b.CheckInDate && checkIn < b.CheckOutDate) ||  // Överlappande incheckning
                                     (checkOut > b.CheckInDate && checkOut <= b.CheckOutDate)   // Överlappande utcheckning
                                    ))
                        .FirstOrDefault();

                    if (existingBooking != null)
                    {
                        Console.WriteLine("Rummet är redan bokat på de angivna datumen.");
                        return;
                    }

                    // Skapa bokningen
                    var booking = new Booking
                    {
                        CustomerId = customerId,
                        RoomId = roomId,
                        CheckInDate = checkIn,
                        CheckOutDate = checkOut,
                        BookingDate = DateTime.Now
                    };

                    // Kontrollera att alla nödvändiga värden inte är null innan vi sparar
                    if (booking.CustomerId == 0 || booking.RoomId == 0 || booking.CheckInDate == DateTime.MinValue || booking.CheckOutDate == DateTime.MinValue)
                    {
                        Console.WriteLine("Bokningen innehåller ogiltiga värden.");
                        return;
                    }

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
                    Console.WriteLine($"Inre fel: {ex.InnerException.Message}");
                }
            }
        }

        public static void ShowBookings()
        {
            using (var context = new HotelDbContext())
            {
                var bookings = context.Bookings
                    .Include(b => b.Customer)
                    .Include(b => b.Room)
                    .ToList();

                if (bookings.Count == 0)
                {
                    Console.WriteLine("Inga bokningar hittades.");
                    return;
                }

                foreach (var booking in bookings)
                {
                    Console.WriteLine($"Bokning ID: {booking.Id}, Kund: {booking.Customer.Name}, Rum: {booking.Room.RoomNumber}, Incheckning: {booking.CheckInDate}, Utcheckning: {booking.CheckOutDate}");
                }
            }
        }

        public static void UpdateBooking()
        {
            using (var context = new HotelDbContext())
            {
                var bookings = context.Bookings
                                      .Include(b => b.Customer)
                                      .Include(b => b.Room)
                                      .ToList();

                if (bookings.Count == 0)
                {
                    Console.WriteLine("Det finns inga bokningar att uppdatera.");
                    return;
                }

                Console.WriteLine("Alla bokningar:");
                foreach (var booking in bookings)
                {
                    Console.WriteLine($"Bokning ID: {booking.Id}, Kund: {booking.Customer.Name}, Rum: {booking.Room.RoomNumber}, Incheckning: {booking.CheckInDate}, Utcheckning: {booking.CheckOutDate}");
                }

                Console.WriteLine("Ange bokningens ID för uppdatering:");
                if (!int.TryParse(Console.ReadLine(), out int bookingId))
                {
                    Console.WriteLine("Ogiltigt boknings-ID.");
                    return;
                }

                var bookingToUpdate = bookings.FirstOrDefault(b => b.Id == bookingId);
                if (bookingToUpdate == null)
                {
                    Console.WriteLine("Bokning med angivet ID hittades inte.");
                    return;
                }

                Console.WriteLine("Ange nytt incheckningsdatum (yyyy-mm-dd):");
                if (!DateTime.TryParse(Console.ReadLine(), out DateTime newCheckIn))
                {
                    Console.WriteLine("Ogiltigt incheckningsdatum.");
                    return;
                }

                Console.WriteLine("Ange nytt utcheckningsdatum (yyyy-mm-dd):");
                if (!DateTime.TryParse(Console.ReadLine(), out DateTime newCheckOut))
                {
                    Console.WriteLine("Ogiltigt utcheckningsdatum.");
                    return;
                }

                if (newCheckOut <= newCheckIn)
                {
                    Console.WriteLine("Utcheckningsdatumet kan inte vara före eller samma som incheckningsdatumet.");
                    return;
                }

                // Kontrollera om de nya datumen är samma som de gamla
                if (newCheckIn == bookingToUpdate.CheckInDate && newCheckOut == bookingToUpdate.CheckOutDate)
                {
                    Console.WriteLine("De angivna datumen är samma som de nuvarande.");
                    return;
                }

                var existingBooking = context.Bookings
                    .Where(b => b.RoomId == bookingToUpdate.RoomId &&
                                ((newCheckIn >= b.CheckInDate && newCheckIn < b.CheckOutDate) ||
                                 (newCheckOut > b.CheckInDate && newCheckOut <= b.CheckOutDate)
                                ))
                    .FirstOrDefault();

                if (existingBooking != null)
                {
                    Console.WriteLine("Rummet är redan bokat på de angivna datumen.");
                    return;
                }

                bookingToUpdate.CheckInDate = newCheckIn;
                bookingToUpdate.CheckOutDate = newCheckOut;

                context.SaveChanges();
                Console.WriteLine($"Bokning {bookingId} uppdaterad!");
            }
        }

        public static void DeleteBooking()
        {
            // Implementera delete booking logik här
        }

        public static void ClearAllRoomsAndCustomers()
        {
            // Implementera logik för att rensa alla rum och kunder
        }

        public static void UpdateCustomer()
        {
            // Implementera update customer logik här
        }
    }
}
