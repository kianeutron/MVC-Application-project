namespace Someren_Case.Models;

public class Activity
{
    public int ActivityID { get; set; }
    public string ActivityName { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
}