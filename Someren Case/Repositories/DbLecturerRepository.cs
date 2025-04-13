using Microsoft.Data.SqlClient;
using Someren_Case.Interfaces;
using Someren_Case.Models;

namespace Someren_Case.Repositories;

public class DbLecturerRepository : ILecturerRepository
{
    private readonly string _connectionString;

    public DbLecturerRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public List<Lecturer> GetAllLecturers()
    {
        var lecturers = new List<Lecturer>();
        using (var conn = new SqlConnection(_connectionString))
        {
            conn.Open();
            var sql = "SELECT LecturerID, FirstName, LastName, PhoneNumber, DateOfBirth FROM Lecturer";
            using (var cmd = new SqlCommand(sql, conn))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                    lecturers.Add(new Lecturer
                    {
                        LecturerID = reader.GetInt32(0),
                        FirstName = reader.GetString(1),
                        LastName = reader.GetString(2),
                        PhoneNumber = reader.GetString(3),
                        DateOfBirth = reader.GetDateTime(4)
                    });
            }
        }

        return lecturers;
    }

    public Lecturer GetLecturerById(int id)
    {
        Lecturer lecturer = null;
        using (var conn = new SqlConnection(_connectionString))
        {
            conn.Open();
            var sql =
                "SELECT LecturerID, FirstName, LastName, PhoneNumber, DateOfBirth FROM Lecturer WHERE LecturerID = @Id";
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                        lecturer = new Lecturer
                        {
                            LecturerID = reader.GetInt32(0),
                            FirstName = reader.GetString(1),
                            LastName = reader.GetString(2),
                            PhoneNumber = reader.GetString(3),
                            DateOfBirth = reader.GetDateTime(4)
                        };
                }
            }
        }

        return lecturer;
    }

    public void AddLecturer(Lecturer lecturer)
    {
        using (var conn = new SqlConnection(_connectionString))
        {
            conn.Open();
            var sql =
                "INSERT INTO Lecturer (FirstName, LastName, PhoneNumber, DateOfBirth) VALUES (@FirstName, @LastName, @PhoneNumber, @DateOfBirth)";
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@FirstName", lecturer.FirstName);
                cmd.Parameters.AddWithValue("@LastName", lecturer.LastName);
                cmd.Parameters.AddWithValue("@PhoneNumber", lecturer.PhoneNumber);
                cmd.Parameters.AddWithValue("@DateOfBirth", lecturer.DateOfBirth);
                cmd.ExecuteNonQuery();
            }
        }
    }

    public void UpdateLecturer(Lecturer lecturer)
    {
        using (var conn = new SqlConnection(_connectionString))
        {
            conn.Open();
            var sql =
                "UPDATE Lecturer SET FirstName = @FirstName, LastName = @LastName, PhoneNumber = @PhoneNumber, DateOfBirth = @DateOfBirth WHERE LecturerID = @LecturerID";
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@FirstName", lecturer.FirstName);
                cmd.Parameters.AddWithValue("@LastName", lecturer.LastName);
                cmd.Parameters.AddWithValue("@PhoneNumber", lecturer.PhoneNumber);
                cmd.Parameters.AddWithValue("@DateOfBirth", lecturer.DateOfBirth);
                cmd.Parameters.AddWithValue("@LecturerID", lecturer.LecturerID);
                cmd.ExecuteNonQuery();
            }
        }
    }

    public void DeleteLecturer(int id)
    {
        using (var conn = new SqlConnection(_connectionString))
        {
            conn.Open();
            var sql = "DELETE FROM Lecturer WHERE LecturerID = @Id";
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();
            }
        }
    }
}