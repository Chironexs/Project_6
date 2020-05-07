using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace RoomBooking.MVC.Models.DbModels
{
    public class User : IdentityUser
    {
        [Required] public string FirstName { get; set; }
        [Required] public string LastName { get; set; }
        [Required] public string Address { get; set; }

        [Required] public string ZipCode { get; set; }
        public ICollection<User> Users { get; set; }
        public ICollection<Booking> Bookings { get; set; }
    }
}