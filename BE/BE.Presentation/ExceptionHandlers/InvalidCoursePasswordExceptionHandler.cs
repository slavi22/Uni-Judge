using BE.Common.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace BE.Presentation.ExceptionHandlers;

public class InvalidCoursePasswordExceptionHandler : IExceptionHandler
{
    private ILogger<InvalidCoursePasswordExceptionHandler> _logger;

    public InvalidCoursePasswordExceptionHandler(ILogger<InvalidCoursePasswordExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not InvalidCoursePasswordException invalidCoursePasswordException)
        {
            return false;
        }

        _logger.LogError(
            invalidCoursePasswordException,
            "Exception occurred: {Message}",
            invalidCoursePasswordException.Message);
        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "An error occurred while processing the request.",
            Detail = invalidCoursePasswordException.Message
        };

        httpContext.Response.StatusCode = problemDetails.Status.Value;
        await httpContext.Response
            .WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}