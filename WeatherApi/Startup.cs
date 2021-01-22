using System;
using System.IO;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using WeatherApi.Models;
using WeatherApi.Services;

namespace WeatherApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.Configure<WeatherApiDotComSettings>(options => Configuration.GetSection("WeatherApiDotCom").Bind(options));

            services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc("v1", new OpenApiInfo { Title = "Weather API" });

                var filePath = Path.Combine(AppContext.BaseDirectory, "WeatherApi.xml");
                swagger.IncludeXmlComments(filePath);
            });

            services.AddScoped<IWeatherService, WeatherService>();
            
            services.AddHttpClient<WeatherApiDotComClient>();

            services.AddSingleton(ConfiguredMapping());
            services.AddScoped<IMapper, ServiceMapper>();
        }

        private TypeAdapterConfig ConfiguredMapping()
        {
            var config = new TypeAdapterConfig();

            config.NewConfig<CurrentWeatherDTO, CurrentWeather>()
                .NameMatchingStrategy(NameMatchingStrategy.IgnoreCase)
                .Map(dest => dest.City, src => src.CurrentWeatherResponse.Location.Name)
                .Map(dest => dest, src => src.CurrentWeatherResponse.Location)
                .Map(dest => dest, src => src.AstronomyResponse.Astronomy.Astro)
                .Map(dest => dest.Temperature, src => src.TemperatureScale == (int) TemperatureScale.Fahrenheit
                    ? src.CurrentWeatherResponse.Current.TemperatureInFahrenheit
                    : src.CurrentWeatherResponse.Current.TemperatureInCelsius);

            return config;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Weather API");
            });
        }
    }
}
