using Microsoft.Data.SqlClient;
using Someren_Case.Models;

namespace Someren_Case.Repositories;

public class DbRoomRepository : IRoomRepository
{
    private readonly string _connectionString;

    public DbRoomRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("dbproject242504")
                            ?? throw new ArgumentNullException("Connection string not found!");
    }

    public List<Room> GetAllRooms()
    {
        var rooms = new List<Room>();

        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            var query = "SELECT RoomID, FloorNumber, NumberOfBeds, Building, RoomType FROM Room";
            var command = new SqlCommand(query, connection);
            var reader = command.ExecuteReader();

            while (reader.Read())
                rooms.Add(new Room
                {
                    RoomID = Convert.ToInt32(reader["RoomID"]),
                    FloorNumber = reader["FloorNumber"] as int?,
                    NumberOfBeds = reader["NumberOfBeds"] as int?,
                    Building = reader["Building"].ToString(),
                    RoomType = reader["RoomType"].ToString()
                });
        }

        return rooms;
    }

    public Room GetRoomById(int id)
    {
        Room room = null;

        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            var query = "SELECT RoomID, FloorNumber, NumberOfBeds, Building, RoomType FROM Room WHERE RoomID = @RoomID";
            var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@RoomID", id);
            var reader = command.ExecuteReader();

            if (reader.Read())
                room = new Room
                {
                    RoomID = Convert.ToInt32(reader["RoomID"]),
                    FloorNumber = reader["FloorNumber"] as int?,
                    NumberOfBeds = reader["NumberOfBeds"] as int?,
                    Building = reader["Building"].ToString(),
                    RoomType = reader["RoomType"].ToString()
                };
        }

        return room;
    }

    public void AddRoom(Room room)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            var query =
                "INSERT INTO Room (FloorNumber, NumberOfBeds, Building, RoomType) VALUES (@FloorNumber, @NumberOfBeds, @Building, @RoomType)";
            var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@FloorNumber", room.FloorNumber);
            command.Parameters.AddWithValue("@NumberOfBeds", room.NumberOfBeds);
            command.Parameters.AddWithValue("@Building", room.Building);
            command.Parameters.AddWithValue("@RoomType", room.RoomType);
            command.ExecuteNonQuery();
        }
    }

    public void UpdateRoom(Room room)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            var query =
                "UPDATE Room SET FloorNumber = @FloorNumber, NumberOfBeds = @NumberOfBeds, Building = @Building, RoomType = @RoomType WHERE RoomID = @RoomID";
            var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@RoomID", room.RoomID);
            command.Parameters.AddWithValue("@FloorNumber", room.FloorNumber);
            command.Parameters.AddWithValue("@NumberOfBeds", room.NumberOfBeds);
            command.Parameters.AddWithValue("@Building", room.Building);
            command.Parameters.AddWithValue("@RoomType", room.RoomType);
            command.ExecuteNonQuery();
        }
    }

    public void DeleteRoom(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            var query = "DELETE FROM Room WHERE RoomID = @RoomID";
            var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@RoomID", id);
            command.ExecuteNonQuery();
        }
    }


    public List<Student> GetStudentsWithoutRoom()
    {
        List<Student> students = new List<Student>();

        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            var query =
                "SELECT StudentID, StudentNumber, FirstName, LastName, PhoneNumber, Class FROM Student WHERE StudentID NOT IN (SELECT StudentID FROM Share)";

            var command = new SqlCommand(query, connection);
            var reader = command.ExecuteReader();

            while (reader.Read())
                students.Add(new Student
                {
                    StudentID = Convert.ToInt32(reader["StudentID"]),
                    StudentNumber = reader["StudentNumber"].ToString(),
                    FirstName = reader["FirstName"].ToString(),
                    LastName = reader["LastName"].ToString(),
                    PhoneNumber = reader["PhoneNumber"].ToString(),
                    Class = reader["Class"].ToString()
                });
        }

        return students;
    }
}