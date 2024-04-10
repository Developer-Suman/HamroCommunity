using HamroCommunity.CustomMiddleware.CustomException;
using Microsoft.AspNetCore.Http;
using Project.DLL.Models;
using Serilog;
using System.Net;

namespace HamroCommunity.CustomMiddleware.GlobalErrorHandling
{
    
    public class ExceptionMiddleware
    {
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _logger = logger;
            _next = next;

        }


        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);

            }catch (UnauthorizedAccessException ex)
            {
                await HandleExceptionAsync(httpContext, ex, HttpStatusCode.Unauthorized);
            }catch(DirectoryNotFoundException ex)
            {
                await HandleExceptionAsync(httpContext,ex , HttpStatusCode.NotFound);
            }catch(NotFoundException ex)
            {
                await HandleExceptionAsync(httpContext ,ex , ex.StatusCode);
            }
            catch(Exception ex)
            {
                await HandleExceptionAsync(httpContext,ex , HttpStatusCode.InternalServerError);
            }
        }

        private async Task HandleExceptionAsync(HttpContext httpContext, Exception ex, HttpStatusCode statusCodes)
        {
            httpContext.Response.StatusCode = (int)statusCodes;
            httpContext.Response.ContentType = "application/json";

            Log.Error("An exception occured:{Exception}", ex);

            await httpContext.Response.WriteAsync(new ErrorDetails
            {
                StatusCode = (int)statusCodes,
                Message = ex.Message,

            }.ToString());

        }
    }

    //Extension method used to add the middleware to the Http Request pipeline
    public static class ErrorHandlingMiddlewareExtensions
    {
        public static void UseErrorHandlingMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }


}
