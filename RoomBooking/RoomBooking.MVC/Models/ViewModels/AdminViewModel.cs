using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoomBooking.MVC.Models.ViewModels
{
    public class AdminViewModel
    {
        public int NumberOfBookings { get; set; }
        public decimal TotalIncome { get; set; }
        public int TotalNumberOfRooms { get; set; }
        public int TotalNumberOfUsers { get; set; }

    }
}
