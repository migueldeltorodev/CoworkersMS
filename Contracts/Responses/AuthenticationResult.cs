namespace ManagementSystem.Api.Contracts.Responses;

public record AuthenticationResult
{
    public string Token { get; init; } = null!;
    public DateTime ExpiresAt { get; init; }
}