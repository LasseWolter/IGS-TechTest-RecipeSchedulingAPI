namespace RecipeSchedulingAPI.Models;

public class Schedule
{
    public List<Event> Events { get; set; }

    public Schedule()
    {
        Events = new List<Event>();
    }
}
