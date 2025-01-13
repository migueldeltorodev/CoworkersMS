namespace ManagementSystem.Api.Contracts.Responses;

public record BookingResponse
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public Guid RoomId { get; init; }
    public string RoomName { get; init; } = null!;
    public DateTime StartTime { get; init; }
    public DateTime EndTime { get; init; }
    public string Status { get; init; } = null!;
    public decimal TotalAmount { get; init; }
}