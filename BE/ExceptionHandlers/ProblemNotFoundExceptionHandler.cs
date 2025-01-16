using BE.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace BE.ExceptionHandlers;

public class ProblemNotFoundExceptionHandler : IExceptionHandler
{
    private readonly ILogger<ProblemNotFoundExceptionHandler> _logger;

    public ProblemNotFoundExceptionHandler(ILogger<ProblemNotFoundExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not ProblemNotFoundException problemNotFoundException)
        {
            return false;
        }

        _logger.LogError(
            problemNotFoundException,
            "Exception occurred: {Message}",
            problemNotFoundException.Message);
        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status404NotFound,
            Title = "An error occurred while processing the request.",
            Detail = problemNotFoundException.Message
        };

        httpContext.Response.StatusCode = problemDetails.Status.Value;
        await httpContext.Response
            .WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}