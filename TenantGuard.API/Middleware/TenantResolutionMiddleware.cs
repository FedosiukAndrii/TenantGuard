using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TenantGuard.Application.Context;

namespace TenantGuard.API.Middleware;

public sealed class TenantResolutionMiddleware(RequestDelegate next)
{
    private const string TenantHeaderName = "X-Tenant-Id";
    private const string UnauthorizedType = "https://httpstatuses.com/401";
    private const string BadRequestType = "https://httpstatuses.com/400";
    private const string ForbiddenType = "https://httpstatuses.com/403";
    private static readonly string[] TenantClaimTypes = ["tenant_id", "tenantId", "TenantId"];

    public async Task InvokeAsync(
        HttpContext httpContext,
        ITenantContext tenantContext,
        IProblemDetailsService problemDetailsService)
    {
        if (IsBypassedPath(httpContext.Request.Path))
        {
            await next(httpContext);
            return;
        }

        if (!TryGetTenantId(httpContext, out var tenantId))
        {
            await WriteProblemAsync(
                httpContext,
                problemDetailsService,
                StatusCodes.Status400BadRequest,
                BadRequestType,
                "Bad Request",
                $"Missing or invalid {TenantHeaderName} header.");
            return;
        }

        if (!HasMatchingTokenTenant(httpContext.User, tenantId, out var statusCode, out var detail))
        {
            await WriteProblemAsync(
                httpContext,
                problemDetailsService,
                statusCode,
                statusCode == StatusCodes.Status401Unauthorized ? UnauthorizedType : ForbiddenType,
                statusCode == StatusCodes.Status401Unauthorized ? "Unauthorized" : "Forbidden",
                detail);
            return;
        }

        tenantContext.SetTenant(tenantId);

        var externalUserId = ResolveExternalUserId(httpContext.User);

        if (!string.IsNullOrWhiteSpace(externalUserId)) 
            tenantContext.SetUser(externalUserId);

        await next(httpContext);
    }

    private static bool TryGetTenantId(HttpContext httpContext, out Guid tenantId)
    {
        tenantId = Guid.Empty;

        return httpContext.Request.Headers.TryGetValue(TenantHeaderName, out var tenantHeaderValue)
            && Guid.TryParse(tenantHeaderValue, out tenantId);
    }

    private static string ResolveExternalUserId(ClaimsPrincipal user) =>
        user.FindFirstValue(ClaimTypes.NameIdentifier) ??
        user.FindFirstValue("sub");

    private static bool HasMatchingTokenTenant(
        ClaimsPrincipal user,
        Guid tenantId,
        out int statusCode,
        out string detail)
    {
        statusCode = StatusCodes.Status403Forbidden;
        detail = $"Authenticated tenant context must match the {TenantHeaderName} header.";

        if (user.Identity?.IsAuthenticated != true)
        {
            statusCode = StatusCodes.Status401Unauthorized;
            detail = "Tenant-scoped requests require an authenticated user.";
            return false;
        }

        var tenantClaim = TenantClaimTypes
            .Select(user.FindFirstValue)
            .FirstOrDefault(value => !string.IsNullOrWhiteSpace(value));

        if (string.IsNullOrWhiteSpace(tenantClaim))
        {
            detail = "Authenticated tenant requests must include a tenant claim.";
            return false;
        }

        if (!Guid.TryParse(tenantClaim, out var tokenTenantId))
        {
            detail = "Authenticated tenant requests must include a valid tenant claim.";
            return false;
        }

        if (tokenTenantId == tenantId)
            return true;

        detail = $"The {TenantHeaderName} header does not match the authenticated tenant claim.";
        return false;
    }

    private static async Task WriteProblemAsync(
        HttpContext httpContext,
        IProblemDetailsService problemDetailsService,
        int statusCode,
        string type,
        string title,
        string detail)
    {
        httpContext.Response.StatusCode = statusCode;

        await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails = new ProblemDetails
            {
                Type = type,
                Title = title,
                Status = statusCode,
                Detail = detail,
                Instance = httpContext.Request.Path
            }
        });
    }

    private static bool IsBypassedPath(PathString path) =>
        path.StartsWithSegments("/openapi", StringComparison.OrdinalIgnoreCase) ||
        path.StartsWithSegments("/swagger", StringComparison.OrdinalIgnoreCase) ||
        path.StartsWithSegments("/health", StringComparison.OrdinalIgnoreCase) ||
        path.StartsWithSegments("/host", StringComparison.OrdinalIgnoreCase) ||
        path.StartsWithSegments("/.well-known", StringComparison.OrdinalIgnoreCase);
}