namespace ManagementSystem.Api.Contracts.Responses;

public record RoomResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; } = null!;
    public string Location { get; init; } = null!;
    public int Capacity { get; init; }
    public string? Description { get; init; }
    public bool IsActive { get; init; }
    public decimal HourlyRate { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
}