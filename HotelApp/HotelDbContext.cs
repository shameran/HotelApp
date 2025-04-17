using Microsoft.EntityFrameworkCore;
using HotelApp.Models;

namespace HotelApp
{
    public class HotelDbContext : DbContext
    {
        // Tabellerna i databasen
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        // Databasen och anslutning
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Server=localhost;Database=HotelDB;Trusted_Connection=True;TrustServerCertificate=True;",
                options => options.EnableRetryOnFailure());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Lägg till rum
            modelBuilder.Entity<Room>().HasData(
                new Room { Id = 1, RoomNumber = "01", RoomType = "Double", PricePerNight = 151, Capacity = 2 },
                new Room { Id = 2, RoomNumber = "02", RoomType = "Single", PricePerNight = 101, Capacity = 1 },
                new Room { Id = 3, RoomNumber = "03", RoomType = "Single", PricePerNight = 101, Capacity = 1 },
                new Room { Id = 4, RoomNumber = "04", RoomType = "Suite", PricePerNight = 251, Capacity = 4 }
            );

            // Lägg till kunder
            modelBuilder.Entity<Customer>().HasData(
                new Customer { Id = 1, FirstName = "John", LastName = "Doe", Email = "johndoe@example.com", City = "Stockholm" },
                new Customer { Id = 2, FirstName = "Jane", LastName = "Smith", Email = "janesmith@example.com", City = "Göteborg" },
                new Customer { Id = 3, FirstName = "Alice", LastName = "Johnson", Email = "alicejohnson@example.com", City = "Malmö" },
                new Customer { Id = 4, FirstName = "Bob", LastName = "Brown", Email = "bobbrown@example.com", City = "Uppsala" }
            );
        }
    }
}