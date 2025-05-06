using Microsoft.AspNetCore.Http;

namespace BE.Business.Services.Interfaces;

public interface ICodeIntellisenseService
{
    /// <summary>
    /// Processes C# code intellisense requests and provides appropriate responses.
    /// </summary>
    /// <param name="text">The request content text to be deserialized into specific request objects.</param>
    /// <param name="httpContext">The HTTP context containing request details and used for response.</param>
    /// <returns>A task representing the asynchronous operation of processing the intellisense request.</returns>
    //TODO: add test
    Task CSharpIntellisense(string text, HttpContext httpContext);
}