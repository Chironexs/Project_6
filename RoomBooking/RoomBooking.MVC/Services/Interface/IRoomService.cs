using System.Collections.Generic;
using RoomBooking.MVC.Models.DbModels;

namespace RoomBooking.MVC.Services.Interface
{
    public interface IRoomService
    {
        bool Create(Room room);
        Room Get(int id);
        IList<Room> GetAll();
        IList<Room> GetStartDate(Room room);
        IList<Room> GetStartDate2();
        bool Update(Room room);
        bool Delete(int id);
        int GetTotalNumberOfRooms();
    }
}