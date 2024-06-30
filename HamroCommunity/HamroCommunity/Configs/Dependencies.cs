using Microsoft.OpenApi.Models;
using Serilog;

namespace HamroCommunity.Configs
{
    public static class Dependencies
    {
        public static void Inject(WebApplicationBuilder builder)
        {
            //builder.Services.AddControllers().AddNewtonsoftJson();
            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();

            #region SeriaLogConfiguration
            Log.Logger = new LoggerConfiguration()
               .ReadFrom.Configuration(builder.Configuration).CreateLogger();
            #region UseSerialog for DI
            // Register Serilog with DI
            //Use both serialog and BuildIn Parallery
            builder.Services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddSerilog(dispose: true);
            });
            #endregion

            #endregion

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

        }
    }
}
