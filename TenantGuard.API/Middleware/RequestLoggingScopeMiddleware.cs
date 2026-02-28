using System.Diagnostics;
using TenantGuard.Application.Context;

namespace TenantGuard.API.Middleware;

public sealed class RequestLoggingScopeMiddleware(
    RequestDelegate next,
    ILogger<RequestLoggingScopeMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext httpContext, ITenantContext tenantContext)
    {
        var traceId = Activity.Current?.Id ?? httpContext.TraceIdentifier;

        using var scope = logger.BeginScope(new Dictionary<string, object>
        {
            ["TenantId"] = tenantContext.TenantId,
            ["TraceId"] = traceId
        });

        if (logger.IsEnabled(LogLevel.Information))
            logger.LogInformation("Handling request {Method} {Path}", httpContext.Request.Method, httpContext.Request.Path);

        await next(httpContext);

        if (logger.IsEnabled(LogLevel.Information))
            logger.LogInformation(
                "Handled request {Method} {Path} with status {StatusCode}",
                httpContext.Request.Method,
                httpContext.Request.Path,
                httpContext.Response.StatusCode);
    }
}
