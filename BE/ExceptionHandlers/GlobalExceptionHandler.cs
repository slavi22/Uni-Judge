using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace BE.ExceptionHandlers;

// disable exception handler middleware in dev mode => https://codewithmukesh.com/blog/global-exception-handling-in-aspnet-core/#iexceptionhandler-in-net-8-and-above-recommended:~:text=Silencing%20Microsoft.AspNetCore.Diagnostics.ExceptionHandlerMiddleware%20Logs
// https://www.milanjovanovic.tech/blog/global-error-handling-in-aspnetcore-8#:~:text=Here%27s%20a%20GlobalExceptionHandler%20implementation%3A
public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(
            exception, "Exception occurred: {Message}", exception.Message);

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Server error",
            Detail = exception.Message
        };

        httpContext.Response.StatusCode = problemDetails.Status.Value;

        await httpContext.Response
            .WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}