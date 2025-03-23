using GrpcHw.Task1.Server.HttpClientService.Models;
using Newtonsoft.Json;

namespace GrpcHw.Task1.Server.HttpClientService;

public class WeatherApi
{
    private readonly HttpClient _httpClient;

    public WeatherApi(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<WeatherServiceResponse> GetWeatherAsync(WeatherApiRequest request, CancellationToken cancellationToken = new())
    {
        using var response = await _httpClient.GetAsync(
            $"?latitude={request.Latitude}&longitude={request.Longitude}&hourly={request.Hourly}",
            cancellationToken);
        
        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        
        var weatherResponse = JsonConvert.DeserializeObject<List<WeatherApiResponseItem>>(content)?.FirstOrDefault();

        if (weatherResponse is null)
            throw new ArgumentNullException();
        
        var filteredData = weatherResponse.Hourly.Times
            .Select((time, index) => new 
            {
                Time = time,
                Temperature = weatherResponse.Hourly.Temperature2M[index],
                Index = index,
            })
            .Where(x => x.Index % 2 == 0)
            .Select(x => new WeatherServiceResponseItem
            {
                Time = x.Time,
                Temperature = x.Temperature
            })
            .OrderBy(x => x.Time)
            .ToList();


        return new WeatherServiceResponse
        {
            WeatherItems = filteredData,
            Type = weatherResponse.HourlyUnits.Temperature2M
        };
    }
}