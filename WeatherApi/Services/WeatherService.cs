using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using WeatherApi.Models;

namespace WeatherApi.Services
{
    public class WeatherService : IWeatherService
    {
        public enum TemperatureScale
        {
            Celsius,
            Fahrenheit
        }

        private readonly WeatherApiDotComClient _weatherApiDotComClient;

        public WeatherService(WeatherApiDotComClient weatherApiDotComClient)
        {
            _weatherApiDotComClient = weatherApiDotComClient;
        }

        public async Task<CurrentWeather> GetCurrentWeather(string city, int temperatureScale)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"/v1/current.json?q={city}");
            var httpResponse = await _weatherApiDotComClient.Client.SendAsync(request);

            if (!httpResponse.IsSuccessStatusCode)
            {
                return null;
            }

            var content = await httpResponse.Content.ReadAsStreamAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var currentWeatherResponse = await JsonSerializer.DeserializeAsync<CurrentWeatherResponse>(content, options);
            var astronomyResponse = await GetAstronomy(city);

            // TODO: Use AutoMapper
            var currentWeather = new CurrentWeather
            {
                City = currentWeatherResponse.Location.Name,
                Region = currentWeatherResponse.Location.Region,
                Country = currentWeatherResponse.Location.Country,
                LocalTime = currentWeatherResponse.Location.Localtime,
                Temperature = temperatureScale == (int)TemperatureScale.Fahrenheit
                    ? currentWeatherResponse.Current.TemperatureInFahrenheit
                    : currentWeatherResponse.Current.TemperatureInCelsius,
                SunRise = astronomyResponse.Astronomy.Astro.Sunrise,
                SunSet = astronomyResponse.Astronomy.Astro.Sunset
            };

            return currentWeather;
        }

        private async Task<AstronomyResponse> GetAstronomy(string city)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"/v1/astronomy.json?q={city}");
            var httpResponse = await _weatherApiDotComClient.Client.SendAsync(request);

            if (!httpResponse.IsSuccessStatusCode)
            {
                return new AstronomyResponse { Astronomy = new Astronomy { Astro = new Astro() } };
            }

            var content = await httpResponse.Content.ReadAsStreamAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            return await JsonSerializer.DeserializeAsync<AstronomyResponse>(content, options);
        }
    }
}