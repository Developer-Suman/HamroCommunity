using Project.DLL.Models.GlobalModel;
using Serilog;

namespace HamroCommunity.Configs
{
    public static class ServiceCollectionConfigure
    {
        public static IServiceCollection ServiceCollectionConfiguration(this IServiceCollection services)
        {
            services.AddSingleton<RequestLoggingOptions>();
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders(); // Clear other logging providers
                loggingBuilder.AddSerilog(dispose: true); // Add Serilog
            });

            return services;
        }
    }
}
