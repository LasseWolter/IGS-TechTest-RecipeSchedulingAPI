using System.Text.Json.Serialization;

namespace RecipeSchedulingAPI.Models;

public class ScheduleRequestList
{
    [JsonPropertyName("input")]
    public List<ScheduleRequest> ScheduleRequests { get; set; }
}