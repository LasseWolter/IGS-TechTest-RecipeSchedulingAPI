using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using RecipeSchedulingAPI.Models;
using RecipeSchedulingAPI.Services;

namespace RecipeSchedulingAPI.Controllers;

[ApiController]
[Route("api/v1/schedule")]
public class ScheduleController
{
    private readonly SchedulingService _schedulingService;
    private readonly ILogger<ScheduleController> _logger;

    public ScheduleController(ILogger<ScheduleController> logger, SchedulingService schedulingService)
    {
        _logger = logger;
        _schedulingService = schedulingService;
    }

    [HttpGet]
    public async Task<Schedule> GetSchedule()
    {
        _logger.LogDebug("GetSchedule entered.");

        // REMARK: Would be better to pass this in using DI
        HttpClient httpClient = new HttpClient();
        // REMARK: The domain where this enpoint is running should ideally be configured in appsettings.config such that 
        // you only need to switch out the config to run this code in different environments.
        var response = await httpClient.GetAsync("http://localhost:8080/recipe");

        if (!response.IsSuccessStatusCode)
        {
            // REMARK: Could add more detail to the error here to make monitoring easier.
            _logger.LogError($"API request to fetch recipe failed: {response.ReasonPhrase}");
            // TODO: return error code
        }

        RecipeList? recipeList = null;
        try
        {
            recipeList = JsonSerializer.Deserialize<RecipeList>(await response.Content.ReadAsStringAsync());
        }
        catch(Exception e)
        {
            _logger.LogError($"Error deserialising recipe response: {e}");
            // TODO: return error code
        }

        if (recipeList == null)
        {  
            _logger.LogError($"Recipe list is null.");
            // TODO: return error code
        }

        var recipeRequestStringRaw = """
                                     {
                                       "input": [
                                         {
                                           "trayNumber": 1,
                                           "recipeName": "Basil",
                                           "startDate": "2022-01-24T12:30:00.0000000Z"
                                         },
                                         {
                                           "trayNumber": 2,
                                           "recipeName": "Strawberries",
                                           "startDate": "2021-13-08T17:33:00.0000000Z"
                                         },
                                         {
                                           "trayNumber": 3,
                                           "recipeName": "Basil",
                                           "startDate": "2030-01-01T23:45:00.0000000Z"
                                         }
                                       ]
                                     }
                                     """;
        RecipeRequestList recipeRequestList = JsonSerializer.Deserialize<RecipeRequestList>(recipeRequestStringRaw);

        return _schedulingService.CreateScheduleForListOfRequests(recipeRequestList.RecipeRequests, recipeList!.Recipes, ordered: true);
    }
}