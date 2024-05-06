using System.Text.Json.Serialization;

namespace RecipeSchedulingAPI.Models;

public class ScheduleRequestList
{
    
    /// <summary>
    /// A list of requests describing the fruit, location and start date.
    /// </summary>
    /// <example>
    /// [
    ///        {
    ///          "trayNumber": 1,
    ///          "recipeName": "Basil",
    ///          "startDate": "2022-01-24T12:30:00.0000000Z"
    ///        },
    ///        {
    ///          "trayNumber": 2,
    ///          "recipeName": "Strawberries",
    ///          "startDate": "2021-13-08T17:33:00.0000000Z"
    ///        },
    ///        {
    ///          "trayNumber": 3,
    ///          "recipeName": "Basil",
    ///          "startDate": "2030-01-01T23:45:00.0000000Z"
    ///        }
    /// ] 
    /// </example>
    [JsonPropertyName("input")]
    public List<ScheduleRequest> ScheduleRequests { get; set; }
}