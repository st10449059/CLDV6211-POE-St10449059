using Microsoft.EntityFrameworkCore;
using CLDV6211_Assignment_Part_1_St10449059.Models;

namespace CLDV6211_Assignment_Part_1_St10449059.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Venue> Venues { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        // NEW: EventTypes Table
        public DbSet<EventType> EventTypes { get; set; }

        // NEW: Seed the database with lookup values
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EventType>().HasData(
                new EventType { EventTypeId = 1, Name = "Concert" },
                new EventType { EventTypeId = 2, Name = "Conference" },
                new EventType { EventTypeId = 3, Name = "Gala" },
                new EventType { EventTypeId = 4, Name = "Exhibition" }
            );
        }
    }
}
