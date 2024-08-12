using FluentValidation;
using Ordering.Application.Exceptions;
using System.Net;
using System.Text.Json;
namespace Ordering.Api.Midddleware;

public class GlobalExceptionHandling
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandling> _logger;

    public GlobalExceptionHandling(RequestDelegate next,
                            ILogger<GlobalExceptionHandling> logger)
    {
        _next = next;
        _logger = logger;
    }


    public async Task InvokeAsync(HttpContext context)
    {

        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred.");
            await HandleExceptionAsync(context, ex);
        }



    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {


        HttpStatusCode statusCode;
        string message = string.Empty;
        string stackTrace = string.Empty;
        var exceptionType = ex.GetType();
        if (exceptionType == typeof(OrderNotFoundException))
        {
            message = ex.Message;
            statusCode = HttpStatusCode.NotFound;
            stackTrace = ex.StackTrace;
        }
        else if (exceptionType == typeof(ValidationException))
        {
            message = ex.Message;
            statusCode = HttpStatusCode.BadRequest;
            stackTrace = ex.StackTrace;
        }
        else
        {
            message = "Please contact with application support ";
            statusCode = HttpStatusCode.InternalServerError;
            stackTrace = ex.StackTrace;
        }

        var response = JsonSerializer.Serialize(new { error = message, statusCode = statusCode, stackTrace = stackTrace });

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        context.Response.WriteAsync(response);


    }
}
