namespace ManagementSystem.Api.Contracts.Responses;

public record AuthenticationResponse
{
    public string Token { get; init; } = null!;
    public DateTime ExpiresAt { get; init; }
    public string Email { get; init; } = null!;
    public string FirstName { get; init; } = null!;
    public string LastName { get; init; } = null!;
    public string Role { get; init; } = null!;
}