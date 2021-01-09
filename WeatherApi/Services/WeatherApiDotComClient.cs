using System;
using System.Net.Http;
using Microsoft.Extensions.Options;
using WeatherApi.Models;

namespace WeatherApi.Services
{
    public class WeatherApiDotComClient
    {
        public readonly HttpClient Client;

        public WeatherApiDotComClient(HttpClient client, IOptions<WeatherApiDotComSettings> weatherApiDotComSettings)
        {
            Client = client;
            Client.BaseAddress = new Uri(weatherApiDotComSettings.Value.Uri);
            Client.DefaultRequestHeaders.Add("key", weatherApiDotComSettings.Value.Key);
        }
    }
}
