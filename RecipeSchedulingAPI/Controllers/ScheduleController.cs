using System.Data;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using RecipeSchedulingAPI.Interfaces;
using RecipeSchedulingAPI.Models;

namespace RecipeSchedulingAPI.Controllers;

[ApiController]
[Route("api/v1/schedule")]
public class ScheduleController(ILogger<ScheduleController> logger, ISchedulingService schedulingService) : ControllerBase
{
    /// <summary>
    ///     Simple welcome endpoint to test if the API is running.
    /// </summary>
    [HttpGet]
    public IActionResult GetRoot()
    {
        logger.LogDebug($"{nameof(GetScheduleForListOfRequests)} entered.");
        return Ok("Welcome to the RecipeSchedulingAPI.");
    }

    /// <summary>
    ///     Get a schedule for a list of fruits by defining a list of schedule requests (consisting of recipe name, tray and
    ///     start date) in the body.
    /// </summary>
    /// <param name="scheduleRequestList">List of requests for which you want to generate a schedule.</param>
    [HttpPost]
    // REMARK: Route naming isn't great here but does the work for now.
    // For production routes should ideally be as self-explanatory as possible.
    [Route("multiple")]
    // REMARK: Parsing the JSON using [FromBody] automatically returns a 400 because we put the [ApiController] attribute on this controller.  
    // This means we don't need to do manual checking if the JSON is valid. 
    public async Task<IActionResult> GetScheduleForListOfRequests([FromBody] ScheduleRequestList scheduleRequestList)
    {
        logger.LogDebug($"{nameof(GetScheduleForListOfRequests)} entered.");

        RecipeList recipeList;
        try
        {
            recipeList = await FetchRecipeData();
        }
        catch (Exception e)
        {
            // REMARK: With an API you generally don't want to expose the exact issue to the outside if it's not something the user/request did wrong.
            // Being more specific about the issue can leak internal about the API design and pose a security risk.
            logger.LogError(e, "Error when trying to fetch recipe data.");
            return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong on our side, please try again later.");
        }

        var schedule = schedulingService.CreateScheduleForListOfRequests(scheduleRequestList.ScheduleRequests, recipeList.Recipes, true);

        if (schedule == null) return BadRequest("Your requests does not contain any valid requests or doesn't match any recipes in our backend. Please check your request data.");

        return Ok(schedule);
    }


    /// <summary>
    ///     Get a schedule for a single fruit by defining a single schedule request (consisting of recipe name, tray and start
    ///     date) in the body.
    /// </summary>
    /// <param name="scheduleRequest">Request for which you want to generate the schedule.</param>
    /// <returns></returns>
    [HttpPost]
    // REMARK: Route naming isn't great here but does the work for now.
    // For production routes should ideally be as self-explanatory as possible.
    [Route("single")]
    // REMARK: Parsing the JSON using [FromBody] automatically returns a 400 because we put the [ApiController] attribute on this controller.  
    // This means we don't need to do manual checking if the JSON is valid. 
    public async Task<IActionResult> GetScheduleForSingleRequest([FromBody] ScheduleRequest scheduleRequest)
    {
        logger.LogDebug($"{nameof(GetScheduleForSingleRequest)} entered.");
        RecipeList recipeList;
        try
        {
            recipeList = await FetchRecipeData();
        }
        catch (Exception e)
        {
            // REMARK: With an API you generally don't want to expose the exact issue to the outside if it's not something the user/request did wrong.
            // Being more specific about the issue can leak internal about the API design and pose a security risk.
            logger.LogError(e, "Error when trying to fetch recipe data.");
            return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong on our side, please try again later.");
        }

        var schedule = schedulingService.CreateScheduleForSingleRequest(scheduleRequest, recipeList.Recipes, true);

        if (schedule == null)
            // REMARK: Ideally we'd return if the start date is null, such that the error is more obvious. 
            return BadRequest("Your request isn't valid or doesn't match any recipes in our backend. Please check your request data.");

        return Ok(schedule);
    }

    // REMARK: The following method does more than one thing violating the SingleResponsibility principle. 
    // I use this method for simplicity. For production code you'd want to move this method somewhere else, e.g. an HttpService and then pass 
    // this service into the Controller using DI. That way you can reuse the same HttpService across controllers.
    // This level of abstraction would also allow you to implement things like caching. 
    // Fetching the recipe data on each request is very inefficient.
    /// <summary>
    ///     Fetch recipe data from the recipe API and serialise it to a Recipe object.
    /// </summary>
    /// <returns>Recipe object.</returns>
    /// <exception cref="HttpRequestException">Thrown if the request to the Recipe API fails.</exception>
    /// <exception cref="DataException">Thrown if the returned and deserialized recipe data is null.</exception>
    /// <exception>Throws exception when recipe data returned by the API can't be serialized into a Recipe object.</exception>
    private async Task<RecipeList> FetchRecipeData()
    {
        // REMARK: Would be better to pass this in using DI
        var httpClient = new HttpClient();
        // REMARK: The domain where this endpoint is running should ideally be configured in appsettings.config such that 
        // you only need to switch out the config to run this code in different environments.
        // Also, note that this is using the internal endpoint within the network spun up through docker-compose. 
        var endpoint = "http://recipe-api:80/recipe";
        var response = await httpClient.GetAsync(endpoint);

        if (!response.IsSuccessStatusCode)
        {
            // REMARK: Added more detail to the data property of the exception to make monitoring easier.
            // This can come in really handy if you sent your logs to some kind of feed where these details can be extracted.
            var e = new HttpRequestException("Failed to fetch recipe data from recipe API.");
            e.Data.Add("Endpoint", endpoint);
            e.Data.Add("Response Status Code", response.StatusCode);
            e.Data.Add("Response Reason Phrase", response.ReasonPhrase);
            throw e;
        }

        // If deserialization fails, an exception is thrown. This exception will be caught outside of this method
        var recipeList = JsonSerializer.Deserialize<RecipeList>(await response.Content.ReadAsStringAsync());

        if (recipeList == null) throw new DataException("Recipe List is null");

        return recipeList;
    }
}