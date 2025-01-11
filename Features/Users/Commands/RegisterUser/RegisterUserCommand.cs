using ManagementSystem.Api.Common.Results;
using MediatR;

namespace ManagementSystem.Api.Features.Users.Commands.RegisterUser;

public record RegisterUserCommand : IRequest<Result<Guid>>
{
    public string Email { get; init; } = null!;
    public string Password { get; init; } = null!;
    public string FirstName { get; init; } = null!;
    public string LastName { get; init; } = null!;
}