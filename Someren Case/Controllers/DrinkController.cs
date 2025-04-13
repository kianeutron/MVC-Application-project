using Microsoft.AspNetCore.Mvc;
using Someren_Case.Models;
using Someren_Case.Repositories;

namespace Someren_Case.Controllers;

public class DrinkController : Controller
{
    private readonly IDrinkRepository _drinkRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IStudentRepository _studentRepository;

    public DrinkController(IDrinkRepository drinkRepository, IOrderRepository orderRepository,
        IStudentRepository studentRepository)
    {
        _drinkRepository = drinkRepository;
        _orderRepository = orderRepository;
        _studentRepository = studentRepository;
    }

    public IActionResult Index()
    {
        var drinks = _drinkRepository.GetAllDrinks();

        var orders = _orderRepository.GetAllOrders();
        var students = _studentRepository.GetAll();

        var drinkOrders = new List<DrinkOrderViewModel>();

        foreach (var drink in drinks)
        {
            var ordersForDrink = orders
                .Where(order => order.DrinkID == drink.DrinkID)
                .Select(order => new OrderViewModel
                {
                    StudentName = students.FirstOrDefault(student => student.StudentID == order.StudentID)?.FirstName,
                    Quantity = order.Quantity
                }).ToList();

            drinkOrders.Add(new DrinkOrderViewModel
            {
                Drink = drink,
                Orders = ordersForDrink
            });
        }


        ViewBag.OrderSuccess = TempData["OrderSuccess"];
        ViewBag.OrderError = TempData["OrderError"];

        return View(drinkOrders);
    }


    public IActionResult Details(int id)
    {
        var drink = _drinkRepository.GetDrinkById(id);
        if (drink == null) return NotFound();
        return View(drink);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create([Bind("Name, Type, VATRate, StockQuantity")] Drink drink)
    {
        if (ModelState.IsValid)
        {
            _drinkRepository.AddDrink(drink);
            TempData["OrderSuccess"] = "Drink successfully created!";
            return RedirectToAction(nameof(Index));
        }

        return View(drink);
    }

    public IActionResult Edit(int id)
    {
        var drink = _drinkRepository.GetDrinkById(id);
        if (drink == null) return NotFound();
        return View(drink);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, [Bind("DrinkID, Name, Type, VATRate, StockQuantity")] Drink drink)
    {
        if (id != drink.DrinkID) return NotFound();

        if (ModelState.IsValid)
        {
            _drinkRepository.UpdateDrink(drink);
            TempData["OrderSuccess"] = "Drink successfully updated!";
            return RedirectToAction(nameof(Index));
        }

        return View(drink);
    }

    public IActionResult Delete(int id)
    {
        var drink = _drinkRepository.GetDrinkById(id);
        if (drink == null) return NotFound();

        return View(drink);
    }

    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        try
        {
            var drink = _drinkRepository.GetDrinkById(id);
            if (drink != null)
            {
                _drinkRepository.DeleteDrink(id);
                TempData["OrderSuccess"] = "Drink successfully deleted!";
            }

            return RedirectToAction("Index");
        }
        catch (Exception)
        {
            TempData["OrderError"] = "Error occurred while deleting the drink.";
            return RedirectToAction("Index");
        }
    }
}