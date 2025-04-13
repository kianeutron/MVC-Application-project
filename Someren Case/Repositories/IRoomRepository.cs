using Someren_Case.Models;

namespace Someren_Case.Repositories;

public interface IRoomRepository
{
    List<Room> GetAllRooms();
    Room GetRoomById(int id);
    void AddRoom(Room room);
    void UpdateRoom(Room room);
    void DeleteRoom(int id);
}