using Microsoft.AspNetCore.Builder;

namespace ApiApplication.Middleware
{
    public static  class LoggingMiddlewareExtension
    {
        public static IApplicationBuilder UseApiLogging(this IApplicationBuilder app)
        {
            app.UseMiddleware<LoggingMiddleware>();
            return app;
        }
    }
}
