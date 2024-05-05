using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using RecipeSchedulingAPI.Models;
using RecipeSchedulingAPI.Services;

namespace RecipeSchedulingAPI.Controllers;

[ApiController]
[Route("api/v1/schedule")]
public class ScheduleController : ControllerBase
{
    private readonly SchedulingService _schedulingService;
    private readonly ILogger<ScheduleController> _logger;

    public ScheduleController(ILogger<ScheduleController> logger, SchedulingService schedulingService)
    {
        _logger = logger;
        _schedulingService = schedulingService;
    }
    
    [HttpPost]
    // REMARK: Parsing the JSON using [FromBody] automatically returns a 400 because we put the [ApiController] attribute on this controller.  
    // This means we don't need to do manual checking if the JSON is valid. 
    public async Task<IActionResult> GetScheduleForListOfRequests([FromBody] RecipeRequestList recipeRequestList)
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
            return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong on our side, please try again later.");
        }

        RecipeList? recipeList = null;
        try
        {
            recipeList = JsonSerializer.Deserialize<RecipeList>(await response.Content.ReadAsStringAsync());
        }
        catch(Exception e)
        {
            _logger.LogError($"Error deserialising recipe response: {e}");
            return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong on our side, please try again later.");
        }

        if (recipeList == null)
        {  
            _logger.LogError($"Recipe list is null.");
            return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong on our side, please try again later.");
        }
        
        var schedule = _schedulingService.CreateScheduleForListOfRequests(recipeRequestList.RecipeRequests, recipeList!.Recipes, ordered: true);

        if (schedule == null)
        {
            return BadRequest("Your requests does not contain any valid requests or doesn't match any recipes in our backend. Please check your request data.");
        }

        return Ok(schedule);
    }
}