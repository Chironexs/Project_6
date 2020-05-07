using System.ComponentModel.DataAnnotations;

namespace RoomBooking.MVC.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Pole email jest obowiązkowe")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Pole hasło jest obowiązkowe")]
        public string Password { get; set; }
    }
}