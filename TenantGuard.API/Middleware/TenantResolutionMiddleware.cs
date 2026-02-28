using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TenantGuard.Application.Context;

namespace TenantGuard.API.Middleware;

public sealed class TenantResolutionMiddleware(RequestDelegate next)
{
    private const string TenantHeaderName = "X-Tenant-Id";
    private const string BadRequestType = "https://httpstatuses.com/400";

    public async Task InvokeAsync(HttpContext httpContext, ITenantContext tenantContext)
    {
        if (IsBypassedPath(httpContext.Request.Path))
        {
            await next(httpContext);
            return;
        }

        if (!TryGetTenantId(httpContext, out var tenantId))
        {
            await WriteMissingOrInvalidTenantProblemAsync(httpContext);
            return;
        }

        tenantContext.SetTenant(tenantId);

        var externalUserId = ResolveExternalUserId(httpContext.User);

        if (!string.IsNullOrWhiteSpace(externalUserId))
        {
            tenantContext.SetUser(externalUserId);
        }

        await next(httpContext);
    }

    private static bool TryGetTenantId(HttpContext httpContext, out Guid tenantId)
    {
        tenantId = Guid.Empty;

        if (!httpContext.Request.Headers.TryGetValue(TenantHeaderName, out var tenantHeaderValue))
        {
            return false;
        }

        return Guid.TryParse(tenantHeaderValue, out tenantId);
    }

    private static string? ResolveExternalUserId(ClaimsPrincipal user) =>
        user.FindFirstValue(ClaimTypes.NameIdentifier) ??
        user.FindFirstValue("sub");

    private static async Task WriteMissingOrInvalidTenantProblemAsync(HttpContext httpContext)
    {
        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        httpContext.Response.ContentType = "application/problem+json";

        var problemDetails = new ProblemDetails
        {
            Type = BadRequestType,
            Title = "Bad Request",
            Status = StatusCodes.Status400BadRequest,
            Detail = $"Missing or invalid {TenantHeaderName} header.",
            Instance = httpContext.Request.Path
        };

        await httpContext.Response.WriteAsJsonAsync(problemDetails);
    }

    private static bool IsBypassedPath(PathString path) =>
        path.StartsWithSegments("/openapi", StringComparison.OrdinalIgnoreCase) ||
        path.StartsWithSegments("/swagger", StringComparison.OrdinalIgnoreCase) ||
        path.StartsWithSegments("/health", StringComparison.OrdinalIgnoreCase) ||
        path.StartsWithSegments("/host", StringComparison.OrdinalIgnoreCase) ||
        path.StartsWithSegments("/.well-known", StringComparison.OrdinalIgnoreCase);
}