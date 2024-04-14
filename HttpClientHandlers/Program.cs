using HttpClientHandlers.CrowdStrike;
using HttpClientHandlers.Weather;

var builder = WebApplication.CreateBuilder(args);
var env = builder.Environment;

var secretsConfigPath = Path.Combine(env.ContentRootPath, "secrets.json");
if (File.Exists(secretsConfigPath))
    builder.Configuration.AddJsonFile(secretsConfigPath);

var envSecretsConfigPath = Path.Combine(env.ContentRootPath, $"secrets.{env.EnvironmentName}.json");
if (File.Exists(envSecretsConfigPath))
    builder.Configuration.AddJsonFile(envSecretsConfigPath);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMemoryCache();

builder.Services.AddScoped<CachedWeatherHandler>();
builder.Services.AddHttpClient("weather")
    .AddHttpMessageHandler<CachedWeatherHandler>();
builder.Services.AddSingleton<IWeatherService, OpenWeatherMapService>();

builder.Services.AddScoped<CachedCrowdStrikeHandler>();
builder.Services.AddHttpClient("CrowdStrike")
    .AddHttpMessageHandler<CachedCrowdStrikeHandler>();
builder.Services.AddSingleton<ICrowdStrikeService, CrowdStrikeService>();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/", () => $"Hello World")
    .WithName("Home")
    .WithOpenApi();

app.MapGet("/weather", async (
        string city,
        IWeatherService weatherService
    ) =>
    {
        var weather = await weatherService.GetWeatherForCityAsync(city);
        return weather is null ? Results.NotFound() : Results.Ok(weather);
    })
    .WithName("GetWeatherForecast")
    .WithOpenApi();

app.MapGet("/CrowdStrike/Token", async (
        ICrowdStrikeService crowdStrikeService
    ) =>
    {
        var csResponse = await crowdStrikeService.GetAccessToken();
        return csResponse is null ? Results.NotFound() : Results.Ok(csResponse);
    })
    .WithName("GetCsAccessToken")
    .WithOpenApi();

app.MapGet("/CrowdStrike/Device", async (
        string deviceName,
        ICrowdStrikeService crowdStrikeService
    ) =>
    {
        var csResponse = await crowdStrikeService.GetStatusForHostname(deviceName);
        return csResponse is null ? Results.NotFound() : Results.Ok(csResponse);
    })
    .WithName("GetCsDevice")
    .WithOpenApi();

app.Run();