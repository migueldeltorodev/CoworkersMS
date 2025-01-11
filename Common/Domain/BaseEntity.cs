namespace ManagementSystem.Api.Common.Domain;

public abstract class BaseEntity
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string CreatedBy { get; set; } = default!;
    public string? UpdatedBy { get; set; }
}