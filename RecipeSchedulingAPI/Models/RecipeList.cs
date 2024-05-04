using System.Text.Json.Serialization;

namespace RecipeSchedulingAPI.Models;

public class RecipeList
{
    [JsonPropertyName("recipes")] 
    public List<Recipe> Recipes { get; set; }
}