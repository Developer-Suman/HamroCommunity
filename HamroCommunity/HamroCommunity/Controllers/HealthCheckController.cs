using HamroCommunity.CustomHealthChecks;
using HamroCommunity.CustomHealthChecks.HealthChecksEndPoints;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Project.DLL.Models;
using System.Text.Json;

namespace HamroCommunity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAllOrigins")]
    public class HealthCheckController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly HealthChecksEndPointConfig _endPointConfig;
        public HealthCheckController(IOptions<HealthChecksEndPointConfig> endPointsConfig)
        {
            _endPointConfig = endPointsConfig.Value;
            _httpClient = new HttpClient();

        }

        [HttpGet]
        public async Task<IActionResult> CheckHealthStatus()
        {
            var apiHealthchecks = new ApiHealthchecks(_httpClient, Options.Create(_endPointConfig));

            var result = await apiHealthchecks.CheckHealthAsync(new HealthCheckContext());

            return result.Status == HealthStatus.Healthy ? Ok("All APIs are up and running") : StatusCode(503, result);

        }
    }
}
