using Newtonsoft.Json;

namespace GrpcHw.Task1.Server.HttpClientService.Models;


public class WeatherApiResponse
{
    public List<WeatherApiResponseItem> WeatherApiResponseItems { get; set; }
    public string Temperature2M { get; set; }
}

public class WeatherApiResponseItem
{

    [JsonProperty("timezone_abbreviation")]
    public string TimezoneAbbreviation { get; set; }

    [JsonProperty("hourly")]
    public Hourly Hourly { get; set; }

    [JsonProperty("hourly_units")]
    public HourlyUnits HourlyUnits { get; set; }
}

public class Hourly
{
    [JsonProperty("time")]
    public List<DateTime> Times { get; set; }

    [JsonProperty("temperature_2m")]
    public List<double> Temperature2M { get; set; }
}

public class HourlyUnits
{
    [JsonProperty("temperature_2m")]
    public string Temperature2M { get; set; }
}
