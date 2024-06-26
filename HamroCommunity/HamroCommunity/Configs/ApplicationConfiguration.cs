﻿using HamroCommunity.CustomMiddleware.GlobalErrorHandling;
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

      

            app.UseStaticFiles();
            app.UseHttpsRedirection();

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
