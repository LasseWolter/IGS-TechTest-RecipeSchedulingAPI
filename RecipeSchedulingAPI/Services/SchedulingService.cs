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
        var matchedRecipes = recipes.FirstOrDefault(r => r.Name == request.RecipeName);
        if (matchedRecipes == null)
        {
            _logger.LogWarning($"No matching recipe found for RecipeRequest: {request.RecipeName}");
            return null;
        }
        
        Schedule schedule = new Schedule();
        foreach (WateringPhase wateringPhase in matchedRecipes.WateringPhases)
        {
            schedule.Events.Add(new Event(DateTime.UtcNow, CommandType.Water, waterAmount: wateringPhase.Amount));
        }
        return schedule;
    }
}