
using Grpc.Core;
using Grpc.Net.Client;
using GrpcHw.Task1.Server;

Console.Write("");

using var cts = new CancellationTokenSource();
var channel = GrpcChannel.ForAddress("https://localhost:5001");
var client = new WeatherService.WeatherServiceClient(channel);
var response = client.GetWeather(new GetWeatherRequest());

await foreach (var weatherItem in response.ResponseStream.ReadAllAsync(cts.Token))
    Console.WriteLine($"{DateTime.Now:hh:mm:ss} погода на {weatherItem.Time} = {weatherItem.Temperature2M} C");
