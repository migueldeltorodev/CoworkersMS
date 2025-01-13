using System.Security.Claims;
using ManagementSystem.Api.Common.Exceptions;
using ManagementSystem.Api.Common.Interfaces;
using ManagementSystem.Api.Contracts.Requests.Users;
using ManagementSystem.Api.Contracts.Responses;
using ManagementSystem.Api.Mappings.User;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ManagementSystem.Api.Endpoints.User;

public static class UserEndpoints
{
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/users")
            .WithTags("Users")
            .WithOpenApi();

        // Public endpoints (no authentication required)
        group.MapPost("/register", async Task<Results<Ok<UserResponse>, BadRequest<string>>> (
            [FromBody] RegisterUserRequest request,
            ISender mediator,
            IUserRepository userRepository,
            CancellationToken cancellationToken) =>
        {
            var command = request.ToCommand();
            var result = await mediator.Send(command, cancellationToken);

            if (!result.IsSuccess)
                return TypedResults.BadRequest(result.Error);

            var user = await userRepository.GetByIdAsync(result.Value, cancellationToken);
            return TypedResults.Ok(user!.ToResponse());
        })
        .WithName("RegisterUser")
        .WithDescription("Register a new user")
        .AllowAnonymous();

        group.MapPost("/login", async Task<Results<Ok<AuthenticationResponse>, BadRequest<string>>> (
            [FromBody] LoginUserRequest request,
            ISender mediator,
            IUserRepository userRepository,
            CancellationToken cancellationToken) =>
        {
            var command = request.ToCommand();
            var result = await mediator.Send(command, cancellationToken);

            if (!result.IsSuccess)
                return TypedResults.BadRequest(result.Error);

            var user = await userRepository.GetByEmailAsync(request.Email, cancellationToken);
            return TypedResults.Ok(result.Value?.ToResponse(user!));
        })
        .WithName("LoginUser")
        .WithDescription("Authenticate a user")
        .AllowAnonymous();

        // Protected endpoints (require authentication)
        group.MapGet("/me", async Task<Results<Ok<UserResponse>, NotFound>> (
            HttpContext context,
            IUserRepository userRepository,
            CancellationToken cancellationToken) =>
        {
            var userId = GetUserIdFromContext(context);
            var user = await userRepository.GetByIdAsync(userId, cancellationToken);

            return user is null
                ? TypedResults.NotFound()
                : TypedResults.Ok(user.ToResponse());
        })
        .WithName("GetCurrentUser")
        .WithDescription("Get current user information")
        .RequireAuthorization();

        // Admin endpoints
        group.MapPut("/{id:guid}/role", async Task<Results<Ok<UserResponse>, NotFound, BadRequest<string>>> (
            Guid id,
            [FromBody] ChangeUserRoleRequest request,
            HttpContext context,
            ISender mediator,
            IUserRepository userRepository,
            CancellationToken cancellationToken) =>
        {
            try
            {
                var requestedByUserId = GetUserIdFromContext(context);
                var command = request.ToCommand(id, requestedByUserId);
                var result = await mediator.Send(command, cancellationToken);

                if (!result.IsSuccess)
                    return TypedResults.BadRequest(result.Error);

                var updatedUser = await userRepository.GetByIdAsync(id, cancellationToken);
                return TypedResults.Ok(updatedUser!.ToResponse());
            }
            catch (NotFoundException)
            {
                return TypedResults.NotFound();
            }
        })
        .WithName("ChangeUserRole")
        .WithDescription("Change a user's role (Admin only)")
        .RequireAuthorization(policy => policy.RequireRole("Administrator"));

        return app;
    }

    private static Guid GetUserIdFromContext(HttpContext context)
    {
        var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? throw new UnauthorizedAccessException("User ID not found in token");
        
        return Guid.Parse(userIdClaim);
    }
}