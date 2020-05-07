using System.ComponentModel.DataAnnotations;

namespace RoomBooking.MVC.Models.ViewModels
{
    public class RegisterViewModel
    {
        public string Email { get; set; }
        [Required] public string FirstName { get; set; }
        [Required] public string LastName { get; set; }
        [Required] public string Address { get; set; }
        [Required] public string ZipCode { get; set; }
        [Required] public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Pole hasło jest obowiązkowe")]
        public string Password { get; set; }

        [Required] [Compare("Password")] public string RepeatPassword { get; set; }
    }
}