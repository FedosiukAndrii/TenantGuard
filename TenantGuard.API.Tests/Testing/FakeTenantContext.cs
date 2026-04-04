#nullable enable
using TenantGuard.Application.Context;

namespace TenantGuard.API.Tests.Testing;

/// <summary>
/// In-memory ITenantContext for tests that need to control tenant identity
/// without going through the full HTTP middleware pipeline.
/// </summary>
public sealed class FakeTenantContext : ITenantContext
{
    public Guid? TenantId { get; private set; }
    public string? ExternalUserId { get; private set; }

    public void SetTenant(Guid tenantId) => TenantId = tenantId;
    public void SetUser(string externalUserId) => ExternalUserId = externalUserId;
}
