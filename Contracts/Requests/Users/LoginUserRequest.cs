namespace ManagementSystem.Api.Contracts.Requests.Users;

public record LoginUserRequest
{
    public string Email { get; init; } = null!;
    public string Password { get; init; } = null!;
}