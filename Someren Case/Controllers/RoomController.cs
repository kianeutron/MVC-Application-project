using Microsoft.AspNetCore.Mvc;
using Someren_Case.Models;
using Someren_Case.Repositories;

namespace Someren_Case.Controllers;

public class RoomController : Controller
{
    private readonly IRoomRepository _roomRepository;
    private readonly IStudentRepository _studentRepository;

    public RoomController(IRoomRepository roomRepository, IStudentRepository studentRepository)
    {
        _roomRepository = roomRepository;
        _studentRepository = studentRepository;
    }

    public IActionResult Index()
    {
        List<Room> rooms = _roomRepository.GetAllRooms();
        return View(rooms);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Room room)
    {
        if (ModelState.IsValid)
        {
            _roomRepository.AddRoom(room);
            TempData["Message"] = "Room created successfully!";
            return RedirectToAction("Index");
        }

        return View(room);
    }

    public IActionResult Edit(int id)
    {
        var room = _roomRepository.GetRoomById(id);
        if (room == null)
            return NotFound();

        return View(room);
    }

    [HttpPost]
    public IActionResult Edit(Room room)
    {
        if (ModelState.IsValid)
        {
            _roomRepository.UpdateRoom(room);
            TempData["Message"] = "Room updated successfully!";
            return RedirectToAction("Index");
        }

        return View(room);
    }

    public IActionResult Delete(int id)
    {
        var room = _roomRepository.GetRoomById(id);
        if (room == null)
            return NotFound();

        return View(room);
    }

    [HttpPost]
    [ActionName("Delete")]
    public IActionResult DeleteConfirmed(int id)
    {
        _roomRepository.DeleteRoom(id);
        TempData["Message"] = "Room deleted successfully!";
        return RedirectToAction("Index");
    }

    public IActionResult ManageDormitory(int id)
    {
        var room = _roomRepository.GetRoomById(id);
        if (room == null)
            return NotFound();

        var studentsInDorm = _studentRepository.GetStudentsByRoomId(id);
        var studentsWithoutRoom = _studentRepository.GetStudentsWithoutRoom();

        var viewModel = new ManageDormitoryViewModel
        {
            Room = room,
            StudentsInDorm = studentsInDorm,
            StudentsWithoutRoom = studentsWithoutRoom
        };

        return View(viewModel);
    }

    [HttpPost]
    public IActionResult AddStudentToDormitory(int roomId, int studentId)
    {
        try
        {
            _studentRepository.AssignStudentToRoom(studentId, roomId);
            TempData["Message"] = "Student added to dormitory successfully!";
        }
        catch (Exception ex)
        {
            TempData["Message"] = "There was an error adding the student to the dormitory.";
        }

        return RedirectToAction("ManageDormitory", new { id = roomId });
    }

    [HttpPost]
    public IActionResult RemoveStudentFromDormitory(int roomId, int studentId)
    {
        try
        {
            _studentRepository.RemoveStudentFromRoom(studentId, roomId);
            TempData["Message"] = "Student removed from dormitory successfully!";
        }
        catch (Exception ex)
        {
            TempData["Message"] = "There was an error removing the student from the dormitory.";
        }

        return RedirectToAction("ManageDormitory", new { id = roomId });
    }
}