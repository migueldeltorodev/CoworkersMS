namespace ManagementSystem.Api.Contracts.Requests.Rooms;

public record CreateRoomRequest
{
    public string Name { get; init; } = null!;
    public string Location { get; init; } = null!;
    public int Capacity { get; init; }
    public decimal HourlyRate { get; init; }
    public string? Description { get; init; }
}