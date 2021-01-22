using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using MapsterMapper;
using WeatherApi.Models;

namespace WeatherApi.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly WeatherApiDotComClient _weatherApiDotComClient;
        private readonly IMapper _mapper;

        public WeatherService(WeatherApiDotComClient weatherApiDotComClient, IMapper mapper)
        {
            _weatherApiDotComClient = weatherApiDotComClient;
            _mapper = mapper;
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

            var currentWeatherDto = new CurrentWeatherDTO
            {
                CurrentWeatherResponse = await JsonSerializer.DeserializeAsync<CurrentWeatherResponse>(content, options),
                AstronomyResponse = await GetAstronomy(city),
                TemperatureScale = temperatureScale
            };

            return _mapper.Map<CurrentWeather>(currentWeatherDto);
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