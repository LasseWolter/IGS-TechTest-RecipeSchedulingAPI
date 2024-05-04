using System.Text.Json.Serialization;

namespace RecipeSchedulingAPI.Models;

public class Recipe
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    [JsonPropertyName("lightingPhases")]
    public List<LightingPhase> LightingPhases { get; set; }
    
    [JsonPropertyName("wateringPhases")]
    public List<WateringPhase> WateringPhases { get; set; }
}
