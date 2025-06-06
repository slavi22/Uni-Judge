using System.Text.Json;
using BE.Business.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using MonacoRoslynCompletionProvider;
using MonacoRoslynCompletionProvider.Api;

namespace BE.Business.Services.Implementations;

public class CodeIntellisenseService : ICodeIntellisenseService
{
    //TODO: add test
    public async Task CSharpIntellisense(string text, HttpContext httpContext)
    {
        if (httpContext.Request.Path.Value?.EndsWith("complete") == true)
        {
            var tabCompletionRequest = JsonSerializer.Deserialize<TabCompletionRequest>(text);
            var tabCompletionResults = await CompletitionRequestHandler.Handle(tabCompletionRequest);
            await JsonSerializer.SerializeAsync(httpContext.Response.Body, tabCompletionResults);
            return;
        }

        if (httpContext.Request.Path.Value?.EndsWith("signature") == true)
        {
            var signatureHelpRequest = JsonSerializer.Deserialize<SignatureHelpRequest>(text);
            var signatureHelpResult = await CompletitionRequestHandler.Handle(signatureHelpRequest);
            await JsonSerializer.SerializeAsync(httpContext.Response.Body, signatureHelpResult);
            return;
        }

        if (httpContext.Request.Path.Value?.EndsWith("hover") == true)
        {
            var hoverInfoRequest = JsonSerializer.Deserialize<HoverInfoRequest>(text);
            var hoverInfoResult = await CompletitionRequestHandler.Handle(hoverInfoRequest);
            await JsonSerializer.SerializeAsync(httpContext.Response.Body, hoverInfoResult);
            return;
        }

        if (httpContext.Request.Path.Value?.EndsWith("codeCheck") == true)
        {
            var codeCheckRequest = JsonSerializer.Deserialize<CodeCheckRequest>(text);
            var codeCheckResults = await CompletitionRequestHandler.Handle(codeCheckRequest);
            await JsonSerializer.SerializeAsync(httpContext.Response.Body, codeCheckResults);
        }
    }
}