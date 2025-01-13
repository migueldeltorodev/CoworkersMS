using AutoMapper;
using ManagementSystem.Api.Common.Interfaces;
using ManagementSystem.Api.Common.Results;
using ManagementSystem.Api.Contracts.Responses;
using MediatR;

namespace ManagementSystem.Api.Features.Rooms.Queries.GetRooms;

public class GetRoomsQueryHandler : IRequestHandler<GetRoomsQuery, Result<IReadOnlyList<RoomResponse>>>
{
    private readonly IMapper _mapper;
    private readonly IRoomRepository _roomRepository;

    public GetRoomsQueryHandler(IRoomRepository roomRepository, IMapper mapper)
    {
        _roomRepository = roomRepository;
        _mapper = mapper;
    }

    public async Task<Result<IReadOnlyList<RoomResponse>>> Handle(
        GetRoomsQuery query,
        CancellationToken cancellationToken)
    {
        var rooms = await _roomRepository.GetRoomsAsync(
            query.SearchTerm,
            query.Location,
            query.MinCapacity,
            query.MaxHourlyRate,
            query.IsActive,
            cancellationToken);

        return Result<IReadOnlyList<RoomResponse>>.Success(
            rooms.Select(r => _mapper.Map<RoomResponse>(r)).ToList());
    }
}