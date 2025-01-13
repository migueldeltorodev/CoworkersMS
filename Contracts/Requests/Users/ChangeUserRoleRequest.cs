using ManagementSystem.Api.Domain.Enums;

namespace ManagementSystem.Api.Contracts.Requests.Users;

public record ChangeUserRoleRequest
{
    public UserRole NewRole { get; init; }
}