using Microsoft.AspNetCore.Mvc;
using Someren_Case.Models;
using Someren_Case.Repositories;

namespace Someren_Case.Controllers;

public class ActivityController : Controller
{
    private readonly IActivityParticipantRepository _activityParticipantRepository;
    private readonly IActivityRepository _activityRepository;
    private readonly IStudentRepository _studentRepository;

    public ActivityController(IActivityRepository activityRepository, IStudentRepository studentRepository,
        IActivityParticipantRepository activityParticipantRepository)
    {
        _activityRepository = activityRepository;
        _studentRepository = studentRepository;
        _activityParticipantRepository = activityParticipantRepository;
    }


    public IActionResult Index()
    {
        var activities = _activityRepository.GetAll();
        return View(activities);
    }


    public IActionResult Details(int id)
    {
        var activity = _activityRepository.GetById(id);
        if (activity == null) return NotFound();
        return View(activity);
    }


    public IActionResult Create()
    {
        return View();
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Activity activity)
    {
        if (ModelState.IsValid)
        {
            _activityRepository.Add(activity);
            return RedirectToAction(nameof(Index));
        }

        return View(activity);
    }


    public IActionResult Edit(int id)
    {
        var activity = _activityRepository.GetById(id);
        if (activity == null) return NotFound();
        return View(activity);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, Activity activity)
    {
        if (id != activity.ActivityID) return NotFound();

        if (ModelState.IsValid)
        {
            _activityRepository.Update(activity);
            return RedirectToAction(nameof(Index));
        }

        return View(activity);
    }


    public IActionResult Delete(int id)
    {
        var activity = _activityRepository.GetById(id);
        if (activity == null) return NotFound();
        return View(activity);
    }


    [HttpPost]
    [ActionName("Delete")]
    public IActionResult DeleteConfirmed(int activityId)
    {
        try
        {
            var activity = _activityRepository.GetById(activityId);
            if (activity != null) _activityRepository.Delete(activity);
            return RedirectToAction("Index");
        }
        catch (Exception)
        {
            return RedirectToAction("Index");
        }
    }


    public IActionResult ManageParticipants(int activityId)
    {
        var activity = _activityRepository.GetById(activityId);
        if (activity == null) return NotFound();

        var participants = _activityParticipantRepository.GetParticipantsByActivityId(activityId);
        var nonParticipants = _activityParticipantRepository.GetNonParticipantsByActivityId(activityId);

        ViewBag.Activity = activity;
        ViewBag.Participants = participants;
        ViewBag.NonParticipants = nonParticipants;

        return View();
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddParticipant(int activityId, int studentId)
    {
        try
        {
            _activityParticipantRepository.AddParticipant(activityId, studentId);


            TempData["Message"] = "The participant has been successfully added to the activity.";


            return RedirectToAction("ManageParticipants", new { activityId });
        }
        catch (Exception ex)
        {
            TempData["Message"] = "There was an error adding the participant. Please try again.";


            return RedirectToAction("ManageParticipants", new { activityId });
        }
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult RemoveParticipant(int activityId, int studentId)
    {
        try
        {
            _activityParticipantRepository.RemoveParticipant(activityId, studentId);


            TempData["Message"] = "The participant has been successfully removed from the activity.";


            return RedirectToAction("ManageParticipants", new { activityId });
        }
        catch (Exception ex)
        {
            TempData["Message"] = "There was an error removing the participant. Please try again.";


            return RedirectToAction("ManageParticipants", new { activityId });
        }
    }
}