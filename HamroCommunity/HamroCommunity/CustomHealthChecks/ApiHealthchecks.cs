using HamroCommunity.CustomHealthChecks.HealthChecksEndPoints;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using System.Net.Http;

namespace HamroCommunity.CustomHealthChecks
{
    public class ApiHealthchecks : IHealthCheck
    {
        private readonly HttpClient _httpClient;
        private readonly HealthChecksEndPointConfig _endPointConfig;
        public ApiHealthchecks(HttpClient httpClient, IOptions<HealthChecksEndPointConfig> endPointsConfig)
        {
            _endPointConfig = endPointsConfig.Value;
            _httpClient = httpClient;
    
        }
     
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            if (_endPointConfig?.Endpoints == null || !_endPointConfig.Endpoints.Any())
            {
                return HealthCheckResult.Unhealthy("Endpoints configuration is missing or empty.");
            }

            var healthCheckTasks = _endPointConfig.Endpoints
                .Select(endpoint => CheckEndpointHealthAsync(endpoint, cancellationToken))
                .ToArray();

            var healthCheckResults = await Task.WhenAll(healthCheckTasks);

            var unhealthyEndpoints = healthCheckResults
                .Where(result => !result.IsHealthy)
                .Select(result => result.Endpoint)
                .ToList();

            if (unhealthyEndpoints.Count == 0)
            {
                return HealthCheckResult.Healthy("All APIs are up and running");
            }
            else
            {
                var description = $"The following endpoints are down: {string.Join(", ", unhealthyEndpoints)}";
                return HealthCheckResult.Unhealthy(description);
            }


        }

        private async Task<EndpointHealthResult> CheckEndpointHealthAsync(string endpoint, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _httpClient.GetAsync(endpoint, cancellationToken);
                return new EndpointHealthResult
                {
                    Endpoint = endpoint,
                    IsHealthy = response.IsSuccessStatusCode
                };
            }
            catch
            {
                return new EndpointHealthResult
                {
                    Endpoint = endpoint,
                    IsHealthy = false
                };
            }
        }

        private class EndpointHealthResult
        {
            public string Endpoint { get; set; }
            public bool IsHealthy { get; set; }
        }
    }

    
}
