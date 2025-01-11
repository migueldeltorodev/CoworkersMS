using ManagementSystem.Api.Domain.Enums;

namespace ManagementSystem.Api.Contracts.DTOs;

public record BookingDto
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public Guid RoomId { get; init; }
    public string RoomName { get; init; } = null!;
    public DateTime StartTime { get; init; }
    public DateTime EndTime { get; init; }
    public BookingStatus Status { get; init; }
    public decimal TotalAmount { get; init; }
}