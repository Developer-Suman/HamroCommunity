using AutoMapper;
using HamroCommunity.Configs;
using HamroCommunity.CustomMiddleware.CustomException;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.DLL.Models;
using Serilog;
using System.Text.Json;

namespace HamroCommunity.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : HamroCommunityBaseController
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IMapper mapper, UserManager<ApplicationUsers> userManager, RoleManager<IdentityRole> roleManager) : base(mapper, userManager, roleManager) 
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var user = GetCurrentUser();
            var Roles = GetCurrentUserRoles();
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