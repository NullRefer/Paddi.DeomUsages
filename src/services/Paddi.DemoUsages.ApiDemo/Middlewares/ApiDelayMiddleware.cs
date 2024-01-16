using System.Diagnostics;

using Microsoft.AspNetCore.Http.Extensions;

namespace Paddi.DemoUsages.ApiDemo.Middlewares;

public class ApiDelayMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ApiDelayMiddleware> _logger;

    public ApiDelayMiddleware(RequestDelegate next, ILogger<ApiDelayMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var sp = new Stopwatch();
        _logger.LogInformation("{Api} request start at {Now}", context.Request.GetDisplayUrl(), DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff"));
        sp.Start();
        await _next(context);
        sp.Stop();
        _logger.LogInformation("{Api} request end at {Now}, time elapsed: {Duration}ms",
                               context.Request.GetDisplayUrl(), DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff"), sp.ElapsedMilliseconds);
    }
}

public static class ApiDelayMiddlewareExtension
{
    public static IApplicationBuilder UseApiDelayMiddleware(this IApplicationBuilder builder) => builder.UseMiddleware<ApiDelayMiddleware>();
}
