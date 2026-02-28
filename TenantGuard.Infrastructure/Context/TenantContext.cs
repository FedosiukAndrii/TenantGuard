using TenantGuard.Application.Context;

namespace TenantGuard.Infrastructure.Context;

public sealed class TenantContext : ITenantContext
{
    public Guid? TenantId { get; private set; }
    public string? ExternalUserId { get; private set; }

    public void SetTenant(Guid tenantId) => TenantId = tenantId;

    public void SetUser(string externalUserId) => ExternalUserId = externalUserId;
}