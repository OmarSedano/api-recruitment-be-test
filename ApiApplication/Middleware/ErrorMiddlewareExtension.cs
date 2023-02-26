using Microsoft.AspNetCore.Builder;

namespace ApiApplication.Middleware
{
    public static  class ErrorMiddlewareExtension
    {
        public static IApplicationBuilder UseApiErrorHandler(this IApplicationBuilder app)
        {
            app.UseMiddleware<ErrorHandlerMiddleware>();
            return app;
        }
    }
}
