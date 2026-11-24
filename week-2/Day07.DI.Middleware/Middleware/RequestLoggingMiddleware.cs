using System.Diagnostics;

namespace Day07.DI.Middleware.Middleware;

/// <summary>
/// Custom middleware to log request/response metadata safely
/// Demonstrates middleware pipeline and ordering
/// </summary>
public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        var requestId = Guid.NewGuid();

        // Log request details
        using (_logger.BeginScope(new Dictionary<string, object>
        {
            ["RequestId"] = requestId,
            ["RequestPath"] = context.Request.Path,
            ["RequestMethod"] = context.Request.Method
        }))
        {
            _logger.LogInformation(
                "HTTP {RequestMethod} {RequestPath} started",
                context.Request.Method,
                context.Request.Path);

            // Store request time in context for later use
            context.Items["RequestId"] = requestId;
            context.Items["RequestStartTime"] = stopwatch.Elapsed;

            try
            {
                // Call the next middleware in the pipeline
                await _next(context);

                stopwatch.Stop();

                // Log response details
                _logger.LogInformation(
                    "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {ElapsedMilliseconds}ms",
                    context.Request.Method,
                    context.Request.Path,
                    context.Response.StatusCode,
                    stopwatch.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                stopwatch.Stop();

                _logger.LogError(
                    ex,
                    "HTTP {RequestMethod} {RequestPath} failed in {ElapsedMilliseconds}ms",
                    context.Request.Method,
                    context.Request.Path,
                    stopwatch.ElapsedMilliseconds);

                throw;
            }
        }
    }
}

/// <summary>
/// Extension method to register the middleware
/// </summary>
public static class RequestLoggingMiddlewareExtensions
{
    public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RequestLoggingMiddleware>();
    }
}
