using RecipeSchedulingAPI.Interfaces;
using RecipeSchedulingAPI.Models;

namespace RecipeSchedulingAPI.Services;

public class SchedulingService : ISchedulingService
{
    private readonly ILogger<SchedulingService> _logger;

    public SchedulingService(ILogger<SchedulingService> logger)
    {
        _logger = logger;
    }

    public Schedule? CreateSchedule(RecipeRequest request, List<Recipe> recipes)
    {
        if (request.StartDate == null)
        {
            _logger.LogError("StartDate is null, can't create schedule.");
            return null;
        }

        DateTime startDate = (DateTime)request.StartDate;

        var matchedRecipes = recipes.FirstOrDefault(r => r.Name == request.RecipeName);
        if (matchedRecipes == null)
        {
            _logger.LogWarning($"No matching recipe found for RecipeRequest: {request.RecipeName}");
            return null;
        }

        Schedule schedule = new Schedule();

        foreach (WateringPhase wateringPhase in matchedRecipes.WateringPhases)
        {
            for (int i = 0; i < wateringPhase.Repetitions; i++)
            {
                int timeOffsetMinutes = wateringPhase.Hours * 60 * i + wateringPhase.Minutes * i;
                schedule.Commands.Add(new Commands(startDate.AddMinutes(timeOffsetMinutes), request.TrayNumber, CommandType.Water, waterAmount: wateringPhase.Amount));
            }
        }

        foreach (LightingPhase lightingPhase in matchedRecipes.LightingPhases)
        {
            for (int i = 0; i < lightingPhase.Repetitions; i++)
            {
                int phaseStartOffsetMinutes = lightingPhase.Hours * 60 * i + lightingPhase.Minutes * i;
                foreach (Operation operation in lightingPhase.Operations)
                {
                    int operationOffsetInMinutes = phaseStartOffsetMinutes + operation.OffsetHours * 60 + operation.OffsetMinutes;
                    schedule.Commands.Add(new Commands(startDate.AddMinutes(operationOffsetInMinutes), request.TrayNumber,  CommandType.Light, lightIntensity: operation.LightIntensity));
                }
            }
        }

        schedule.Commands = schedule.Commands.OrderBy(e => e.DateTimeUtc).ToList();
        return schedule;
    }
}