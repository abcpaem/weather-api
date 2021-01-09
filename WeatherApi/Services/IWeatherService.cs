using System.Threading.Tasks;
using WeatherApi.Models;

namespace WeatherApi.Services
{
    public interface IWeatherService
    {
        Task<CurrentWeather> GetCurrentWeather(string city);
    }
}