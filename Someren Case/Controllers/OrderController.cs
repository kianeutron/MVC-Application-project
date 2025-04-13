using Microsoft.AspNetCore.Mvc;
using Someren_Case.Models;
using Someren_Case.Repositories;

public class OrderController : Controller
{
    private readonly IDrinkRepository _drinkRepository;
    private readonly IOrderRepository _orderRepo;
    private readonly IStudentRepository _studentRepository;

    public OrderController(IStudentRepository studentRepo, IDrinkRepository drinkRepo, IOrderRepository orderRepo)
    {
        _studentRepository = studentRepo;
        _drinkRepository = drinkRepo;
        _orderRepo = orderRepo;
    }

    [HttpGet]
    public IActionResult Create()
    {
        var students = _studentRepository.GetAll();
        var drinks = _drinkRepository.GetAllDrinks();

        ViewData["Students"] = students;
        ViewData["Drinks"] = drinks;

        return View();
    }

    [HttpPost]
    public IActionResult Create(OrderViewModel model)
    {
        if (model.StudentID.HasValue && model.DrinkID.HasValue && model.Quantity > 0)
        {
            var order = new Order
            {
                StudentID = model.StudentID.Value,
                DrinkID = model.DrinkID.Value,
                Quantity = model.Quantity
            };


            var drink = _drinkRepository.GetDrinkById(model.DrinkID.Value);
            if (drink != null)
            {
                if (drink.StockQuantity >= model.Quantity)
                {
                    drink.StockQuantity -= model.Quantity;


                    _drinkRepository.UpdateDrink(drink);


                    _orderRepo.AddOrder(order);


                    TempData["OrderSuccess"] = "Order placed successfully!";
                }
                else
                {
                    TempData["OrderError"] = "Not enough stock for this order!";
                }
            }
            else
            {
                TempData["OrderError"] = "Drink not found!";
            }

            return RedirectToAction("Index", "Drink");
        }


        model.Students = _studentRepository.GetAll();
        model.Drinks = _drinkRepository.GetAllDrinks();
        return View(model);
    }
}