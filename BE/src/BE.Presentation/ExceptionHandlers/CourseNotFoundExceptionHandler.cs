using BE.Common.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace BE.Presentation.ExceptionHandlers;

public class CourseNotFoundExceptionHandler : IExceptionHandler
{
    private ILogger<CourseNotFoundExceptionHandler> _logger;

    public CourseNotFoundExceptionHandler(ILogger<CourseNotFoundExceptionHandler> logger)
    {
        _logger = logger;
    }


    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not CourseNotFoundException courseNotFoundException)
        {
            return false;
        }

        _logger.LogError(
            courseNotFoundException,
            "Exception occurred: {Message}",
            courseNotFoundException.Message);
        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status404NotFound,
            Title = "An error occurred while processing the request.",
            Detail = courseNotFoundException.Message
        };

        httpContext.Response.StatusCode = problemDetails.Status.Value;
        await httpContext.Response
            .WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}