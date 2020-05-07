using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using RoomBooking.MVC.Models.DbModels;

namespace RoomBooking.MVC.Models.ViewModels
{
    public class BookingViewModel : Booking
    {
        public ICollection<Room> Rooms { get; set; }
        public ICollection<Booking> Bookings { get; set; }

        public Room room { get; set; }
        public Booking booking { get; set; }

        public ICollection<SelectListItem> ListOfTimeIntervals
        {
            get
            {
                List <SelectListItem> list = new List<SelectListItem>();
                // range of hours, multiplied by 4 (e.g. 24 hours = 96)
                //int timeRange = 96;
                int timeRange = 33;

                // range of minutes, e.g. 15 min
                //int minuteRange = 15;
                int minuteRange = 30;

                // starting time, e.g. 0:00
                TimeSpan startTime = new TimeSpan(6, 0, 0);

                // placeholder
                list.Add(
                    new SelectListItem
                    {
                        Text = "Wybierz godzinę", 
                        Value = "0", 
                        Disabled = true
                    });

                // get standard ticks
                DateTime startDate = new DateTime(DateTime.MinValue.Ticks);

                // create time format based on range above
                for (int i = 0; i < timeRange; i++)
                {
                    int minutesAdded = minuteRange * i;
                    TimeSpan timeAdded = new TimeSpan(0, minutesAdded, 0);
                    TimeSpan tm = startTime.Add(timeAdded);
                    DateTime result = startDate + tm;

                    list.Add(
                        new SelectListItem
                        {
                            Text = result.ToString("HH:mm"), 
                            Value = result.ToString("HH:mm")
                        });
                }

                return list;
            }
        }

    }
}