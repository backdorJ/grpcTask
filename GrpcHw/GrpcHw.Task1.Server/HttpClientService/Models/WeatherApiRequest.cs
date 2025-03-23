namespace GrpcHw.Task1.Server.HttpClientService.Models;

public class WeatherApiRequest
{
    /// <summary>
    /// Широта
    /// </summary>
    public double Latitude { get; set; }

    /// <summary>
    /// Долгота
    /// </summary>
    public double Longitude { get; set; }

    /// <summary>
    /// Почасовой
    /// </summary>
    public string Hourly { get; set; }
}