using Someren_Case.Models;

namespace Someren_Case.Repositories;

public interface IActivityParticipantRepository
{
    List<Student> GetParticipantsByActivityId(int activityId);
    List<Student> GetNonParticipantsByActivityId(int activityId);
    void AddParticipant(int activityId, int studentId);
    void RemoveParticipant(int activityId, int studentId);
}