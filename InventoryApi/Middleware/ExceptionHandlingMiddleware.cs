using Inventory.Application.Exceptions;
using Inventory.Api.Responses;
using Microsoft.AspNetCore.Http.HttpResults;

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
            var isBadRequest = exception is BadRequestException;
            var isServerError = !isNotFound && !isConflict && !isBadRequest;
            // var statusCode = isNotFound
            //     ? StatusCodes.Status404NotFound
            //     ? StatusCodes.Status400BadRequest
            //     : StatusCodes.Status409Conflict;

            int statusCode;
            string messageError = "";
            if (isNotFound)
                statusCode = StatusCodes.Status404NotFound;
            else if (isConflict)
                statusCode = StatusCodes.Status409Conflict;
            else if (isBadRequest)
                statusCode = StatusCodes.Status400BadRequest;
            else
                statusCode = StatusCodes.Status500InternalServerError;
            

            if (isNotFound)
            {
                messageError = $"{exception.Message}";
                    logger.LogWarning(exception, $"Resource not found. Trace ID: {exception.StackTrace}",
                    context.TraceIdentifier);
            }
            else if (isConflict)
            {
                messageError = $"Duplicate product name. Trace ID: {exception.StackTrace}";
                logger.LogWarning(exception, $"Duplicate product name. Trace ID: {exception.StackTrace}",
                    context.TraceIdentifier);
            }
            else if (isBadRequest)
            {
                messageError = $" {exception.Message}";
                logger.LogWarning(exception, $"Message: {exception.Message}",
                    context.TraceIdentifier, exception.Message);
            }
            else if(isServerError)
            {
                messageError =  $"An unhandled exception occurred. Trace ID: {exception.Message}";
                logger.LogError(exception, $"An unhandled exception occurred. Trace ID: {exception.StackTrace}",
                    context.TraceIdentifier);
            }

            var response = new ApiResponse<object>(
                false,
                statusCode,
                messageError,
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
