using ManagementSystem.Api.Common.Domain;

namespace ManagementSystem.Api.Domain.Entities;

public class AuditLog : BaseEntity
{
    private AuditLog()
    {
    }

    public AuditLog(
        string entityType,
        Guid entityId,
        string action,
        string changes,
        string userId)
    {
        EntityType = entityType;
        EntityId = entityId;
        Action = action;
        Changes = changes;
        CreatedBy = userId;
        CreatedAt = DateTime.UtcNow;
    }

    public string EntityType { get; private set; } = null!;
    public Guid EntityId { get; private set; }
    public string Action { get; private set; } = null!;
    public string Changes { get; private set; } = null!;
}