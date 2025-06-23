using Microsoft.EntityFrameworkCore;
using MVCEventEaseApp.Models;

namespace MVCEventEaseApp.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        public DbSet<Venues> Venues { get; set; }
        public DbSet<Events> Events { get; set; }
        public DbSet<Bookings> Bookings { get; set; }
        public DbSet<EventType> EventTypes { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Bookings>()
        .HasOne(b => b.Venue)
        .WithMany()
        .HasForeignKey(b => b.VenueID)
        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Venues>()
                .HasData(new Venues { VenueID=1,
                    VenueName = "Grand Hall",
                    Locations = "123 Main St, Cityville",
                    Capacity = "500",
                    ImageURL = "https://example.com/images/grandhall.jpg"
                });
             
            modelBuilder.Entity<Venues>().HasData(new Venues
            {
                VenueID = 2,
                VenueName = "City Conference Center",
                Locations = "456 Elm St, Cityville",
                Capacity = "300",
                ImageURL = "https://example.com/images/cityconference.jpg"
            });

            modelBuilder.Entity<Events>()
                .HasData(new Events
                {
                    EventID = 1,
                    EventName = "Tech Conference 2023",
                    EventDate = new DateTime(2023, 10, 15),
                    Description = "A conference for tech enthusiasts.",
                    VenueID = 1
                });
            modelBuilder.Entity<Events>().HasData(new Events {
                VenueID = 2,
                EventID = 2,
                EventName = "Business Summit 2023",
                EventDate = new DateTime(2023, 11, 20),
                Description = "A summit for business leaders."
            });
            modelBuilder.Entity<Bookings>()
                .HasData(new Bookings
                {
                    BookingID = 1,
                    EventID = 1,
                    VenueID = 1,
                    BookingDate = new DateTime(2023, 10, 1)
                });
            modelBuilder.Entity<Bookings>().HasData(new Bookings {
                BookingID = 2,
                VenueID = 2,
                EventID = 2,
                
                BookingDate = new DateTime(2023, 10, 5)
            });
            modelBuilder.Entity<EventType>().HasData(
    new EventType { Id = 1, Name = "Conference" },
    new EventType { Id = 2, Name = "Wedding" },
    new EventType { Id = 3, Name = "Expo" },
    new EventType { Id = 4, Name = "Workshop" }
);


        }
    }
}
