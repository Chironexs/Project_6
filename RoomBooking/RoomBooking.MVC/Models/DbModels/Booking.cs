using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using RoomBooking.MVC.Models.ViewModels;

namespace RoomBooking.MVC.Models.DbModels
{
    public class Booking
    {
        public int Id { get; set; }
        [ForeignKey("User")] public string UserId { get; set; }
        public User User { get; set; }
        [ForeignKey("Room")] public int RoomId { get; set; }
        public Room Room { get; set; }

        [Required][DataType(DataType.Date)] public DateTime Date { get; set; }
        [Required] public DateTime StarTime { get; set; }

        [Required] public DateTime EndTime { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public bool? AllDay { get; set; }
        public decimal TotalPrice { get; set; }
    }
}