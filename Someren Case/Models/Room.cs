namespace Someren_Case.Models;

public class Room
{
    public int RoomID { get; set; }
    public int? FloorNumber { get; set; }
    public int? NumberOfBeds { get; set; }
    public string Building { get; set; }
    public string RoomType { get; set; }
}