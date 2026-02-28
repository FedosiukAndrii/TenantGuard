namespace TenantGuard.Application.Context;

public interface ITenantContext
{
    Guid? TenantId { get; }
    string? ExternalUserId { get; }

    void SetTenant(Guid tenantId);
    void SetUser(string externalUserId);
}
