using RecipeSchedulingAPI.Enums;

namespace RecipeSchedulingAPI.Models;

public class Commands
{
    public Commands(DateTime dateTimeUtc, int trayNumber, CommandType commandType, int? waterAmount = null, LightIntensity? lightIntensity = null)
    {
        DateTimeUtc = dateTimeUtc;
        TrayNumber = trayNumber;
        CommandType = commandType;
        WaterAmount = waterAmount;
        LightIntensity = lightIntensity;
    }

    // REMARK: One could add a string for the phase here but technically that's not needed for the tower.
    // The tower only needs to know when, where and what to do. The context doesn't really matter. 
    // It might help though if something goes wrong to have some context. 
    // It's a tradeoff between sending less data over the wire and having more verbose logging/debugging capabilities.
    public DateTime DateTimeUtc { get; set; }
    public CommandType CommandType { get; set; }
    public int? WaterAmount { get; set; }
    public LightIntensity? LightIntensity { get; set; }

    public int TrayNumber { get; set; }
}