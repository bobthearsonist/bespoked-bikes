using System.Net;
using System.Text.Json;
using BespokedBikes.Application.Generated;
using FluentValidation;

namespace BespokedBikes.Api.Middleware;

/// <summary>
/// Global exception handling middleware that converts exceptions to proper HTTP responses
/// </summary>
public class GlobalExceptionHandlerMiddleware(
    RequestDelegate next,
    ILogger<GlobalExceptionHandlerMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = context.Response;
        response.ContentType = "application/json";

        ErrorResponse errorResponse;

        switch (exception)
        {
            case KeyNotFoundException keyNotFoundException:
                response.StatusCode = (int)HttpStatusCode.NotFound;
                errorResponse = new ErrorResponse
                {
                    StatusCode = response.StatusCode,
                    Message = keyNotFoundException.Message,
                    Timestamp = DateTimeOffset.UtcNow,
                    Path = context.Request.Path
                };
                break;

            case ValidationException validationException:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse = new ErrorResponse
                {
                    StatusCode = response.StatusCode,
                    Message = "Validation failed",
                    Errors = validationException.Errors
                        .Select(e => $"{e.PropertyName}: {e.ErrorMessage}")
                        .ToList(),
                    Timestamp = DateTimeOffset.UtcNow,
                    Path = context.Request.Path
                };
                break;

            case ArgumentException argumentException:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse = new ErrorResponse
                {
                    StatusCode = response.StatusCode,
                    Message = argumentException.Message,
                    Timestamp = DateTimeOffset.UtcNow,
                    Path = context.Request.Path
                };
                break;

            case NotImplementedException notImplementedException:
                response.StatusCode = (int)HttpStatusCode.NotImplemented;
                errorResponse = new ErrorResponse
                {
                    StatusCode = response.StatusCode,
                    Message = notImplementedException.Message,
                    Timestamp = DateTimeOffset.UtcNow,
                    Path = context.Request.Path
                };
                break;

            default:
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                errorResponse = new ErrorResponse
                {
                    StatusCode = response.StatusCode,
                    Message = "An internal server error occurred",
                    Timestamp = DateTimeOffset.UtcNow,
                    Path = context.Request.Path
                };
                break;
        }

        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        await response.WriteAsync(JsonSerializer.Serialize(errorResponse, jsonOptions));
    }
}
