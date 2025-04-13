namespace Someren_Case.Models;

public class Drink
{
    public int DrinkID { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public decimal VATRate { get; set; }
    public int StockQuantity { get; set; }
}