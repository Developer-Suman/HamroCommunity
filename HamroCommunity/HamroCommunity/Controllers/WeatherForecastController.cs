using HamroCommunity.CustomMiddleware.CustomException;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Text.Json;

namespace HamroCommunity.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
           
            _logger.LogInformation("You requested the Get Action of WeatherForecasting");

            //throw new Exception("An error occured");
            var result =  Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();

            var resultJson = JsonSerializer.Serialize(result);
            _logger.LogInformation("Weather Forecasting => {@result}", resultJson);

            return result;
        }
    }
}