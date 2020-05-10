using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RoomBooking.MVC.Models.DbModels;

namespace RoomBooking.MVC.Services.Interface
{
    public interface IBookingService
    {
        bool Create(Booking booking);
        Booking Get(int id);
        IList<Booking> GetUserBooking(string id);
        IList<Booking> GetAllToalPrice();
        IList<Booking> GetStartDate(); //
        IList<Booking> GetAll();
        IList<Booking> GetRoomBookings(int roomId);
        bool Update(Booking room);
        bool Delete(int id);
        bool IsOccupied( int roomId, DateTime newStartTime, DateTime newEndTime);
        bool IsOccupiedEditExisting(int bookingId, int roomId, DateTime newStartTime, DateTime newEndTime);
        int GetNumberOfBookings();
        decimal GetTotalIncome();
    }
}