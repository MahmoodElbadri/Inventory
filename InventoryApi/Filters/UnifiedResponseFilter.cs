using Inventory.Api.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.WebUtilities;

namespace Inventory.Api.Filters;

public sealed class UnifiedResponseFilter : IAlwaysRunResultFilter
{
    public void OnResultExecuting(ResultExecutingContext context)
    {
        var statusCode = context.Result switch
        {
            ObjectResult result => result.StatusCode ?? context.HttpContext.Response.StatusCode,
            StatusCodeResult result => result.StatusCode,
            _ => (int?)null
        };

        // These statuses cannot carry a response body.
        if (statusCode is null or < 200 or 204 or 205 or 304)
        {
            return;
        }

        var value = (context.Result as ObjectResult)?.Value;
        if (value is ApiResponse<object>)
        {
            return;
        }

        var success = statusCode < 400;
        var problem = value as ProblemDetails;
        var message = success
            ? ReasonPhrases.GetReasonPhrase(statusCode.Value)
            : problem?.Detail ?? problem?.Title ?? value as string
                ?? ReasonPhrases.GetReasonPhrase(statusCode.Value);
        object? errors = problem is ValidationProblemDetails validation
            ? validation.Errors
            : success ? null : value;

        var response = new ApiResponse<object>(success, statusCode.Value, message,
            success ? value : null, errors, context.HttpContext.TraceIdentifier);

        if (context.Result is ObjectResult objectResult)
        {
            // Retain CreatedAtActionResult so MVC still generates the Location header.
            objectResult.Value = response;
            objectResult.DeclaredType = typeof(ApiResponse<object>);
            objectResult.ContentTypes.Clear();
            objectResult.ContentTypes.Add("application/json");
        }
        else
        {
            context.Result = new ObjectResult(response) { StatusCode = statusCode };
        }
    }

    public void OnResultExecuted(ResultExecutedContext context) { }
}
