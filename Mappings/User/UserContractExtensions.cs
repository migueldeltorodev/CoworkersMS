using ManagementSystem.Api.Contracts.Requests.Users;
using ManagementSystem.Api.Contracts.Responses;
using ManagementSystem.Api.Features.Users.Commands.ChangeUserRole;
using ManagementSystem.Api.Features.Users.Commands.LoginUser;
using ManagementSystem.Api.Features.Users.Commands.RegisterUser;

namespace ManagementSystem.Api.Mappings.User;

public static class UserContractExtensions
{
    public static RegisterUserCommand ToCommand(this RegisterUserRequest request)
        => new()
        {
            Email = request.Email,
            Password = request.Password,
            FirstName = request.FirstName,
            LastName = request.LastName
        };

    public static LoginUserCommand ToCommand(this LoginUserRequest request)
        => new()
        {
            Email = request.Email,
            Password = request.Password
        };

    public static ChangeUserRoleCommand ToCommand(this ChangeUserRoleRequest request, Guid userId, Guid requestedByUserId)
        => new()
        {
            UserId = userId,
            NewRole = request.NewRole,
            RequestedByUserId = requestedByUserId
        };

    public static AuthenticationResponse ToResponse(this AuthenticationResult result, Domain.Entities.User user)
        => new()
        {
            Token = result.Token,
            ExpiresAt = result.ExpiresAt,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Role = user.Role.ToString()
        };

    public static UserResponse ToResponse(this Domain.Entities.User user)
        => new()
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Role = user.Role.ToString(),
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };
}