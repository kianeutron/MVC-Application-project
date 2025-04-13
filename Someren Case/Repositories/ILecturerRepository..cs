using Someren_Case.Models;

namespace Someren_Case.Interfaces;

public interface ILecturerRepository
{
    List<Lecturer> GetAllLecturers();
    Lecturer GetLecturerById(int id);
    void AddLecturer(Lecturer lecturer);
    void UpdateLecturer(Lecturer lecturer);
    void DeleteLecturer(int id);
}