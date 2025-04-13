using Someren_Case.Models;

namespace Someren_Case.Repositories;

public interface IStudentRepository
{
    List<Student> GetAll();
    Student? GetById(int studentId);
    void Add(Student student);
    void Update(Student student);
    void Delete(Student student);
    List<Student> Filter(string studentClass);


    List<Student> GetStudentsByRoomId(int roomId);
    void AssignStudentToRoom(int studentId, int roomId);
    void RemoveStudentFromRoom(int studentId, int roomId);


    List<Student> GetStudentsWithoutRoom();
}