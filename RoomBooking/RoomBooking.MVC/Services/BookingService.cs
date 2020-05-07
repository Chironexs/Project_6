using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RoomBooking.MVC.Context;
using RoomBooking.MVC.Models.DbModels;
using RoomBooking.MVC.Models.ViewModels;
using RoomBooking.MVC.Services.Interface;

namespace RoomBooking.MVC.Services
{
    public class BookingService : IBookingService
    {
        private readonly RoomBookingContext context;

        public BookingService(RoomBookingContext _context)
        {
            context = _context;
        }

        public bool Create(Booking booking)
        {
            context.Bookings.Add(booking);
            return context.SaveChanges() > 0;
        }

        public Booking Get(int id)
        {
            return context.Bookings.Include(c => c.User).SingleOrDefault(a => a.Id == id);
        }

        public IList<Booking> GetUserBooking(string id)
        {
            return context.Bookings.Where(a => a.UserId == id).OrderBy(a => a.Date).ToList();
        }

        public IList<Booking> GetAll()
        {
            return context.Bookings.Include(c => c.User).OrderBy(a => a.Date).ToList();
        }

        public IList<Booking> GetAllToalPrice()
        {
            return context.Bookings
                .GroupBy(i => new {Y = i.Date}, i => i.TotalPrice)
                .Select(g => new Booking {Date = g.Key.Y, TotalPrice = g.Sum(s => s)})
                .OrderBy(i => i.Date)
                .ThenByDescending(i => i.TotalPrice)
                .ToList();
        }

        public bool Update(Booking booking)
        {
            var cache = context.Bookings.FirstOrDefault(o => o.Id == booking.Id);
            if (cache != null)
            {
                context.Entry(cache).CurrentValues.SetValues(booking);
                context.SaveChanges();
                return true;
            }

            return false;
        }

        public bool Delete(int id)
        {
            var booking = context.Bookings.FirstOrDefault(o => o.Id == id);
            if (booking != null)
            {
                context.Bookings.Remove(booking);
                context.SaveChanges();
                return true;
            }

            return false;
        }

        public bool IsOccupied(int roomId, DateTime newStartTime, DateTime newEndTime)
        {
            var checkIsOccupied = context.Bookings.Where(a => a.RoomId == roomId).Any(a => !(a.EndTime <= newStartTime) && !(a.StarTime >= newEndTime)); 
            return checkIsOccupied;
        }

        public bool IsOccupiedEditExisting(int bookingId, int roomId, DateTime newStartTime, DateTime newEndTime)
        {
            var checkIsOccupied = context.Bookings.Where(a => a.RoomId == roomId).Where(c => c.Id != bookingId).Any(a => !(a.EndTime <= newStartTime) && !(a.StarTime >= newEndTime));
            return checkIsOccupied;
        }


        public int GetNumberOfBookings()
        {
            return context.Bookings.Count();
        }

        public decimal GetTotalIncome()
        {
            return context.Bookings.Sum(i => i.TotalPrice);
        }

        // do usunięcia
        public IList<Booking> GetStartDate()
        {
            var asBookings = context.Bookings.OrderBy(a => a.StarTime).ToList();
            return asBookings;

            //return context.Bookings.Where(a => a.Id == id).OrderBy(a => a.StarTime).ToList();

            // return context.Bookings
            //                .GroupBy(i => new {Y = i.StarTime, M = i.EndTime}, i => i.TotalPrice)
            //                .Select(g => new Booking {StarTime = g.Key.Y, EndTime = g.Key.M, TotalPrice = g.Sum(s => s)})
            //                .OrderBy(i => i.StarTime)
            //                .ThenBy(i => i.EndTime)
            //                .ThenByDescending(i => i.TotalPrice)
            //                .ToList();
        }
    }
}