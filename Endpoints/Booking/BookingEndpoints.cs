using Delta;
using ManagementSystem.Api.Common.Exceptions;
using ManagementSystem.Api.Contracts.DTOs;
using ManagementSystem.Api.Contracts.Requests.Bookings;
using ManagementSystem.Api.Features.Bookings.Queries.GetUserBookings;
using ManagementSystem.Api.Mappings.Booking;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ManagementSystem.Api.Endpoints.Booking;

public static class BookingEndpoints
{
    public static IEndpointRouteBuilder MapBookingEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/bookings")
            .WithTags("Bookings")
            .WithOpenApi()
            .UseDelta();

        group.MapPost("/", async Task<Results<Created<Guid>, BadRequest<string>>> (
                [FromBody] CreateBookingRequest request,
                HttpContext context,
                ISender mediator,
                CancellationToken cancellationToken) =>
            {
                try
                {
                    var userId = context.GetUserIdFromContext();
                    var command = request.ToCommand(userId);

                    var result = await mediator.Send(command, cancellationToken);

                    return result.IsSuccess
                        ? TypedResults.Created($"/api/bookings/{result.Value}", result.Value)
                        : TypedResults.BadRequest(result.Error);
                }
                catch (NotFoundException ex)
                {
                    return TypedResults.BadRequest(ex.Message);
                }
            })
            .WithName("CreateBooking")
            .WithDescription("Create a new booking for a room")
            .RequireAuthorization();

        group.MapPost("/{id:guid}/cancel", async Task<Results<NoContent, NotFound, BadRequest<string>>> (
                Guid id,
                [FromBody] CancelBookingRequest request,
                HttpContext context,
                ISender mediator,
                CancellationToken cancellationToken) =>
            {
                try
                {
                    var userId = context.GetUserIdFromContext();
                    var command = request.ToCommand(id, userId);
                    var result = await mediator.Send(command, cancellationToken);

                    if (result.IsSuccess)
                        return TypedResults.NoContent();

                    return TypedResults.BadRequest(result.Error);
                }
                catch (NotFoundException)
                {
                    return TypedResults.NotFound();
                }
            })
            .WithName("CancelBooking")
            .WithDescription("Cancel an existing booking")
            .RequireAuthorization();

        group.MapGet("/me", async Task<Results<Ok<List<BookingDto>>, NotFound>> (
                HttpContext context,
                [FromQuery] DateTime? fromDate,
                ISender mediator,
                CancellationToken cancellationToken) =>
            {
                var userId = context.GetUserIdFromContext();
                var query = new GetUserBookingsQuery { UserId = userId, FromDate = fromDate };
                var result = await mediator.Send(query, cancellationToken);

                return result.IsSuccess
                    ? TypedResults.Ok(result.Value)
                    : TypedResults.NotFound();
            })
            .WithName("GetMyBookings")
            .WithDescription("Get all bookings for the current user")
            .RequireAuthorization();

        return app;
    }
}