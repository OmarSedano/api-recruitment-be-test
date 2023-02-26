using ApiApplication.Exceptions;
using Microsoft.AspNetCore.Http;
using System;
using System.Diagnostics;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;

namespace ApiApplication.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {

            var watch = Stopwatch.StartNew();

            await _next(context);

            var duration = watch.ElapsedMilliseconds;
            watch.Stop();
            _logger.LogInformation($"Request. {context.Request.Method} {context.Request.GetDisplayUrl()}. Execution Time: {duration} ms");
        }
    }
}
