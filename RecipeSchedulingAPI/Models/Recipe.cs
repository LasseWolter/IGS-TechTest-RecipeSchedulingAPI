namespace RecipeSchedulingAPI.Models;

public class Recipe
{
    public string Name { get; }
    public List<BasePhase> Phases { get; set; }
}
