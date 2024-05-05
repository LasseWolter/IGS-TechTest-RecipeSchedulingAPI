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

    public Schedule? CreateScheduleForSingleRequest(RecipeRequest request, List<Recipe> recipes, bool ordered = false)
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
                    schedule.Commands.Add(new Commands(startDate.AddMinutes(operationOffsetInMinutes), request.TrayNumber, CommandType.Light, lightIntensity: operation.LightIntensity));
                }
            }
        }

        if (ordered)
        {
            schedule.Commands = schedule.Commands.OrderBy(e => e.DateTimeUtc).ToList();
        }

        return schedule;
    }

    // REMARK: Why return 'null' and not just an empty schedule? 
    // I decided to return null if something went wrong (e.g. the recipe wasn't found or the startDate is null). 
    // An empty schedule will be returned if there is nothing left to do. This could be handy if we later decide to add the functionality of 
    // querying this API for the schedule for update while a schedule is already being executed. 
    // I think having this distinction makes it easier for the tower to know and log if something is wrong.
    // We could also solely rely on logs from this API and just return an emtpy schedule if there is a problem. It's a design decision I'm happy to discuss.
    public Schedule? CreateScheduleForListOfRequests(List<RecipeRequest> requests, List<Recipe> recipes, bool ordered = false)
    {
        Schedule? fullSchedule = null;
        foreach (RecipeRequest request in requests)
        {
            var schedule = CreateScheduleForSingleRequest(request, recipes);
            if (schedule == null)
            {
                continue;
            }

            fullSchedule ??= new Schedule();

            fullSchedule.Commands.AddRange(schedule.Commands);
        }

        if (fullSchedule != null && ordered)
        {
            fullSchedule.Commands = fullSchedule.Commands.OrderBy(e => e.DateTimeUtc).ToList();
        }

        return fullSchedule;
    }
}