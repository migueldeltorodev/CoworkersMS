using ManagementSystem.Api.Domain.Entities;

namespace ManagementSystem.Api.Common.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}