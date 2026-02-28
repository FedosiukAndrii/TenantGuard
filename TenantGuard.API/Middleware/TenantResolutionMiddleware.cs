using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TenantGuard.Application.Context;

namespace TenantGuard.API.Middleware;

public sealed class TenantResolutionMiddleware(RequestDelegate next)
{
    private const string TenantHeaderName = "X-Tenant-Id";

    public async Task InvokeAsync(HttpContext httpContext, ITenantContext tenantContext)
    {
        if (IsBypassedPath(httpContext.Request.Path))
        {
            await next(httpContext);
            return;
        }

        if (!httpContext.Request.Headers.TryGetValue(TenantHeaderName, out var tenantHeaderValue) ||
            !Guid.TryParse(tenantHeaderValue, out var tenantId))
        {
            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            httpContext.Response.ContentType = "application/problem+json";

            var problemDetails = new ProblemDetails
            {
                Type = "https://httpstatuses.com/400",
                Title = "Bad Request",
                Status = StatusCodes.Status400BadRequest,
                Detail = $"Missing or invalid {TenantHeaderName} header.",
                Instance = httpContext.Request.Path
            };

            await httpContext.Response.WriteAsJsonAsync(problemDetails);
            return;
        }

        tenantContext.SetTenant(tenantId);

        var externalUserId =
            httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ??
            httpContext.User.FindFirstValue("sub");

        if (!string.IsNullOrWhiteSpace(externalUserId))
            tenantContext.SetUser(externalUserId);

        await next(httpContext);
    }

    private static bool IsBypassedPath(PathString path) =>
        path.StartsWithSegments("/openapi", StringComparison.OrdinalIgnoreCase) ||
        path.StartsWithSegments("/throw", StringComparison.OrdinalIgnoreCase);
}