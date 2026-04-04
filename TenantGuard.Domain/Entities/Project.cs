namespace TenantGuard.Domain.Entities;

/// <summary>
/// Represents a project owned by a tenant.
/// Full CRUD API is delivered in a later story; this entity exists
/// to anchor tenant-scoped persistence patterns (query filters, save guards).
/// </summary>
public sealed class Project(Guid id, Guid tenantId, string name) : ITenantScoped
{
    public Guid Id { get; private set; } = id;
    public Guid TenantId { get; private set; } = tenantId;
    public string Name { get; private set; } = name;
}
