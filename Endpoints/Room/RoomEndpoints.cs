using ManagementSystem.Api.Contracts.Requests.Rooms;
using ManagementSystem.Api.Contracts.Responses;
using ManagementSystem.Api.Mappings.Room;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ManagementSystem.Api.Endpoints.Room;

public static class RoomEndpoints
{
    public static void MapRoomEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/rooms")
            .WithTags("Rooms");

        group.MapPost("/", async Task<Results<Created<Guid>, BadRequest<string>>> (
                [FromBody] CreateRoomRequest request,
                ISender mediator,
                CancellationToken cancellationToken) =>
            {
                var command = request.ToCommand();
                var result = await mediator.Send(command, cancellationToken);

                if (!result.IsSuccess)
                    return TypedResults.BadRequest(result.Error);

                return TypedResults.Created($"/api/rooms/{result.Value}", result.Value);
            })
            .WithName("CreateRoom")
            .WithDescription("Create a new room")
            .RequireAuthorization(policy => policy.RequireRole("Administrator"));
        
        group.MapGet("/", async Task<Results<Ok<IReadOnlyList<RoomResponse>>, BadRequest<string>>> (
                [AsParameters] GetRoomsRequest request,
                ISender mediator,
                CancellationToken cancellationToken) =>
            {
                var query = request.ToQuery();
                var result = await mediator.Send(query, cancellationToken);

                if (!result.IsSuccess)
                    return TypedResults.BadRequest(result.Error);

                return TypedResults.Ok(result.Value);
            })
            .WithName("GetRooms")
            .WithDescription("Get all rooms with optional filtering")
            .AllowAnonymous();
    }
}