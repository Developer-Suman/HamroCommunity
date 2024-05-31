//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Filters;
//using System.Net;
//using System.Threading.RateLimiting;

//namespace HamroCommunity.CustomAttributes
//{
//    public class RateLimitMiddleware
//    {
//        private readonly RequestDelegate _next;
//        private readonly RateLimiter _rateLimiter;

//        public RateLimitMiddleware(RequestDelegate next, RateLimiter rateLimiter)
//        {
//            _next = next;
//            _rateLimiter = rateLimiter;
//        }

//        public async Task InvokeAsync(HttpContext context)
//        {
//            var rateLimitLease = await _rateLimiter.AcquireAsync(3);

//            if (!rateLimitLease.IsAcquired)
//            {
//                context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
//                await context.Response.WriteAsync("Too Many Requests");
//                return;
//            }

//            try
//            {
//                await _next(context);
//            }
//            finally
//            {
//                rateLimitLease.Dispose();
//            }
//        }
//    }

//    public static class RateLimitMiddlewareExtensions
//    {
//        public static IApplicationBuilder UseRateLimitMiddleware(this IApplicationBuilder builder)
//        {
//            return builder.UseMiddleware<RateLimitMiddleware>();
//        }
//    }


//}
