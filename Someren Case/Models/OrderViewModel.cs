namespace Someren_Case.Models;

public class OrderViewModel
{
    public int? StudentID { get; set; }
    public int? DrinkID { get; set; }
    public int Quantity { get; set; }

    public string StudentName { get; set; }
    public List<Drink> Drinks { get; set; }
    public List<Student> Students { get; set; }
}