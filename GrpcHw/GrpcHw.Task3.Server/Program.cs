
using System.Text;
using FluentValidation;
using GrpcHw.Task3.Server;
using GrpcHw.Task3.Server.Data;
using GrpcHw.Task3.Server.Services;
using GrpcHw.Task3.Server.Utils;
using GrpcHw.Task3.Server.Validators;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using AuthService = GrpcHw.Task3.Server.Services.AuthService;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel(opt =>
{
    opt.ListenAnyIP(9090, opt =>
    {
        opt.Protocols = HttpProtocols.Http1AndHttp2;
    });
});
builder.Services.AddGrpc();
builder.Services.AddMemoryCache();
builder.Services.AddScoped<JwtTokenGenerator>();
builder.Services.AddScoped<HashService>();
builder.Services.AddScoped<IValidator<RegisterRequest>, RegisterValidator>();
builder.Services.AddAuthorization();
builder.Services.AddTransient<Migrator>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });
builder.Services.AddCors(po => po.AddDefaultPolicy(p => p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration["ConnectionString"]));

var app = builder.Build();

using var scope = app.Services.CreateScope();
var provider = scope.ServiceProvider;
var migrator = provider.GetRequiredService<Migrator>();
await migrator.MigrateAsync();

app.UseCors();
app.UseRouting();
app.UseGrpcWeb();

app.UseAuthentication();
app.UseAuthorization();

app.MapGrpcService<AuthService>().EnableGrpcWeb();
app.MapGrpcService<ChatService>().EnableGrpcWeb();
app.Run();