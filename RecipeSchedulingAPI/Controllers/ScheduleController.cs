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
        string endpoint = "http://localhost:8080/recipe";
        var response = await httpClient.GetAsync(endpoint);

        if (!response.IsSuccessStatusCode)
        {
            // REMARK: Added more detail to the data property of the exception to make monitoring easier.
            // This can come in really handy if you sent your logs to some kind of feed where these details can be extracted.
            var e = new Exception("Failed to fetch recipe data from recipe API.");
            e.Data.Add("Endpoint", endpoint);
            e.Data.Add("Response Status Code", response.StatusCode);
            e.Data.Add("Response Reason Phrase", response.ReasonPhrase);
            _logger.LogError(e, "API request to fetch recipe failed");
            
            // REMARK: With an API you generally don't want to expose the exact issue to the outside if it's not something the user/request did wrong.
            // Being more specific about the issue can leak internal about the API design and pose a security risk.
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