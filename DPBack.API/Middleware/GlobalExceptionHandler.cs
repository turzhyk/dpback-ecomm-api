using DPBack.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace DPBack.API.Middleware;

public sealed class GlobalExceptionHandler(RequestDelegate next, ILogger<GlobalExceptionHandler> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (OperationCanceledException)
        {
            logger.LogInformation("Request was canceled by client");
            context.Response.StatusCode = 499;
        }
        catch (Exception error)
        {
            logger.LogWarning(error, "Unhandled exception");
            await HandleExceptionAsync(context, error);

        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception error)
    {
        var (statusCode, title) = error switch
        {
            ArgumentException => (StatusCodes.Status400BadRequest, "Bad request"),
            KeyNotFoundException  => (StatusCodes.Status404NotFound, "Not found"),
            UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, "Unauthorized"), 
            StatusChangeNotAllowedException => (StatusCodes.Status409Conflict, "Status change not allowed"),
            CustomerDoesNotExistException => (StatusCodes.Status404NotFound, "Customer does not exist"),
            InvalidJsonValuesException => (StatusCodes.Status400BadRequest, "Invalid Json Value"),
            OrderDoesNotExistException => (StatusCodes.Status404NotFound, "Order does not exist"),
            _ => (StatusCodes.Status500InternalServerError, "Internal server error")
        };
        context.Response.StatusCode = statusCode;
        var problemDetails = new ProblemDetails
        {
            Title = title,
            Status = statusCode,
            Type = error.GetType().Name,
            Detail = statusCode == 500 ? null : error.Message
        };
        problemDetails.Extensions["traceId"] = context.TraceIdentifier;
        return context.Response.WriteAsJsonAsync(problemDetails);
    }
}

