using System.Text;
using BE.Util.SubmissionTemplates.Base;

namespace BE.Util.SubmissionTemplates;

public abstract class CSharpTemplate : BaseTemplateCreator
{
    // maybe we get the solution method from the db

    // the template for the solution class body when there is stdin
    private const string CsharpTemplateWithStdIn =
        "using System;\n\nclass Program\n{\n static void Main(string[] args)\n {\n string input = Console.ReadLine();\n string[] inputArray = input.Split(\"\\n\");\n Solution solution = new Solution();\n var result = solution.{solutionMethod}({methodParameters});\n Console.WriteLine(result);\n }\n}\n\n{solutionClass}";
    // the template for the solution class body when there is no stdin
    private const string CsharpTemplateWithoutStdIn =
        "using System;\n\nclass Program\n{\n static void Main(string[] args)\n {\n Solution solution = new Solution();\n var result = solution.{solutionMethod}();\n Console.WriteLine(result);\n }\n}\n\n{solutionClass}";


    // constructs the solution class body
    public static string ConstructSolution(string solutionMethod, string solutionClass, string stdIn)
    {
        string body;
        // checks if there is stdin and uses the according method to construct the solution class body
        if (stdIn != null)
        {
            body = SolutionWithStdIn(solutionMethod, solutionClass, stdIn);
        }
        else
        {
            body = SolutionWithoutStdIn(solutionMethod, solutionClass);
        }
        return body;
    }

    // constructs the solution class body when there is stdin
    private static string SolutionWithStdIn(string solutionMethod, string solutionClass, string stdIn)
    {
        // splits the stdin by new line so then we can map the inputs to the method parameters
        var stdInArray = Encoding.UTF8.GetString(Convert.FromBase64String(stdIn)).Split("\n");
        // constructs the solution class body by replacing the placeholders with the actual values
        var body = CsharpTemplateWithStdIn.Replace("{solutionMethod}", solutionMethod)
            .Replace("{solutionClass}", Encoding.UTF8.GetString(Convert.FromBase64String(solutionClass)))
            .Replace("{methodParameters}", string.Join(", ", SetArrayItemsTypeCharacter(stdInArray)));
        // returns the solution class body as a base64 string
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(body));
    }

    // constructs the solution class body when there is no stdin
    private static string SolutionWithoutStdIn(string solutionMethod, string solutionClass)
    {
        // constructs the solution class body by replacing the placeholders with the actual values
        var body = CsharpTemplateWithoutStdIn.Replace("{solutionMethod}", solutionMethod)
            .Replace("{solutionClass}", Encoding.UTF8.GetString(Convert.FromBase64String(solutionClass)));
        // returns the solution class body as a base64 string
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(body));
    }
}