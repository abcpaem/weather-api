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
        /// The temperature provided is on the Celsius scale (°C) or also known as centigrade scale.
        /// </remarks>
        /// <param name="city">The city to get the weather for.</param>
        /// <response code="200">Response when the current weather conditions are retrieved successfully.</response>
        /// <response code="404">Response when no weather data is found for the city provided.</response>
        /// <response code="500">Response when something has gone wrong in the weather request.</response>
        [HttpGet("{city}")]
        [ProducesResponseType(typeof(CurrentWeather), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get([FromRoute] string city)
        {
            var currentWeather = await _weatherService.GetCurrentWeather(city);

            if (currentWeather == null)
            {
                return NotFound();
            }

            return Ok(currentWeather);
        }
    }
}
