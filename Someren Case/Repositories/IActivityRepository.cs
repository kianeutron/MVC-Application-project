using Someren_Case.Models;

public interface IActivityRepository
{
    List<Activity> GetAll();
    Activity GetById(int activityId);
    void Add(Activity activity);
    void Update(Activity activity);
    void Delete(Activity activity);
}