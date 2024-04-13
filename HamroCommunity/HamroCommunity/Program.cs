
using HamroCommunity.Configs;
using HamroCommunity.CustomMiddleware.GlobalErrorHandling;
using Microsoft.OpenApi.Models;
using Project.BLL;
using Project.DLL;
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

    builder.Host.UseSerilog();

    var app = builder.Build();
   
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
