using ManagementSystem.Api.Common.Interfaces;
using ManagementSystem.Api.Common.Results;
using ManagementSystem.Api.Domain.Entities;
using MediatR;

namespace ManagementSystem.Api.Features.Rooms.Commands.CreateRoom;

public class CreateRoomCommandHandler : IRequestHandler<CreateRoomCommand, Result<Guid>>
{
    private readonly IRoomRepository _roomRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateRoomCommandHandler(
        IRoomRepository roomRepository,
        IUnitOfWork unitOfWork)
    {
        _roomRepository = roomRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(
        CreateRoomCommand command,
        CancellationToken cancellationToken)
    {
        // Validate if the room already exists?
        if (await _roomRepository.ExistsByNameAsync(command.Name, cancellationToken))
            return Result<Guid>.Failure("A room with this name already exists");

        var room = new Room(
            command.Name,
            command.Location,
            command.Capacity,
            command.HourlyRate,
            command.Description);

        await _roomRepository.AddAsync(room, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(room.Id);
    }
}