using System.Data.SqlClient;
using Someren_Case.Models;

namespace Someren_Case.Repositories;

public class DbActivityRepository : IActivityRepository
{
    private readonly string _connectionString;

    public DbActivityRepository(string connectionString)
    {
        _connectionString = connectionString;
    }


    public List<Activity> GetAll()
    {
        List<Activity> activities = new List<Activity>();

        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            var query = "SELECT ActivityID, ActivityName, Date, StartTime, EndTime FROM Activity";

            using (var command = new SqlCommand(query, connection))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                    activities.Add(new Activity
                    {
                        ActivityID = reader.GetInt32(0),
                        ActivityName = reader.GetString(1),
                        Date = reader.GetDateTime(2),
                        StartTime = reader.GetTimeSpan(3),
                        EndTime = reader.GetTimeSpan(4)
                    });
            }
        }

        return activities;
    }


    public Activity GetById(int activityId)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            var query =
                "SELECT ActivityID, ActivityName, Date, StartTime, EndTime FROM Activity WHERE ActivityID = @ActivityID";

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ActivityID", activityId);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                        return new Activity
                        {
                            ActivityID = reader.GetInt32(0),
                            ActivityName = reader.GetString(1),
                            Date = reader.GetDateTime(2),
                            StartTime = reader.GetTimeSpan(3),
                            EndTime = reader.GetTimeSpan(4)
                        };
                }
            }
        }

        return null;
    }


    public void Add(Activity activity)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            var query =
                "INSERT INTO Activity (ActivityName, Date, StartTime, EndTime) VALUES (@ActivityName, @Date, @StartTime, @EndTime)";

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ActivityName", activity.ActivityName);
                command.Parameters.AddWithValue("@Date", activity.Date);
                command.Parameters.AddWithValue("@StartTime", activity.StartTime);
                command.Parameters.AddWithValue("@EndTime", activity.EndTime);

                command.ExecuteNonQuery();
            }
        }
    }


    public void Update(Activity activity)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            var query =
                "UPDATE Activity SET ActivityName = @ActivityName, Date = @Date, StartTime = @StartTime, EndTime = @EndTime WHERE ActivityID = @ActivityID";

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ActivityID", activity.ActivityID);
                command.Parameters.AddWithValue("@ActivityName", activity.ActivityName);
                command.Parameters.AddWithValue("@Date", activity.Date);
                command.Parameters.AddWithValue("@StartTime", activity.StartTime);
                command.Parameters.AddWithValue("@EndTime", activity.EndTime);

                command.ExecuteNonQuery();
            }
        }
    }


    public void Delete(Activity activity)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            var query = "DELETE FROM Activity WHERE ActivityID = @ActivityID";

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ActivityID", activity.ActivityID);
                command.ExecuteNonQuery();
            }
        }
    }


    public void AddParticipantToActivity(int studentId, int activityId)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            var query = "INSERT INTO ActivityParticipant (StudentID, ActivityID) VALUES (@StudentID, @ActivityID)";

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@StudentID", studentId);
                command.Parameters.AddWithValue("@ActivityID", activityId);
                command.ExecuteNonQuery();
            }
        }
    }


    public void RemoveParticipantFromActivity(int studentId, int activityId)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            var query = "DELETE FROM ActivityParticipant WHERE StudentID = @StudentID AND ActivityID = @ActivityID";

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@StudentID", studentId);
                command.Parameters.AddWithValue("@ActivityID", activityId);
                command.ExecuteNonQuery();
            }
        }
    }


    public List<Student> GetStudentsInActivity(int activityId)
    {
        List<Student> students = new List<Student>();

        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            var query =
                "SELECT s.StudentID, s.StudentNumber, s.FirstName, s.LastName, s.PhoneNumber, s.Class FROM Student s " +
                "JOIN ActivityParticipant ap ON s.StudentID = ap.StudentID " +
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
                            StudentNumber = reader.GetString(1),
                            FirstName = reader.GetString(2),
                            LastName = reader.GetString(3),
                            PhoneNumber = reader.GetString(4),
                            Class = reader.GetString(5)
                        });
                }
            }
        }

        return students;
    }
}