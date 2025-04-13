namespace Someren_Case.Models;

public class Order
{
    public int OrderID { get; set; }
    public int StudentID { get; set; }
    public int DrinkID { get; set; }
    public int Quantity { get; set; }
    public virtual Student Student { get; set; }
    public virtual Drink Drink { get; set; }
}