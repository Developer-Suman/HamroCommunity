

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

    builder.Services.AddControllers();


    builder.Services.AddEndpointsApiExplorer();

    #region ConfigureSwaggerForAuthentication
    builder.Services.AddSwaggerGen(
option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "HAMROCOMMUNITY API", Version = "V1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type=ReferenceType.SecurityScheme,
                                    Id="Bearer"
                                }
                            },
                            Array.Empty<string>()
                        }
    });
}
);
    #endregion










    var app = builder.Build();

    // Configure the HTTP request pipeline.


  

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

  


    #region RedirectSwagger
    //Redirect request from the root Url to swagger UI
    app.Use(async (context, next) =>
    {
        if (context.Request.Path == "/")
        {
            context.Response.Redirect("/swagger/index.html");
            return;
        }

        await next();

    });
    #endregion

    #region HttpRequestPipeLine For Swagger
    if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Simple Api");
            c.DocExpansion(DocExpansion.None);

        });

    }

    #endregion


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
