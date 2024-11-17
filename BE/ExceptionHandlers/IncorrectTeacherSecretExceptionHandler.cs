using BE.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace BE.ExceptionHandlers;

public class IncorrectTeacherSecretExceptionHandler : IExceptionHandler
{
    private readonly ILogger<IncorrectTeacherSecretExceptionHandler> _logger;

    public IncorrectTeacherSecretExceptionHandler(ILogger<IncorrectTeacherSecretExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not IncorrectTeacherSecretException incorrectTeacherSecretException)
        {
            return false;
        }

        _logger.LogError(
            incorrectTeacherSecretException,
            "Exception occurred: {Message}",
            incorrectTeacherSecretException.Message);
        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "One or more validation errors occurred.",
            Detail = incorrectTeacherSecretException.Message
        };

        httpContext.Response.StatusCode = problemDetails.Status.Value;
        await httpContext.Response
            .WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}