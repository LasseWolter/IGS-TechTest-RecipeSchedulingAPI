using System.Text.Json;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RecipeSchedulingAPI.Enums;
using RecipeSchedulingAPI.Models;

namespace RecipeSchedulingAPI.Controllers;

[ApiController]
[Route("api/v1/schedule")]
public class ScheduleController
{
    private readonly ILogger<ScheduleController> _logger;

    public ScheduleController(ILogger<ScheduleController> logger)
    {
        _logger = logger;
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

        Console.WriteLine(await response.Content.ReadAsStringAsync());
        
        var rawJsonString = """
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
        InputData inputData = JsonSerializer.Deserialize<InputData>(rawJsonString);
        Schedule schedule = new Schedule();
        schedule.Events.Add(new Event(DateTime.UtcNow, CommandType.Water, waterAmount: 3));
        schedule.Events.Add(new Event(DateTime.UtcNow, CommandType.Light, lightIntesnity: LightIntensity.High));
        return schedule;
    }
}