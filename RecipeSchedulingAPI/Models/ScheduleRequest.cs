using System.Globalization;
using System.Text.Json.Serialization;

namespace RecipeSchedulingAPI.Models;

public class ScheduleRequest
{
    /// <summary>
    /// The tray on which the fruit is grown.
    /// </summary>
    /// <example>1</example>
    [JsonPropertyName("trayNumber")]
    public int TrayNumber { get; set; }

    /// <summary>
    /// The recipe name for the fruit.
    /// </summary>
    /// <example>Basil</example>
    [JsonPropertyName("recipeName")]
    public string RecipeName { get; set; }

    private string startDateRaw;

    /// <summary>
    /// Raw date string that is sent to the API endpoint.
    /// Set StartDate if the date is valid, otherwise StartDate will be null.
    /// </summary>
    /// <example>2022-01-24T12:30:00.0000000Z</example>
    [JsonPropertyName("startDate")]
    public string StartDateRaw
    {
        get => startDateRaw;

        // REMARK: There are other ways of handling invalid data, e.g. using a custom JSON parser.
        // I opted for this approach to keep it easy for now and make the logic clear from this data model alone. 
        // It also allows me to just read all JSON data without a try catch block and then filter out unwanted data in the end.
        // Tbf, this approach could be inefficient if you read tons of data, because you'd probably want to throw away the data as soon
        // as you identify bad/invalid data
        set
        {
            if (DateTime.TryParse(value, CultureInfo.InvariantCulture, out DateTime startDate))
            {
                StartDate = startDate;
            }

            startDateRaw = value;
        }
    }

    /// <summary>
    /// Date when you want to start growing the fruit. Is null if the date passed in was invalid/couldn't be parsed.
    /// </summary>
    [JsonIgnore]
    public DateTime? StartDate { get; set; }
}