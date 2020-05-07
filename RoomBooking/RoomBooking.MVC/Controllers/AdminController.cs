using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RoomBooking.MVC.Context;
using RoomBooking.MVC.Models.DbModels;
using RoomBooking.MVC.Models.ViewModels;
using RoomBooking.MVC.Services.Interface;

namespace RoomBooking.MVC.Controllers
{
    public class AdminController : Controller
    {
        private readonly IBookingService bookingService;
        private readonly IRoomService roomService;
        private readonly UserManager<IdentityUser> userManager;

        public AdminController(IBookingService _bookingService, IRoomService _roomService,
            UserManager<IdentityUser> _userManager)
        {
            bookingService = _bookingService;
            userManager = _userManager;
            roomService = _roomService;
        }

        //statystyki
        public IActionResult Index()
        {
            AdminViewModel adminViewModel = new AdminViewModel();
            adminViewModel.NumberOfBookings = bookingService.GetNumberOfBookings();
            adminViewModel.TotalIncome = bookingService.GetTotalIncome();
            adminViewModel.TotalNumberOfUsers = userManager.Users.Count();
            adminViewModel.TotalNumberOfRooms = roomService.GetTotalNumberOfRooms();
            return View(adminViewModel);
        }

        // użytkownicy
        public IActionResult UserList()
        {
            return View(userManager.Users);
        }

        // do przeniesiena do index
        public IActionResult Chart()
        {
            var chart = bookingService.GetAllToalPrice();
            var chart2 = bookingService.GetAll(); 
            var chart3 = bookingService.GetStartDate(); // zwraca IList<booking>
            return View("Chart", chart);
        }


        //pomocnicza zmienna
        [NonAction]
        public IActionResult Chart1()
        {
            var chart = bookingService.GetAllToalPrice(); // zwraca IList<booking>
            var chart2 = bookingService.GetAll(); // zwraca IList<booking>
            var chart3 = bookingService.GetStartDate(); // zwraca IList<booking>
            return View("Chart", chart3);
        }
    }
}