using BE.Common.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace BE.Presentation.ExceptionHandlers;

public class DuplicateCourseIdExceptionHandler : IExceptionHandler
{
    private ILogger<DuplicateCourseIdExceptionHandler> _logger;

    public DuplicateCourseIdExceptionHandler(ILogger<DuplicateCourseIdExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not DuplicateCourseIdException duplicateCourseIdException)
        {
            return false;
        }

        _logger.LogError(
            duplicateCourseIdException,
            "Exception occurred: {Message}",
            duplicateCourseIdException.Message);
        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "An error occurred while processing the request.",
            Detail = duplicateCourseIdException.Message
        };

        httpContext.Response.StatusCode = problemDetails.Status.Value;
        await httpContext.Response
            .WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}