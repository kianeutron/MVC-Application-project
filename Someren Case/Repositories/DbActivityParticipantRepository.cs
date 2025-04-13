using System.Data.SqlClient;
using Someren_Case.Models;

namespace Someren_Case.Repositories;

public class DbActivityParticipantRepository : IActivityParticipantRepository
{
    private readonly string _connectionString;

    public DbActivityParticipantRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public List<Student> GetParticipantsByActivityId(int activityId)
    {
        List<Student> students = new List<Student>();

        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            var query = "SELECT s.StudentID, s.FirstName, s.LastName FROM Student s " +
                        "JOIN Participate ap ON s.StudentID = ap.StudentID " +
                        "WHERE ap.ActivityID = @ActivityID";
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ActivityID", activityId);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                        students.Add(new Student
                        {
                            StudentID = reader.GetInt32(0),
                            FirstName = reader.GetString(1),
                            LastName = reader.GetString(2)
                        });
                }
            }
        }

        return students;
    }

    public List<Student> GetNonParticipantsByActivityId(int activityId)
    {
        List<Student> students = new List<Student>();

        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            var query = "SELECT s.StudentID, s.FirstName, s.LastName FROM Student s " +
                        "WHERE s.StudentID NOT IN (SELECT StudentID FROM Participate WHERE ActivityID = @ActivityID)";
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ActivityID", activityId);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                        students.Add(new Student
                        {
                            StudentID = reader.GetInt32(0),
                            FirstName = reader.GetString(1),
                            LastName = reader.GetString(2)
                        });
                }
            }
        }

        return students;
    }

    public void AddParticipant(int activityId, int studentId)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            var query = "INSERT INTO Participate (ActivityID, StudentID) VALUES (@ActivityID, @StudentID)";
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ActivityID", activityId);
                command.Parameters.AddWithValue("@StudentID", studentId);
                command.ExecuteNonQuery();
            }
        }
    }

    public void RemoveParticipant(int activityId, int studentId)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            var query = "DELETE FROM Participate WHERE ActivityID = @ActivityID AND StudentID = @StudentID";
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ActivityID", activityId);
                command.Parameters.AddWithValue("@StudentID", studentId);
                command.ExecuteNonQuery();
            }
        }
    }
}