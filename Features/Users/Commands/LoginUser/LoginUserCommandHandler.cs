using ManagementSystem.Api.Common.Interfaces;
using ManagementSystem.Api.Common.Results;
using ManagementSystem.Api.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ManagementSystem.Api.Features.Users.Commands.LoginUser;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Result<AuthenticationResult>>
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IUserRepository _userRepository;

    public LoginUserCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher<User> passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<Result<AuthenticationResult>> Handle(
        LoginUserCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);

        if (user is null) return Result<AuthenticationResult>.Failure("Invalid credentials");

        if (_passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password)
            is PasswordVerificationResult.Failed)
            return Result<AuthenticationResult>.Failure("Invalid credentials");

        var token = _jwtTokenGenerator.GenerateToken(user);
        var expiresAt = DateTime.UtcNow.AddHours(1);

        return Result<AuthenticationResult>.Success(new AuthenticationResult
        {
            Token = token,
            ExpiresAt = expiresAt
        });
    }
}