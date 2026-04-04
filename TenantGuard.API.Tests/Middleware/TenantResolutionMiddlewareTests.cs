using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using TenantGuard.API.Tests.Testing;
using Xunit;

namespace TenantGuard.API.Tests.Middleware;

public sealed class TenantResolutionMiddlewareTests(TenantGuardApiFactory factory): IClassFixture<TenantGuardApiFactory>
{
    [Fact]
    public async Task UnauthenticatedTenantRequest_ReturnsUnauthorizedProblemDetails()
    {
        using var client = CreateClient();
        using var request = CreateRequest(requestTenantId: Guid.NewGuid().ToString());

        using var response = await client.SendAsync(request);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);

        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        Assert.NotNull(problem);
        Assert.Equal((int)HttpStatusCode.Unauthorized, problem.Status);
        Assert.Equal("Unauthorized", problem.Title);
        Assert.Equal("Tenant-scoped requests require an authenticated user.", problem.Detail);
    }

    [Fact]
    public async Task MissingTenantHeader_ReturnsBadRequestProblemDetails()
    {
        using var client = CreateClient();
        using var request = CreateAuthenticatedRequest();

        using var response = await client.SendAsync(request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        Assert.NotNull(problem);
        Assert.Equal((int)HttpStatusCode.BadRequest, problem.Status);
        Assert.Equal("Bad Request", problem.Title);
        Assert.Equal("Missing or invalid X-Tenant-Id header.", problem.Detail);
    }

    [Fact]
    public async Task InvalidTenantHeader_ReturnsBadRequestProblemDetails()
    {
        using var client = CreateClient();
        using var request = CreateAuthenticatedRequest(requestTenantId: "not-a-guid", tokenTenantId: Guid.NewGuid().ToString());

        using var response = await client.SendAsync(request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        Assert.NotNull(problem);
        Assert.Equal((int)HttpStatusCode.BadRequest, problem.Status);
        Assert.Equal("Missing or invalid X-Tenant-Id header.", problem.Detail);
    }

    [Fact]
    public async Task InvalidAuthenticatedTenantClaim_ReturnsForbiddenProblemDetails()
    {
        using var client = CreateClient();
        using var request = CreateAuthenticatedRequest(requestTenantId: Guid.NewGuid().ToString(), tokenTenantId: "not-a-guid");

        using var response = await client.SendAsync(request);

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);

        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        Assert.NotNull(problem);
        Assert.Equal((int)HttpStatusCode.Forbidden, problem.Status);
        Assert.Equal("Forbidden", problem.Title);
        Assert.Equal("Authenticated tenant requests must include a valid tenant claim.", problem.Detail);
    }

    [Fact]
    public async Task MismatchedAuthenticatedTenantClaim_ReturnsForbiddenProblemDetails()
    {
        using var client = CreateClient();
        using var request = CreateAuthenticatedRequest(requestTenantId: Guid.NewGuid().ToString(), tokenTenantId: Guid.NewGuid().ToString());

        using var response = await client.SendAsync(request);

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);

        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        Assert.NotNull(problem);
        Assert.Equal((int)HttpStatusCode.Forbidden, problem.Status);
        Assert.Equal("Forbidden", problem.Title);
        Assert.Equal("The X-Tenant-Id header does not match the authenticated tenant claim.", problem.Detail);
    }

    [Fact]
    public async Task AuthenticatedRequestWithoutTenantClaim_ReturnsForbiddenProblemDetails()
    {
        using var client = CreateClient();
        using var request = CreateAuthenticatedRequest(requestTenantId: Guid.NewGuid().ToString());

        using var response = await client.SendAsync(request);

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);

        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        Assert.NotNull(problem);
        Assert.Equal((int)HttpStatusCode.Forbidden, problem.Status);
        Assert.Equal("Authenticated tenant requests must include a tenant claim.", problem.Detail);
    }

    [Fact]
    public async Task HostPath_BypassesTenantValidation()
    {
        using var client = CreateClient();

        using var response = await client.GetAsync("/host/tenants");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task MatchingAuthenticatedTenantClaim_AllowsPipelineToContinue()
    {
        using var client = CreateClient();
        var tenantId = Guid.NewGuid().ToString();
        using var request = CreateAuthenticatedRequest(requestTenantId: tenantId, tokenTenantId: tenantId);

        using var response = await client.SendAsync(request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    private HttpClient CreateClient()
    {
        return factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false,
            BaseAddress = new Uri("https://localhost")
        });
    }

    private static HttpRequestMessage CreateRequest(string requestTenantId = null)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "/test/tenant-probe");

        if (!string.IsNullOrWhiteSpace(requestTenantId))
            request.Headers.Add("X-Tenant-Id", requestTenantId);

        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/problem+json"));
        return request;
    }

    private static HttpRequestMessage CreateAuthenticatedRequest(string requestTenantId = null, string tokenTenantId = null)
    {
        var request = CreateRequest(requestTenantId);
        request.Headers.Add(TestAuthenticationHandler.AuthenticateHeaderName, "true");

        if (!string.IsNullOrWhiteSpace(tokenTenantId))
            request.Headers.Add(TestAuthenticationHandler.TenantIdHeaderName, tokenTenantId);

        return request;
    }
}