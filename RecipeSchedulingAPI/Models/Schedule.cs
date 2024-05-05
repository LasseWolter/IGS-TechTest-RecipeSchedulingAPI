namespace RecipeSchedulingAPI.Models;

public class Schedule
{
    public List<Commands> Commands { get; set; }

    public Schedule()
    {
        Commands = new List<Commands>();
    }
}
