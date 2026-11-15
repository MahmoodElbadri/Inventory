namespace Inventory.Api.Responses;

public sealed record ApiResponse<T>(
    bool Success,
    int StatusCode,
    string Message,
    T? Data,
    object? Errors,
    string TraceId)
{
    public DateTimeOffset Timestamp { get; } = DateTimeOffset.UtcNow;
}
