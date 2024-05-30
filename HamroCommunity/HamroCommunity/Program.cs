
using HamroCommunity.Configs;
using HamroCommunity.CustomMiddleware.GlobalErrorHandling;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.OpenApi.Models;
using Project.BLL;
using Project.DLL;
using Project.DLL.Seed;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerUI;

//try
//{

//}
//catch(Exception ex)
//{
//    Log.Error("The following {Exception} was thrown during Application Startup", ex);
//}
//finally
//{
//    Log.CloseAndFlush();
//}
var builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration;
builder.Services
    .AddDAL(configuration)
    .AddBLL();

//Configure RateLimiter
builder.Services.AddRateLimiter(config =>
{
    config.AddFixedWindowLimiter("FixedWindowPolicy", options =>
    {
        options.Window = TimeSpan.FromSeconds(5);
        options.PermitLimit = 3;
        options.QueueLimit = 0;
        options.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;

    }).RejectionStatusCode = 429;
});


builder.Services.AddRateLimiter(config =>
{
    config.AddSlidingWindowLimiter("SlidingWindowPolicy", options =>
    {
        options.Window = TimeSpan.FromSeconds(15);
        options.PermitLimit = 3;
        options.QueueLimit = 2;
        options.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
        options.SegmentsPerWindow = 3;
    }).RejectionStatusCode=  429;

});

builder.Services.AddRateLimiter(config =>
{
    config.AddTokenBucketLimiter("TokenBucketPolicy", options =>
    {
        options.ReplenishmentPeriod = TimeSpan.FromSeconds(10);
        options.TokenLimit = 3;
        options.QueueLimit= 2;
        options.TokensPerPeriod = 2;
        options.AutoReplenishment = true;
        options.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
    }).RejectionStatusCode = 429;
});


builder.Services.AddRateLimiter(config =>
{
    config.AddConcurrencyLimiter("ConcurrencyPolicy", options =>
    {
        options.PermitLimit = 3;
        options.QueueLimit = 0;
        options.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;

    }).RejectionStatusCode = 429;
});

// Add services to the container.
Dependencies.Inject(builder);

Log.Information("Application StartUp");

var app = builder.Build();
app.UseRateLimiter();


using (var scope = app.Services.CreateScope())
{
    var dataSeeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
    await dataSeeder.Seed();
}

ApplicationConfiguration.Configure(app); //Configurations

app.Run();