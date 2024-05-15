
using HamroCommunity.Configs;
using HamroCommunity.CustomMiddleware.GlobalErrorHandling;
using Microsoft.OpenApi.Models;
using Project.BLL;
using Project.DLL;
using Project.DLL.Seed;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerUI;

try
{
    var builder = WebApplication.CreateBuilder(args);

    ConfigurationManager configuration = builder.Configuration;
    builder.Services
        .AddBLL()
        .AddDAL(configuration);

    // Add services to the container.
    Dependencies.Inject(builder);

    Log.Information("Application StartUp");

    var app = builder.Build();

    using(var scope = app.Services.CreateScope())
    {
        var dataSeeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
        await dataSeeder.Seed();
    }
   
    ApplicationConfiguration.Configure(app); //Configurations

    app.Run();
}
catch(Exception ex)
{
    Log.Error("The following {Exception} was thrown during Application Startup", ex);
}
finally
{
    Log.CloseAndFlush();
}
