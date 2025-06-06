namespace BE.Common.Util.SubmissionTemplates;

public static class JsTemplate
{
    public static string ConstructJsSolutionBase(string mainMethodBody, string solutionFunc)
    {
        var stdinFunc =
            "const fs = require('fs');\n\nfunction getInput() {\n  return fs.readFileSync(0, 'utf8').trim();\n}";
        var body = mainMethodBody.Replace("{stdinFunc}", stdinFunc).Replace("{solutionFunc}", solutionFunc);
        return body;
    }
}