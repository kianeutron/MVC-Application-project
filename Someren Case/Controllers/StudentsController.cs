using Microsoft.AspNetCore.Mvc;
using Someren_Case.Models;
using Someren_Case.Repositories;

namespace Someren_Case.Controllers;

public class StudentController : Controller
{
    private readonly IStudentRepository _studentRepository;

    public StudentController(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }


    public IActionResult Index()
    {
        List<Student> students = _studentRepository.GetAll();
        return View(students);
    }


    [HttpPost]
    public IActionResult Filter(string studentClass)
    {
        try
        {
            List<Student> students = _studentRepository.Filter(studentClass);
            return View("Index", students);
        }
        catch (Exception)
        {
            return RedirectToAction("Index");
        }
    }


    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }


    [HttpPost]
    public IActionResult Create(Student student)
    {
        try
        {
            if (ModelState.IsValid)
            {
                _studentRepository.Add(student);
                return RedirectToAction("Index");
            }

            return View(student);
        }
        catch (Exception ex)
        {
            return View(student);
        }
    }


    [HttpGet]
    public IActionResult Edit(int studentId)
    {
        var student = _studentRepository.GetById(studentId);
        if (student == null) return NotFound();
        return View(student);
    }


    [HttpPost]
    public IActionResult Edit(Student student)
    {
        try
        {
            _studentRepository.Update(student);
            return RedirectToAction("Index");
        }
        catch (Exception)
        {
            return View(student);
        }
    }

    [HttpGet]
    public IActionResult Delete(int studentId)
    {
        var student = _studentRepository.GetById(studentId);

        if (student == null) return NotFound();

        return View(student);
    }


    [HttpPost]
    [ActionName("Delete")]
    public IActionResult DeleteConfirmed(int studentId)
    {
        try
        {
            var student = _studentRepository.GetById(studentId);

            if (student != null) _studentRepository.Delete(student);

            return RedirectToAction("Index");
        }
        catch (Exception)
        {
            return RedirectToAction("Index");
        }
    }
}