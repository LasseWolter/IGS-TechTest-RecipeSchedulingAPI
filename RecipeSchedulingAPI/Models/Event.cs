using RecipeSchedulingAPI.Enums;

namespace RecipeSchedulingAPI.Models;

public class Event
{
    public DateTime DateTimeUtc { get; set; }
    public CommandType CommandType { get; set; }
    public int? WaterAmount { get; set; }
    public LightIntensity? LightIntensity { get; set; }

    public Event(DateTime dateTimeUtc, CommandType commandType, int? waterAmount = null, LightIntensity? lightIntensity = null)
    {
        DateTimeUtc = dateTimeUtc;
        CommandType = commandType;
        WaterAmount = waterAmount;
        LightIntensity = lightIntensity;
    }
}