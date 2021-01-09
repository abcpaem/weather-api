using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using WeatherApi.Models;

namespace WeatherApi.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly WeatherApiDotComClient _weatherApiDotComClient;

        public WeatherService(WeatherApiDotComClient weatherApiDotComClient)
        {
            _weatherApiDotComClient = weatherApiDotComClient;
        }

        public async Task<CurrentWeather> GetCurrentWeather(string city)
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

            // TODO: Use AutoMapper
            var currentWeather = new CurrentWeather
            {
                City = currentWeatherResponse.Location.Name,
                Region = currentWeatherResponse.Location.Region,
                Country = currentWeatherResponse.Location.Country,
                LocalTime = currentWeatherResponse.Location.Localtime,
                Temperature = currentWeatherResponse.Current.Temperature
            };

            return currentWeather;
        }
    }
}