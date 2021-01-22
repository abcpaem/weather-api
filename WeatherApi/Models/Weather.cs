using System.Text.Json.Serialization;

namespace WeatherApi.Models
{
    public enum TemperatureScale
    {
        Celsius,
        Fahrenheit
    }

    public class CurrentWeather
    {
        public string City { get; set; }

        public string Region { get; set; }

        public string Country { get; set; }

        public string LocalTime { get; set; }

        public decimal Temperature { get; set; }

        public string SunRise { get; set; }

        public string SunSet { get; set; }
    }

    public class CurrentWeatherDTO
    {
        public CurrentWeatherResponse CurrentWeatherResponse { get; set; }
        public AstronomyResponse AstronomyResponse { get; set; }
        public int TemperatureScale { get; set; }
    }

    public class CurrentWeatherResponse
    {
        public Location Location { get; set; }
        public Current Current { get; set; }
    }

    public class Location
    {
        public string Name { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        public string Localtime { get; set; }
    }

    public class Current
    {
        [JsonPropertyName("temp_c")]
        public decimal TemperatureInCelsius { get; set; }

        [JsonPropertyName("temp_f")]
        public decimal TemperatureInFahrenheit { get; set; }
    }

    public class AstronomyResponse
    {
        public Astronomy Astronomy { get; set; }
    }

    public class Astronomy
    {
        public Astro Astro { get; set; }
    }

    public class Astro
    {
        public string Sunrise { get; set; }
        public string Sunset { get; set; }
    }
}
