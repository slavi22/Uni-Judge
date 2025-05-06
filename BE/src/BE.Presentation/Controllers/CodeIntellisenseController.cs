using System.Net;
using BE.Business.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BE.Presentation.Controllers
{
    [Route("api/code-intellisense")]
    [ApiController]
    public class CodeIntellisenseController : ControllerBase
    {
        private readonly ICodeIntellisenseService _intellisenseService;

        public CodeIntellisenseController(ICodeIntellisenseService intellisenseService)
        {
            _intellisenseService = intellisenseService;
        }

        //TODO: add test
        [HttpPost("csharp/{request}")]
        public async Task CSharpCompletion()
        {
            using var reader = new StreamReader(HttpContext.Request.Body);
            var text = await reader.ReadToEndAsync();
            if (text != null)
            {
               await _intellisenseService.CSharpIntellisense(text, HttpContext);
               return;
            }
            HttpContext.Response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
        }
    }
}
