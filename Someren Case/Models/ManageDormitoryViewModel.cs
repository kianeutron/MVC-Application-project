namespace Someren_Case.Models;

public class ManageDormitoryViewModel
{
    public Room Room { get; set; }
    public List<Student> StudentsInDorm { get; set; }
    public List<Student> StudentsWithoutRoom { get; set; }
}