using ManagementSystem.Api.Common.Results;
using MediatR;
using Microsoft.Identity.Client;

namespace ManagementSystem.Api.Features.Users.Commands.LoginUser;

public record LoginUserCommand : IRequest<Result<AuthenticationResult>>, IRequest<Result<Contracts.Responses.AuthenticationResult>>
{
    public string Email { get; init; } = null!;
    public string Password { get; init; } = null!;
}