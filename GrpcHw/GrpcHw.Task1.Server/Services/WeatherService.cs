using Grpc.Core;
using GrpcHw.Task1.Server.HttpClientService;
using GrpcHw.Task1.Server.HttpClientService.Models;

namespace GrpcHw.Task1.Server.Services;

public class WeatherService : Server.WeatherService.WeatherServiceBase
{
    private readonly WeatherApi _weatherApi;

    public WeatherService(WeatherApi weatherApi)
    {
        _weatherApi = weatherApi;
    }

    public override async Task GetWeather(GetWeatherRequest request, IServerStreamWriter<GetWeatherResponse> responseStream, ServerCallContext context)
    {
        var token = context.CancellationToken;

        var weatherResponse = await _weatherApi.GetWeatherAsync(new WeatherApiRequest
        {
            Hourly = "temperature_2m",
            Latitude = 52.52,
            Longitude = 13.41,
        }, token);
        
        foreach (var weatherItem in weatherResponse.WeatherItems)
        {
            await responseStream.WriteAsync(new GetWeatherResponse
            {
                Time = weatherItem.Time.ToString("dd:MM:yy hh:mm:ss"),
                Temperature2M = weatherItem.Temperature.ToString("F1"),
            }, token);
        }
    }
}