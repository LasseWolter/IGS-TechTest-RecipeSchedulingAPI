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
        }
        
        var recipes = JsonSerializer.Deserialize<RecipeList>(await response.Content.ReadAsStringAsync());

        return _schedulingService.CreateSchedule(new Recipe());
    }
}