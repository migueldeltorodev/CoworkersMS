namespace ManagementSystem.Api.Contracts.Requests.Rooms;

public record GetRoomsRequest
{
    public string? SearchTerm { get; init; }
    public string? Location { get; init; }
    public int? MinCapacity { get; init; }
    public decimal? MaxHourlyRate { get; init; }
    public bool? IsActive { get; init; }
}