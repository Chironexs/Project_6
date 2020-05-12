using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RoomBooking.MVC.Models.DbModels;

namespace RoomBooking.MVC.Services.Interface
{
    public interface IUserService
    {
        IList<User> GetAll();
    }
}
