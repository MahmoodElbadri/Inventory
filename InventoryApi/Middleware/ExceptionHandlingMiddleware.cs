using Inventory.Application.Exceptions;
using Inventory.Api.Responses;

namespace Inventory.Api.Middleware;

public sealed class ExceptionHandlingMiddleware(
    RequestDelegate next,
    ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (OperationCanceledException) when (context.RequestAborted.IsCancellationRequested)
        {
            // The client has disconnected; there is no response left to write.
            throw;
        }
        catch (Exception exception)
        {
            if (context.Response.HasStarted)
            {
                logger.LogError(exception,
                    "An exception occurred after the response started. Trace ID: {TraceId}",
                    context.TraceIdentifier);
                throw;
            }

            var isNotFound = exception is NotFoundException;
            var isConflict = exception is DuplicateProductNameException;
            var statusCode = isNotFound
                ? StatusCodes.Status404NotFound
                : StatusCodes.Status409Conflict;

            if (isNotFound)
            {
                logger.LogWarning(exception, "Resource not found. Trace ID: {TraceId}",
                    context.TraceIdentifier);
            }
            else if (isConflict)
            {
                logger.LogWarning(exception, "Duplicate product name. Trace ID: {TraceId}",
                    context.TraceIdentifier);
            }
            else
            {
                logger.LogError(exception, "An unhandled exception occurred. Trace ID: {TraceId}",
                    context.TraceIdentifier);
            }

            var response = new ApiResponse<object>(
                false,
                statusCode,
                isNotFound ? exception.Message : isConflict ? exception.Message : "An unexpected error occurred. Please try again later.",
                null,
                null,
                context.TraceIdentifier);

            context.Response.Clear();
            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsJsonAsync(response,
                cancellationToken: context.RequestAborted);
        }
    }
}
