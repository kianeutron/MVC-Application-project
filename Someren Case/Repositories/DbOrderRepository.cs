using Microsoft.Data.SqlClient;
using Someren_Case.Models;

public class DbOrderRepository : IOrderRepository
{
    private readonly string _connectionString;

    public DbOrderRepository(string connectionString)
    {
        _connectionString = connectionString;
    }


    public void AddOrder(Order order)
    {
        using (var conn = new SqlConnection(_connectionString))
        {
            var query =
                "INSERT INTO [OrderTable] (StudentID, DrinkID, Quantity) VALUES (@studentId, @drinkId, @quantity)";
            var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@studentId", order.StudentID);
            cmd.Parameters.AddWithValue("@drinkId", order.DrinkID);
            cmd.Parameters.AddWithValue("@quantity", order.Quantity);

            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }

    public List<Order> GetAllOrders()
    {
        List<Order> orders = new List<Order>();

        using (var conn = new SqlConnection(_connectionString))
        {
            var query = "SELECT OrderID, StudentID, DrinkID, Quantity FROM [OrderTable]";
            var cmd = new SqlCommand(query, conn);

            conn.Open();
            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                var order = new Order
                {
                    OrderID = reader.GetInt32(0),
                    StudentID = reader.GetInt32(1),
                    DrinkID = reader.GetInt32(2),
                    Quantity = reader.GetInt32(3)
                };

                orders.Add(order);
            }
        }

        return orders;
    }
}