using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WeatherApi.Models;
using WeatherApi.Services;

namespace WeatherApi.Controllers
{
    [ApiController]
    [Route("api/weather")]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherService _weatherService;

        public WeatherController(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        /// <summary>
        /// Gets the current weather conditions for the city specified.
        /// </summary>
        /// <remarks>
        /// You can choose the scale for the temperature returned between the Celsius scale (°C) or the Fahrenheit scale. You can also see sunrise and sunset informantion for the city specified.
        /// </remarks>
        /// <param name="city">The city to get the weather for.</param>
        /// <param name="temperatureScale">Specifies the temperature scale to use: (optional parameter)
        /// 
        /// 0 - Celsius (default value if not provided)
        /// 1 - Fahrenheit
        /// </param>
        /// <response code="200">Response when the current weather conditions are retrieved successfully.</response>
        /// <response code="404">Response when no weather data is found for the city provided.</response>
        /// <response code="500">Response when something has gone wrong in the weather request.</response>
        [HttpGet("{city}")]
        [ProducesResponseType(typeof(CurrentWeather), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, VaryByQueryKeys = new[] { "temperatureScale" })]
        public async Task<IActionResult> Get([FromRoute] string city, [FromQuery] int temperatureScale)
        {
            var currentWeather = await _weatherService.GetCurrentWeather(city, temperatureScale);

            if (currentWeather == null)
            {
                return NotFound();
            }

            return Ok(currentWeather);
        }
    }
}
