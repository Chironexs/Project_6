using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RoomBooking.MVC.Context;
using RoomBooking.MVC.Models.DbModels;
using RoomBooking.MVC.Services.Interface;

namespace RoomBooking.MVC.Services
{
    public class RoomService : IRoomService
    {
        private readonly RoomBookingContext context;

        public RoomService(RoomBookingContext _context)
        {
            context = _context;
        }

        public bool Create(Room room)
        {
            context.Rooms.Add(room);
            return context.SaveChanges() > 0;
        }

        public Room Get(int id)
        {
            return context.Rooms.SingleOrDefault(a => a.Id == id);
        }

        public IList<Room> GetAll()
        {
            return context.Rooms.ToList();
        }

        // nie używane
        public IList<Room> GetStartDate(Room room)
        {
            return context.Rooms.Include(a => a.Bookings).ThenInclude(a => a.StarTime).Where(a => a.Id == room.Id).ToList();
        }

        // nie używane
        public IList<Room> GetStartDate2()
        {
            return context.Rooms.Include(a => a.Bookings).ThenInclude(a => a.StarTime).ToList();
        }

        public bool Update(Room room)
        {
            context.Rooms.Update(room);
            return context.SaveChanges() > 0;
        }

        public bool Delete(int id)
        {
            var book = context.Rooms.SingleOrDefault(a => a.Id == id);
            if (book == null)
            {
                return false;
            }
            context.Rooms.Remove(book);
            return context.SaveChanges() > 0;
        }
        public int GetTotalNumberOfRooms()
        {
            return context.Rooms.Count();
        }
    }
}