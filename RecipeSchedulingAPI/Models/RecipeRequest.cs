using System.Globalization;
using System.Text.Json.Serialization;

namespace RecipeSchedulingAPI.Models;

public class RecipeRequest
{
    [JsonPropertyName("trayNumber")]
    public int TrayNumber { get; set; }

    [JsonPropertyName("recipeName")]
    public string RecipeName { get; set; }

    /// <summary>
    /// Raw date string that is sent to the API endpoint.
    /// Set StartDate if the date is valid, otherwise StartDate will be null.
    /// </summary>
    private string startDateRaw;

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

    public DateTime? StartDate { get; set; }
}