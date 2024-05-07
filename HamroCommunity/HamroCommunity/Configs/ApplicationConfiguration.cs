using HamroCommunity.CustomMiddleware.GlobalErrorHandling;
using Serilog;
using Serilog.Events;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace HamroCommunity.Configs
{
    public static class ApplicationConfiguration
    {
        public static void Configure(WebApplication app)
        {
            //app.UseSerilogRequestLogging();
            app.Use((context, next) =>
            {
                context.Response.Headers.Remove("X-Powered-By");
                context.Response.Headers.Remove("Server");
                context.Response.Headers.Add("X-Frame-Options", "DENY");
                return next();
            });

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

            // Use Serilog for logging
            //app.UseSerilogRequestLogging(options =>
            //{
            //    options.GetLevel = (httpContext, elapsed, ex) =>
            //    {
            //        // Customize log level based on request
            //        if (httpContext.Response.StatusCode > 499)
            //        {
            //            return LogEventLevel.Error;
            //        }
            //        else if (httpContext.Response.StatusCode > 399)
            //        {
            //            return LogEventLevel.Warning;
            //        }

            //        return LogEventLevel.Information;
            //    };

            //    options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
            //    {
            //        // Include custom properties in the log context
            //        // Include properties you need
            //        diagnosticContext.Set("RequestHost", httpContext.Request.Host);
            //        diagnosticContext.Set("RequestMethod", httpContext.Request.Method);
            //        diagnosticContext.Set("RequestPath", httpContext.Request.Path);
            //        diagnosticContext.Set("StatusCode", httpContext.Response.StatusCode);
            //        diagnosticContext.Set("ElapsedMilliseconds", httpContext.Response.Headers["X-Request-ElapsedMs"]);

            //        // Add more properties as needed
            //    };

            //});

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            //app.UseSerilogRequestLogging();

            app.UseCors("AllowAllOrigins"); //UseCors must be placed after UseRouting an before UseAuthorization
            //This is to ensure the cors headers are included in the response for both authorized and unauthorized calls

            app.UseAuthentication();

            app.UseAuthorization();

            

            app.UseMiddleware<ExceptionMiddleware>();

            app.MapControllers();
        }
    }
}
