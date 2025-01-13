using System.Net;
using FluentValidation;
using ManagementSystem.Api.Exceptions.Booking;
using ManagementSystem.Api.Exceptions.Room;
using ManagementSystem.Api.Exceptions.Users;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ManagementSystem.Api.Common.Exceptions;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;
    private readonly IHostEnvironment _environment;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IHostEnvironment environment)
    {
        _logger = logger;
        _environment = environment;
    }
    
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var problemDetails = new ProblemDetails
        {
            Instance = httpContext.Request.Path,
            Status = (int)HttpStatusCode.InternalServerError,
            Title = "An unexpected error occurred.",
            Detail = exception.Message
        };

        switch (exception)
        {
            case UserAlreadyExistsException ex:
                _logger.LogWarning(ex, "Attempt to create duplicate user");
                problemDetails.Title = ex.Message;
                problemDetails.Status = (int)HttpStatusCode.Conflict;
                break;

            case InvalidUserDataException ex:
                _logger.LogWarning(ex, "Invalid user data");
                problemDetails.Title = ex.Message;
                problemDetails.Status = (int)HttpStatusCode.BadRequest;
                break;

            case RoomAlreadyBookedException ex:
                _logger.LogWarning(ex, "Room already booked");
                problemDetails.Title = ex.Message;
                problemDetails.Status = (int)HttpStatusCode.Conflict;
                break;

            case InvalidBookingTimeException ex:
                _logger.LogWarning(ex, "Invalid booking time");
                problemDetails.Title = ex.Message;
                problemDetails.Status = (int)HttpStatusCode.BadRequest;
                break;

            case NotFoundException ex:
                _logger.LogWarning(ex, "Resource not found");
                problemDetails.Title = ex.Message;
                problemDetails.Status = (int)HttpStatusCode.NotFound;
                break;
            
            case ValidationException ex:
                _logger.LogWarning(ex, "Validation failed");
                problemDetails.Title = "Validation failed";
                problemDetails.Status = (int)HttpStatusCode.BadRequest;
                problemDetails.Extensions["errors"] = ex.Errors;
                break;

            default:
                _logger.LogError(exception, "Unhandled exception");
                problemDetails.Title = "An unexpected error occurred.";
                problemDetails.Status = (int)HttpStatusCode.InternalServerError;
                break;
        }

        // Añadir información adicional en desarrollo
        if (_environment.IsDevelopment())
        {
            problemDetails.Detail = exception.StackTrace;
        }

        httpContext.Response.ContentType = "application/problem+json";
        httpContext.Response.StatusCode = problemDetails.Status ?? (int)HttpStatusCode.InternalServerError;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}