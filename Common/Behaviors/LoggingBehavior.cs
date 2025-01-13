using System.Text.Json;
using MediatR;

namespace ManagementSystem.Api.Common.Behaviors;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var correlationId = Guid.NewGuid();

        // Request Logging
        // Serialize the request
        var requestJson = JsonSerializer.Serialize(request);
        // Log the serialized request
        _logger.LogInformation("Handling request {CorrelationID}: {Request}", correlationId, requestJson);

        // Response logging
        var response = await next();
        // Serialize the request
        var responseJson = JsonSerializer.Serialize(response);
        // Log the serialized request
        _logger.LogInformation("Response for {Correlation}: {Response}", correlationId, responseJson);

        // Return response
        return response;
    }
}