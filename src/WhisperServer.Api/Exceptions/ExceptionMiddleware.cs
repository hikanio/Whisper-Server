using WhisperServer.Api.Exceptions.Abstractions.Http;

namespace WhisperServer.Api.Exceptions;

/// <summary>
/// Middleware for handling exceptions in the HTTP request pipeline.
/// Catches exceptions thrown during the processing of a request and returns an appropriate error response.
/// </summary>
public class ExceptionMiddleware : IMiddleware
{
    public record Error(string Code, string Message);

    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, exception.Message);
            await HandleExceptionAsync(exception, context);
        }
    }

    private static async Task HandleExceptionAsync(Exception exception, HttpContext context)
    {
        var (statusCode, error) = exception switch
        {
            Status409Exception => (StatusCodes.Status409Conflict,
                new Error(exception.GetType().Name.Replace("Exception", string.Empty), exception.Message)),
            Status404Exception => (StatusCodes.Status404NotFound,
                new Error(exception.GetType().Name.Replace("Exception", string.Empty), exception.Message)),
            Status401Exception => (StatusCodes.Status401Unauthorized,
                new Error(exception.GetType().Name.Replace("Exception", string.Empty), exception.Message)),
            CustomException => (StatusCodes.Status400BadRequest,
                new Error(exception.GetType().Name.Replace("Exception", string.Empty), exception.Message)),
            _ => (StatusCodes.Status500InternalServerError, new Error("Error", "Server error"))
        };

        context.Response.StatusCode = statusCode;
        await context.Response.WriteAsJsonAsync(error);
    }
}