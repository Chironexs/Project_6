using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RoomBooking.MVC.Models.DbModels;

namespace RoomBooking.MVC.Context
{
    public class RoomBookingContext : IdentityDbContext
    {
        public RoomBookingContext(DbContextOptions<RoomBookingContext> options) : base(options)
        {
        }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<User> Users { get; set; }

    }
}
