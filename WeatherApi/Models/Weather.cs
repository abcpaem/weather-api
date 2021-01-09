using System.Text.Json.Serialization;

namespace WeatherApi.Models
{
    public class CurrentWeather
    {
        public string City { get; set; }

        public string Region { get; set; }

        public string Country { get; set; }

        public string LocalTime { get; set; }

        public decimal Temperature { get; set; }
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
        // Temperature in Celsius
        [JsonPropertyName("temp_c")]
        public decimal Temperature { get; set; }
    }
}
