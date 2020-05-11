using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RoomBooking.MVC.Models.DbModels;
using RoomBooking.MVC.Models.ViewModels;
using RoomBooking.MVC.Services.Interface;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RoomBooking.MVC.Controllers
{
    public class BookingController : Controller
    {
        private readonly IBookingService bookingService;
        private readonly IRoomService roomService;
        private readonly UserManager<IdentityUser> userManager;

        public BookingController(IBookingService _bookingService, IRoomService _roomService,
            UserManager<IdentityUser> _userManager)
        {
            bookingService = _bookingService;
            userManager = _userManager;
            roomService = _roomService;
        }

        // [Authorize(Roles = "User")]
        [HttpGet]
        public IActionResult Add()
        {
            ViewData["Error"] = "";
            ViewData["Success"] = "";
            BookingViewModel bookingViewModel = new BookingViewModel();

            bookingViewModel.Bookings = bookingService.GetAll();
            bookingViewModel.Rooms = roomService.GetAll();

            string dateTime = DateTime.Now.ToString();
            string createdDate = Convert.ToDateTime(dateTime).ToString("yyyy-MM-dd HH:mm");
            bookingViewModel.StarTime = DateTime.ParseExact(createdDate, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);

            string dateTimePlusDay = DateTime.Now.AddDays(1).ToString();
            string createdDatePlusDay = Convert.ToDateTime(dateTimePlusDay).ToString("yyyy-MM-dd");
            bookingViewModel.Date = DateTime.ParseExact(createdDatePlusDay, "yyyy-MM-dd", CultureInfo.InvariantCulture).Date;
            bookingViewModel.AllDay = "false";

            return View(bookingViewModel);
        }

        // [Authorize(Roles = "User")]
        [HttpPost]
        public IActionResult Add(Booking booking)
        {
            BookingViewModel bookingViewModel = new BookingViewModel();
            bookingViewModel.Rooms = roomService.GetAll();
            if (ModelState.IsValid)
            {

                if (booking.AllDay == "true")
                {
                    booking.UserId = userManager.GetUserId(HttpContext.User);

                    string dateTime = DateTime.Now.ToString();
                    string createdDate = Convert.ToDateTime(dateTime).ToString("yyyy-MM-dd HH:mm");
                    booking.CreatedDateTime = DateTime.ParseExact(createdDate, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);

                    var Date = Convert.ToDateTime(booking.Date).ToString("yyyy-MM-dd");

                    var StartTimeConvert = Convert.ToDateTime($"{Date} 06:00").ToString("yyyy-MM-dd HH:mm");
                    var EndTimeConvert = Convert.ToDateTime($"{Date} 22:00").ToString("yyyy-MM-dd HH:mm");

                    booking.StarTime = DateTime.ParseExact(StartTimeConvert, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
                    booking.EndTime = DateTime.ParseExact(EndTimeConvert, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);

                    bookingViewModel.StarTime = booking.StarTime;
                    bookingViewModel.EndTime = booking.EndTime;
                    bookingViewModel.Date = booking.Date;

                    bookingViewModel.StarTime = booking.StarTime;
                    bookingViewModel.EndTime = booking.EndTime;
                    bookingViewModel.Date = booking.Date;

                    if (!(booking.StarTime >= DateTime.Now))
                    {
                        // ModelState.AddModelError("", "Data jest z przeszłości");
                        // BadRequest(ModelState);
                        ViewData["Success"] = "";
                        ViewData["Error"] = "Nie można dodać rezerwacji z datą w przeszłości";
                        return View("Add", bookingViewModel);
                    }

                    var checkIsOccupied = bookingService.IsOccupied(booking.RoomId, booking.StarTime, booking.EndTime);
                    if (checkIsOccupied)
                    {
                        // ModelState.AddModelError("", "Wybrany termin jest już zajęty");
                        ViewData["Success"] = "";
                        ViewData["Error"] = "Wybrany termin jest już zajęty";

                        return View("Add", bookingViewModel);
                    }

                    TimeSpan timeDifference = booking.EndTime - booking.StarTime;
                    var hours = (decimal) timeDifference.TotalHours;
                    var room = roomService.Get(booking.RoomId);
                    booking.TotalPrice = room.Price * hours;

                    bookingService.Create(booking);

                    ViewData["Success"] = "Dodano nową rezerwację";
                    ViewData["Error"] = "";
                    bookingViewModel.AllDay = "true";
                    return View("Add", bookingViewModel);
                }
                if (booking.EndTime > booking.StarTime)
                {
                    booking.UserId = userManager.GetUserId(HttpContext.User);

                    string dateTime = DateTime.Now.ToString();
                    string createdDate = Convert.ToDateTime(dateTime).ToString("yyyy-MM-dd HH:mm");
                    booking.CreatedDateTime = DateTime.ParseExact(createdDate, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);

                    var Date = Convert.ToDateTime(booking.Date).ToString("yyyy-MM-dd");
                    var StartTime = Convert.ToDateTime(booking.StarTime).ToString("HH:mm");
                    var EndTime = Convert.ToDateTime(booking.EndTime).ToString("HH:mm");

                    var StartTimeConvert = Convert.ToDateTime($"{Date} {StartTime}").ToString("yyyy-MM-dd HH:mm");
                    var EndTimeConvert = Convert.ToDateTime($"{Date} {EndTime}").ToString("yyyy-MM-dd HH:mm");

                    booking.StarTime = DateTime.ParseExact(StartTimeConvert, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
                    booking.EndTime = DateTime.ParseExact(EndTimeConvert, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);

                    bookingViewModel.StarTime = booking.StarTime;
                    bookingViewModel.EndTime = booking.EndTime;
                    bookingViewModel.Date = booking.Date;
                    
                    if (!(booking.StarTime >= DateTime.Now))
                    {
                        // ModelState.AddModelError("", "Data jest z przeszłości");
                        // BadRequest(ModelState);
                        ViewData["Success"] = "";
                        ViewData["Error"] = "Nie można dodać rezerwacji z datą w przeszłości";
                        return View("Add", bookingViewModel);
                    }

                    var checkIsOccupied = bookingService.IsOccupied(booking.RoomId, booking.StarTime, booking.EndTime);
                    if (checkIsOccupied)
                    {
                        // ModelState.AddModelError("", "Wybrany termin jest już zajęty");
                        ViewData["Success"] = "";
                        ViewData["Error"] = "Wybrany termin jest już zajęty";
                        return View("Add", bookingViewModel);
                    }

                    TimeSpan timeDifference = booking.EndTime - booking.StarTime;
                    var hours = (decimal) timeDifference.TotalHours;
                    var room = roomService.Get(booking.RoomId);
                    booking.TotalPrice = room.Price * hours;

                    bookingService.Create(booking);

                    ViewData["Success"] = "Dodano nową rezerwację";
                    ViewData["Error"] = "";
                    return View("Add", bookingViewModel);
                }
                ViewData["Success"] = "";
                ViewData["Error"] = "Niepoprawna godzina zakończenia";
                return View("Add", bookingViewModel);
            }
            ViewData["Success"] = "";
            ViewData["Error"] = "Niepoprawne dane";
            return View("Add", bookingViewModel);
        }

        // [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult AdminList()
        {
            BookingViewModel bookingViewModel = new BookingViewModel
            {
                Bookings = bookingService.GetAll(),
                Rooms = roomService.GetAll()
            };
            User user = new User();
            user.Bookings = bookingService.GetAll();
            return View(user);
        }

        // [Authorize(Roles = "User")]
        [HttpGet]
        public IActionResult List()
        {
            ViewData["Error"] = "";
            ViewData["Success"] = "";
            var userId = userManager.GetUserId(HttpContext.User);
            BookingViewModel bookingViewModel = new BookingViewModel
            {
                Bookings = bookingService.GetUserBooking(userId),
                Rooms = roomService.GetAll()
            };
            return View(bookingViewModel);
        }

        // [Authorize(Roles = "User")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            ViewData["Error"] = "";
            ViewData["Success"] = "";
            BookingViewModel bookingViewModel = new BookingViewModel();
            IdentityUser identityUser = new IdentityUser();
            identityUser.Id = userManager.GetUserId(HttpContext.User);
            if (bookingService.Get(id).UserId == userManager.GetUserId(HttpContext.User) ||
                await userManager.IsInRoleAsync(identityUser, "Admin"))
            {
                var bookingObject = bookingService.Get(id);
                bookingViewModel.booking = bookingService.Get(id);
                bookingViewModel.Rooms = roomService.GetAll();
                bookingViewModel.room = roomService.Get(bookingObject.RoomId);
                return View(bookingViewModel);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(BookingViewModel bookingViewModel)
        {
            IdentityUser identityUser = new IdentityUser();
            identityUser.Id = userManager.GetUserId(HttpContext.User);
            var userID = bookingService.Get(bookingViewModel.Id).UserId;
            if (userID == identityUser.Id || await userManager.IsInRoleAsync(identityUser, "Admin"))
            {
                var bookingId = bookingViewModel.booking.Id;
                var bookingObject = bookingService.Get(bookingId);
                bookingViewModel.Rooms = roomService.GetAll();
                bookingViewModel.room = roomService.Get(bookingObject.RoomId);

                if (ModelState.IsValid)
                {
                    Booking booking = new Booking();
                    booking.Id = bookingViewModel.Id;

                    var Date = Convert.ToDateTime(bookingViewModel.booking.Date).ToString("yyyy-MM-dd");
                    var StartTime = Convert.ToDateTime(bookingViewModel.booking.StarTime).ToString("HH:mm");
                    var EndTime = Convert.ToDateTime(bookingViewModel.booking.EndTime).ToString("HH:mm");

                    var StartTimeConvert = Convert.ToDateTime($"{Date} {StartTime}").ToString("yyyy-MM-dd HH:mm");
                    var EndTimeConvert = Convert.ToDateTime($"{Date} {EndTime}").ToString("yyyy-MM-dd HH:mm");

                    booking.StarTime = DateTime.ParseExact(StartTimeConvert, "yyyy-MM-dd HH:mm",
                        CultureInfo.InvariantCulture);
                    booking.EndTime =
                        DateTime.ParseExact(EndTimeConvert, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);

                    if (booking.EndTime > booking.StarTime)
                    {
                        booking.Date = bookingViewModel.booking.Date;
                        booking.RoomId = bookingViewModel.room.Id;
                        booking.CreatedDateTime = DateTime.Now;

                        var checkIsOccupied = bookingService.IsOccupiedEditExisting(booking.Id, booking.RoomId, booking.StarTime, booking.EndTime);
                        if (checkIsOccupied)
                        {
                            ViewData["Success"] = "";
                            ViewData["Error"] = "Wybrany termin jest już zajęty";
                            return View("Edit", bookingViewModel);
                        }

                        if (!(booking.StarTime >= DateTime.Now))
                        {
                            ViewData["Success"] = "";
                            ViewData["Error"] = "Nie można dodać rezerwacji z datą w przeszłości";
                            return View("Edit", bookingViewModel);
                        }

                        TimeSpan timeDifference = booking.EndTime - booking.StarTime;
                        var hours = (decimal) timeDifference.TotalHours;
                        var room = roomService.Get(booking.RoomId);
                        booking.TotalPrice = room.Price * hours;

                        booking.UserId = userID;
                        bookingService.Update(booking);
                        
                        ViewData["Success"] = "Zmieniono rezerwację";
                        ViewData["Error"] = "";
                        return RedirectToAction("List");
                    }
                    ViewData["Success"] = "";
                    ViewData["Error"] = "Niepoprawna godzina zakończenia";
                    return View("Edit", bookingViewModel);
                }

                ViewData["Success"] = "";
                ViewData["Error"] = "Niepoprawne dane";
                return View("Edit", bookingViewModel);
            }

            return RedirectToAction("List");
        }


        //admin wszystkie
        // [Authorize(Roles = "User")]
        // [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Remove(int id)
        {
            var a = bookingService.Get(id);
            if (a.Date == DateTime.Now)
            {
                ModelState.AddModelError("", "Nie można usunąć rezerwacji będącej w tym samym dniu");
                return RedirectToAction("Index", "Home");
            }
            if (bookingService.Get(id).UserId == userManager.GetUserId(HttpContext.User))
            {
                bookingService.Delete(id);
               // return RedirectToAction("List");
            }

            IdentityUser identityUser = new IdentityUser();
            identityUser.Id = userManager.GetUserId(HttpContext.User);

            if (await userManager.IsInRoleAsync(identityUser, "Admin"))
            {
                bookingService.Delete(id);
                return RedirectToAction("AdminList");
            }

            return RedirectToAction("List");
        }


        // do widoku kalendarza 
        [HttpGet]
        public IActionResult Calendar()
        {
            BookingViewModel bookingViewModel = new BookingViewModel();
            bookingViewModel.Rooms = roomService.GetAll();
            return View(bookingViewModel);
        }

        [HttpPost]
        public IActionResult Calendar(int id)
        {
            BookingViewModel bookingViewModel = new BookingViewModel();
            bookingViewModel.Bookings = bookingService.GetRoomBookings(id);

            Booking booking = new Booking();

            return View(bookingViewModel);
        }


        // GET
        [Route("/[controller]/[action]/{id}")]
        [HttpGet("{id}")]
        public IActionResult CalendarGet(int id)
        {
            BookingViewModel bookingViewModel = new BookingViewModel();
            bookingViewModel.Rooms = roomService.GetAll();
            if (id == 0)
            {
                bookingViewModel.Bookings = bookingService.GetAll();
                List<object> listAll = new List<object>();
                foreach (var item in bookingViewModel.Bookings)
                {
                    Booking booking = new Booking();
                    booking.Id = item.Id;
                    booking.StarTime = item.StarTime;
                    booking.EndTime = item.EndTime;
                    booking.RoomId = item.RoomId;
                    booking.UserId = item.Room.Name;
                    booking.RoomId = item.Room.Id;
                    //                booking.Room = item.Room;
                    //                booking.Room.Name = item.Room.Name;
                    listAll.Add(booking);
                }
                return Ok(listAll);
            }

            bookingViewModel.Bookings = bookingService.GetRoomBookings(id);
            List<object> listId = new List<object>();
            foreach (var item in bookingViewModel.Bookings)
            {
                Booking booking = new Booking();
                booking.Id = item.Id;
                booking.StarTime = item.StarTime;
                booking.EndTime = item.EndTime;
                booking.RoomId = item.RoomId;
                booking.UserId = item.Room.Name;
                booking.RoomId = item.Room.Id;
                //                booking.Room = item.Room;
                //                booking.Room.Name = item.Room.Name;
                listId.Add(booking);
            }
            return Ok(listId);
        }

        [NonAction]
        public bool Validation(Booking booking)
        {
            return true;
        }
    }
}