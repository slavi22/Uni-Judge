using BE.Common.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace BE.Presentation.ExceptionHandlers;

public class DuplicateProblemIdExceptionHandler : IExceptionHandler
{
    private ILogger<DuplicateProblemIdExceptionHandler> _logger;

    public DuplicateProblemIdExceptionHandler(ILogger<DuplicateProblemIdExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not DuplicateProblemIdException duplicateProblemIdException)
        {
            return false;
        }

        _logger.LogError(
            duplicateProblemIdException,
            "Exception occurred: {Message}",
            duplicateProblemIdException.Message);
        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "An error occurred while processing the request.",
            Detail = duplicateProblemIdException.Message
        };

        httpContext.Response.StatusCode = problemDetails.Status.Value;
        await httpContext.Response
            .WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}