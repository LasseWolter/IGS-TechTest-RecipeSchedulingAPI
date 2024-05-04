using System.Text.Json;
using RecipeSchedulingAPI.Enums;
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
                schedule.Events.Add(new Event(startDate.AddHours(wateringPhase.Hours).AddMinutes(wateringPhase.Minutes), CommandType.Water, waterAmount: wateringPhase.Amount));
            }
        }
        return schedule;
    }
}