using System.Text.Json.Serialization;

namespace RecipeSchedulingAPI.Models;

public class RecipeRequestList
{
    [JsonPropertyName("input")]
    public List<RecipeRequest> RecipeRequests { get; set; }
}