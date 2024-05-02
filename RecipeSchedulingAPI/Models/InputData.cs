using System.Text.Json.Serialization;

namespace RecipeSchedulingAPI.Models;

public class InputData
{
    [JsonPropertyName("input")]
    public List<InputEntry> Input { get; set; }
}