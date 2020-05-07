using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RoomBooking.MVC.Models.DbModels;
using RoomBooking.MVC.Services.Interface;

namespace RoomBooking.MVC.Controllers
{
    public class RoomController : Controller
    {
        private readonly IRoomService roomService;
        private readonly UserManager<IdentityUser> userManager;

        public RoomController(IRoomService _roomService)
        {
            roomService = _roomService;
        }
        
        [HttpGet]
        public IActionResult List()
        {
            var allRooms = roomService.GetAll();
            return View(allRooms);
        }

        // [Authorize(Roles = "Admin")]
        public IActionResult AdminList()
        {
            var allRooms = roomService.GetAll();
            return View(allRooms);
        }

        // [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        // [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Add(Room room)
        {
            if (ModelState.IsValid)
            {
                var result = roomService.Create(room);
                if (result)
                {
                    return RedirectToAction("List");
                }
            }
            return View("Add", room);
        }

        // [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            return View(roomService.Get(id));
        }

        // [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Edit(Room room)
        {
            if (ModelState.IsValid)
            {
                //plan.UserId = userManager.GetUserId(HttpContext.User);
                // room
                //plan.UserId = userManager.GetUserId(HttpContext.User);
                roomService.Update(room);
            }

            return RedirectToAction("List");
        }
        // [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Remove(int id)
        {
            roomService.Delete(id);
            return RedirectToAction("List");
        }
    }
}