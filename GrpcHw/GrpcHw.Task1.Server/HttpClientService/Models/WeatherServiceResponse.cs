namespace GrpcHw.Task1.Server.HttpClientService.Models;

public class WeatherServiceResponse
{
    public List<WeatherServiceResponseItem> WeatherItems { get; set; }
    public string Type { get; set; }
}

public class WeatherServiceResponseItem
{
    public DateTime Time { get; set; }
    public double Temperature { get; set; }
}