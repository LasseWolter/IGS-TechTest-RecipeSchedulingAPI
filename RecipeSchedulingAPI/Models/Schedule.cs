namespace RecipeSchedulingAPI.Models;

public class Schedule
{
    public Schedule()
    {
        Commands = new List<Commands>();
    }

    public List<Commands> Commands { get; set; }
}