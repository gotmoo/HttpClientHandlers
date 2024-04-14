namespace HttpClientHandlers.Weather;

public interface IWeatherService
{
    Task<WeatherResponse?> GetWeatherForCityAsync(string city);
}
