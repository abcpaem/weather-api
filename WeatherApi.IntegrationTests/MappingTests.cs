using FluentAssertions;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using WeatherApi.Models;
using Xunit;

namespace WeatherApi.IntegrationTests
{
    public class MappingTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly IMapper _mapper;
        private readonly CurrentWeatherDTO _currentWeatherDTO = new CurrentWeatherDTO
        {
            CurrentWeatherResponse = new CurrentWeatherResponse
            {
                Location = new Location
                {
                    Country = "United Kingdom",
                    Localtime = "2021-01-01 10:00",
                    Name = "London",
                    Region = "City of London, Greater London"
                },
                Current = new Current
                {
                    TemperatureInCelsius = 2,
                    TemperatureInFahrenheit = 35.6m
                }

            },
            AstronomyResponse = new AstronomyResponse
            {
                Astronomy = new Astronomy
                {
                    Astro = new Astro
                    {
                        Sunrise = "07:52 AM",
                        Sunset = "04:32 PM"
                    }
                }
            }
        };

        public MappingTests(WebApplicationFactory<Startup> factory)
        {
            using var scope = factory.Services.CreateScope();
            _mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
        }

        [Theory]
        [InlineData(TemperatureScale.Celsius)]
        [InlineData(TemperatureScale.Fahrenheit)]
        public void CurrentWeatherDTO_isMapped_Into_CurrentWeather(TemperatureScale temperatureScale)
        {
            _currentWeatherDTO.TemperatureScale = (int)temperatureScale;

            var expectedResult = new CurrentWeather
            {
                City = _currentWeatherDTO.CurrentWeatherResponse.Location.Name,
                Country = _currentWeatherDTO.CurrentWeatherResponse.Location.Country,
                LocalTime = _currentWeatherDTO.CurrentWeatherResponse.Location.Localtime,
                Region = _currentWeatherDTO.CurrentWeatherResponse.Location.Region,
                SunRise = _currentWeatherDTO.AstronomyResponse.Astronomy.Astro.Sunrise,
                SunSet = _currentWeatherDTO.AstronomyResponse.Astronomy.Astro.Sunset,
                Temperature = temperatureScale == TemperatureScale.Celsius ?
                    _currentWeatherDTO.CurrentWeatherResponse.Current.TemperatureInCelsius :
                    _currentWeatherDTO.CurrentWeatherResponse.Current.TemperatureInFahrenheit
            };

            var result = _mapper.Map<CurrentWeather>(_currentWeatherDTO);

            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}
