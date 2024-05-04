using System.Text.Json.Serialization;
using RecipeSchedulingAPI.Interfaces;

namespace RecipeSchedulingAPI.Models;

public class LightingPhase : IPhase
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    [JsonPropertyName("order")]
    public int Order { get; set; }
    
    [JsonPropertyName("hours")]
    public int Hours { get; set; }
    
    [JsonPropertyName("minutes")]
    public int Minutes { get; set; }
    
    [JsonPropertyName("repititions")]
    public int Repetitions { get; set; }
    
    [JsonPropertyName("operations")]
    public List<Operation> Operations { get; set;  }
}