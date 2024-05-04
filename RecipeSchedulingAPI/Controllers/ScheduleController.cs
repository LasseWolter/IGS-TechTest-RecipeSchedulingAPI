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
    public Schedule GetSchedule()
    {
        _logger.LogDebug("GetSchedule entered.");
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