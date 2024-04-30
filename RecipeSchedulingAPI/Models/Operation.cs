using RecipeSchedulingAPI.Enums;

namespace RecipeSchedulingAPI.Models;

public class Operation
{
    public int OffsetHourse { get; set; }
    public int OffsetMinutes { get; set; }
    public LightIntensity LightIntensity { get; set; } 
}