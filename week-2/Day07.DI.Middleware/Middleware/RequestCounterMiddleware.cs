using Day07.DI.Middleware.Services;

namespace Day07.DI.Middleware.Middleware;

/// <summary>
/// Middleware that uses a singleton service to track request count
/// Demonstrates middleware accessing DI services
/// </summary>
public class RequestCounterMiddleware
{
    private readonly RequestDelegate _next;

    public RequestCounterMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IRequestCounterService counterService)
    {
        // Increment the counter for each request
        counterService.Increment();

        // Add request count to response headers
        context.Response.OnStarting(() =>
        {
            context.Response.Headers["X-Request-Count"] = counterService.GetCount().ToString();
            return Task.CompletedTask;
        });

        await _next(context);
    }
}

public static class RequestCounterMiddlewareExtensions
{
    public static IApplicationBuilder UseRequestCounter(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RequestCounterMiddleware>();
    }
}
