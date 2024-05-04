using System.Text.Json.Serialization;
using RecipeSchedulingAPI.Enums;

namespace RecipeSchedulingAPI.Models;

public class Operation
{
    [JsonPropertyName("offsetHours")]
    public int OffsetHourse { get; set; }
    
    [JsonPropertyName("offsetMinutes")]
    public int OffsetMinutes { get; set; }
    
    [JsonPropertyName("lightIntensity")]
    public LightIntensity LightIntensity { get; set; } 
}