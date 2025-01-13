namespace ManagementSystem.Api.Features.Users.Commands.LoginUser;

public record AuthenticationResult
{
    public string Token { get; init; } = null!;
    public DateTime ExpiresAt { get; init; }
}