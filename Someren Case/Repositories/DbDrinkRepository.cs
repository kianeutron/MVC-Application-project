using Microsoft.Data.SqlClient;
using Someren_Case.Models;

namespace Someren_Case.Repositories;

public class DbDrinkRepository : IDrinkRepository
{
    private readonly string _connectionString;

    public DbDrinkRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("dbproject242504")
                            ?? throw new ArgumentNullException("Connection string not found!");
    }


    public List<Drink> GetAllDrinks()
    {
        var drinks = new List<Drink>();

        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            var command = new SqlCommand("SELECT DrinkID, Name, Type, VATRate, StockQuantity FROM Drink", connection);

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var drink = new Drink
                    {
                        DrinkID = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Type = reader.GetString(2),
                        VATRate = reader.GetDecimal(3),
                        StockQuantity = reader.GetInt32(4)
                    };
                    drinks.Add(drink);
                }
            }
        }

        return drinks;
    }


    public Drink GetDrinkById(int id)
    {
        Drink drink = null;

        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            var command =
                new SqlCommand("SELECT DrinkID, Name, Type, VATRate, StockQuantity FROM Drink WHERE DrinkID = @DrinkID",
                    connection);
            command.Parameters.AddWithValue("@DrinkID", id);

            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                    drink = new Drink
                    {
                        DrinkID = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Type = reader.GetString(2),
                        VATRate = reader.GetDecimal(3),
                        StockQuantity = reader.GetInt32(4)
                    };
            }
        }

        return drink;
    }


    public void AddDrink(Drink drink)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            var command =
                new SqlCommand(
                    "INSERT INTO Drink (Name, Type, VATRate, StockQuantity) VALUES (@Name, @Type, @VATRate, @StockQuantity)",
                    connection);
            command.Parameters.AddWithValue("@Name", drink.Name);
            command.Parameters.AddWithValue("@Type", drink.Type);
            command.Parameters.AddWithValue("@VATRate", drink.VATRate);
            command.Parameters.AddWithValue("@StockQuantity", drink.StockQuantity);

            command.ExecuteNonQuery();
        }
    }


    public void UpdateDrink(Drink drink)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            var command =
                new SqlCommand(
                    "UPDATE Drink SET Name = @Name, Type = @Type, VATRate = @VATRate, StockQuantity = @StockQuantity WHERE DrinkID = @DrinkID",
                    connection);
            command.Parameters.AddWithValue("@Name", drink.Name);
            command.Parameters.AddWithValue("@Type", drink.Type);
            command.Parameters.AddWithValue("@VATRate", drink.VATRate);
            command.Parameters.AddWithValue("@StockQuantity", drink.StockQuantity);
            command.Parameters.AddWithValue("@DrinkID", drink.DrinkID);

            command.ExecuteNonQuery();
        }
    }


    public void DeleteDrink(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            var command = new SqlCommand("DELETE FROM Drink WHERE DrinkID = @DrinkID", connection);
            command.Parameters.AddWithValue("@DrinkID", id);

            command.ExecuteNonQuery();
        }
    }
}