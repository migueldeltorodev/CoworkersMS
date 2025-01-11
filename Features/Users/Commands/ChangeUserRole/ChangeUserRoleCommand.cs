using ManagementSystem.Api.Common.Results;
using ManagementSystem.Api.Domain.Enums;
using MediatR;

namespace ManagementSystem.Api.Features.Users.Commands.ChangeUserRole;

public record ChangeUserRoleCommand : IRequest<Result<Unit>>
{
    public Guid UserId { get; init; }
    public UserRole NewRole { get; init; }
    public Guid RequestedByUserId { get; init; }
}