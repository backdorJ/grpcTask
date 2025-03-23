using System.Net;
using GrpcHw.Task1.Server.HttpClientService;
using GrpcHw.Task1.Server.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.Listen(IPAddress.Any, 5001, listenOptions =>
    {
        listenOptions.UseHttps();
        listenOptions.Protocols = HttpProtocols.Http2;
    });
});

builder.Services.AddGrpc();
builder.Services.AddHttpClient<WeatherApi>(conf =>
{
    conf.BaseAddress = new Uri("https://api.open-meteo.com/v1/forecast");
});

var app = builder.Build();

app.MapGrpcService<WeatherService>();

app.Run();