namespace TenantGuard.Domain.Entities;

/// <summary>
/// Marks an entity as belonging to a specific tenant.
/// Implementors carry a non-negotiable TenantId used by persistence
/// to enforce isolation via query filters and save guards.
/// </summary>
public interface ITenantScoped
{
    Guid TenantId { get; }
}
