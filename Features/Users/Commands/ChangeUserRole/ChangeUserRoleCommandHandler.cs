using ManagementSystem.Api.Common.Exceptions;
using ManagementSystem.Api.Common.Interfaces;
using ManagementSystem.Api.Common.Results;
using ManagementSystem.Api.Domain.Entities;
using ManagementSystem.Api.Domain.Enums;
using MediatR;

namespace ManagementSystem.Api.Features.Users.Commands.ChangeUserRole;

public class ChangeUserRoleCommandHandler : IRequestHandler<ChangeUserRoleCommand, Result<Unit>>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ChangeUserRoleCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Unit>> Handle(ChangeUserRoleCommand request, CancellationToken cancellationToken)
    {
        var requestingUser = await _userRepository.GetByIdAsync(request.RequestedByUserId, cancellationToken)
                             ?? throw new NotFoundException(nameof(User), request.RequestedByUserId);

        if (requestingUser.Role != UserRole.Administrator)
        {
            return Result<Unit>.Failure("Only administrators can change user roles");
        }

        var userToUpdate = await _userRepository.GetByIdAsync(request.UserId, cancellationToken)
                           ?? throw new NotFoundException(nameof(User), request.UserId);

        userToUpdate.SetRole(request.NewRole);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Unit>.Success(Unit.Value);
    }
}