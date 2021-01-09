using Microsoft.AspNetCore.Mvc;
using Moq;
using WeatherApi.Controllers;
using WeatherApi.Models;
using WeatherApi.Services;
using Xunit;

namespace WeatherApi.UnitTests
{
    public class WeatherControllerTests
    {
        const int CelsiusScale = (int)WeatherService.TemperatureScale.Celsius;

        [Fact]
        public void Get_WhenCalledWithValidCity_ReturnsOk()
        {
            var mockWeatherService = new Mock<IWeatherService>();
            mockWeatherService.Setup(s => s.GetCurrentWeather(It.IsAny<string>(), CelsiusScale)).ReturnsAsync(new CurrentWeather());
            var weatherController = new WeatherController(mockWeatherService.Object);

            var result = weatherController.Get(It.IsAny<string>(), CelsiusScale);

            mockWeatherService.Verify(x => x.GetCurrentWeather(It.IsAny<string>(), 0), Times.Once);
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public void Get_WhenCalledWithInValidCity_ReturnsNotFound()
        {
            var mockWeatherService = new Mock<IWeatherService>();
            mockWeatherService.Setup(s => s.GetCurrentWeather(It.IsAny<string>(), CelsiusScale)).ReturnsAsync((CurrentWeather)null);
            var weatherController = new WeatherController(mockWeatherService.Object);

            var result = weatherController.Get(It.IsAny<string>(), CelsiusScale);

            mockWeatherService.Verify(x => x.GetCurrentWeather(It.IsAny<string>(), CelsiusScale), Times.Once);
            Assert.IsType<NotFoundResult>(result.Result);
        }
    }
}
