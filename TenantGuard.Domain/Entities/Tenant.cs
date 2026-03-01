using TenantGuard.Domain.Enums;

namespace TenantGuard.Domain.Entities;

public sealed class Tenant
{
    private Tenant()
    {
    }

    public Tenant(Guid id, string slug, string name, TenantStatus status, DateTimeOffset createdAtUtc, string createdBy)
    {
        Id = id;
        Slug = slug;
        Name = name;
        Status = status;
        CreatedAtUtc = createdAtUtc;
        CreatedBy = createdBy;
    }

    public Guid Id { get; private set; }
    public string Slug { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public TenantStatus Status { get; private set; }
    public DateTimeOffset CreatedAtUtc { get; private set; }
    public string CreatedBy { get; private set; } = string.Empty;
}
