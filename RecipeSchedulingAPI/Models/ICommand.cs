namespace RecipeSchedulingAPI.Models;

public interface ICommand
{
    public CommandType CommandType { get; }
    public Dictionary<string, int> Parameters { get; }
}