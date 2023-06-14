using IntegrationLogger.Commands.Interfaces;

namespace IntegrationLogger.Models;
public class ApiResponse<T> : ICommandResult
{
    public ApiResponse(bool success, string? message, T? data, object? error)
    {
        Success = success;
        Message = message;
        Data = data;
        Error = error;
    }

    public bool Success { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }
    public object? Error { get; set; }
}