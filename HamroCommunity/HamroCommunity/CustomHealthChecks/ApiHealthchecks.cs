using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace HamroCommunity.CustomHealthChecks
{
    public class ApiHealthchecks : IHealthCheck
    {
        private readonly HttpClient _httpClient;
        public ApiHealthchecks(HttpClient httpClient)
        {
            _httpClient = httpClient;
    
        }
     
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.GetAsync("https://localhost:7202/api/Location/Province/get-all", cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                return await Task.FromResult(new HealthCheckResult(
                    status: HealthStatus.Healthy,
                    description: "The Api is Up and Running"
                    ));
            }
            return await Task.FromResult(new HealthCheckResult(
                status: HealthStatus.Unhealthy,
                description:"The API is down"
                ));
        }
    }
}
