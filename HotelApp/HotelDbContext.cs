using Microsoft.EntityFrameworkCore;
using HotelApp.Models;

namespace HotelApp
{
    public class HotelDbContext : DbContext
    {
        // tabellerna i databasen
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        //  databasen och anslutning
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Ansluter till SQL Server 
            optionsBuilder.UseSqlServer(
                "Server=localhost;Database=HotelDB;Trusted_Connection=True;TrustServerCertificate=True;",
                options => options.EnableRetryOnFailure());
        }
    }
}

