using Someren_Case.Models;

namespace Someren_Case.Repositories;

public interface IDrinkRepository
{
    List<Drink> GetAllDrinks();
    Drink GetDrinkById(int id);
    void AddDrink(Drink drink);
    void UpdateDrink(Drink drink);
    void DeleteDrink(int id);
}