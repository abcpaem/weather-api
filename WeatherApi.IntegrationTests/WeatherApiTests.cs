using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using WeatherApi.Models;
using WeatherApi.Services;
using Xunit;

namespace WeatherApi.IntegrationTests
{
    public class WeatherApiTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public WeatherApiTests(WebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Theory]
        [InlineData("London")]

        public async void Get_WhenCalledWithValidCity_ReturnsCurrentWeather(string city)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"api/weather/{city}");

            var response = _client.SendAsync(request);

            var currentWeather = JsonConvert.DeserializeObject<CurrentWeather>(await response.Result.Content.ReadAsStringAsync());

            Assert.Equal(city, currentWeather.City);
            Assert.NotNull(currentWeather.Region);
            Assert.NotNull(currentWeather.Country);
            Assert.NotNull(currentWeather.LocalTime);
            Assert.Equal(HttpStatusCode.OK, response.Result.StatusCode);
        }

        [Theory]
        [InlineData("0")]

        public async void Get_WhenCalledWithInValidCity_ReturnsNotFound(string city)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"api/weather/{city}");

            var response = _client.SendAsync(request);

            var currentWeather = JsonConvert.DeserializeObject<CurrentWeather>(await response.Result.Content.ReadAsStringAsync());

            Assert.Null(currentWeather.City);
            Assert.Equal(HttpStatusCode.NotFound, response.Result.StatusCode);
        }

        [Theory]
        [InlineData("Madrid")]
        public async void Get_WhenCalledWithValidCityAndScaleFarenheit_ReturnsCurrentWeather(string city)
        {
            var requestCelsius = new HttpRequestMessage(HttpMethod.Get, $"api/weather/{city}?temperatureScale={(int)WeatherService.TemperatureScale.Celsius}");
            var requestFahrenheit = new HttpRequestMessage(HttpMethod.Get, $"api/weather/{city}?temperatureScale={(int)WeatherService.TemperatureScale.Fahrenheit}");

            var response = _client.SendAsync(requestCelsius);
            var currentWeatherInCelsius = JsonConvert.DeserializeObject<CurrentWeather>(await response.Result.Content.ReadAsStringAsync());
            response = _client.SendAsync(requestFahrenheit);
            var currentWeatherInFahrenheit = JsonConvert.DeserializeObject<CurrentWeather>(await response.Result.Content.ReadAsStringAsync());

            var fahrenheitToCelsius = (currentWeatherInFahrenheit.Temperature - 32) / (decimal) 1.8;

            Assert.Equal(fahrenheitToCelsius, currentWeatherInCelsius.Temperature);
        }
    }
}
