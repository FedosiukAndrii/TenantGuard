using TenantGuard.Domain.Enums;

namespace TenantGuard.Domain.Entities;

public sealed class Tenant(
    Guid id,
    string slug,
    string name,
    TenantStatus status,
    DateTimeOffset createdAtUtc,
    string createdBy)
{
    public Guid Id { get; private set; } = id;
    public string Slug { get; private set; } = slug;
    public string Name { get; private set; } = name;
    public TenantStatus Status { get; private set; } = status;
    public DateTimeOffset CreatedAtUtc { get; private set; } = createdAtUtc;
    public string CreatedBy { get; private set; } = createdBy;
}
