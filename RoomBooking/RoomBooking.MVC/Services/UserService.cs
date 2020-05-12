using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RoomBooking.MVC.Context;
using RoomBooking.MVC.Models.DbModels;
using RoomBooking.MVC.Services.Interface;

namespace RoomBooking.MVC.Services
{
    public class UserService : IUserService
    {
        private readonly RoomBookingContext context;

        public UserService(RoomBookingContext _context)
        {
            context = _context;
        }

        public IList<User> GetAll()
        {
            return context.Users.ToList();
        }
    }
}
