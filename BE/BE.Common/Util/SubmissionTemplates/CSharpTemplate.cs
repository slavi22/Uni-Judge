namespace BE.Common.Util.SubmissionTemplates;

public static class CSharpTemplate
{
    // maybe we get the solution method from the db?

    // the template for the solution class body when there is stdin
    //private const string CsharpTemplateWithStdIn =
    //"using System;\n\nclass Program\n{\n static void Main(string[] args)\n {\n string input = Console.ReadLine();\n string[] inputArray = input.Split(\"\\n\");\n Solution solution = new Solution();\n var result = solution.{solutionMethod}({methodParameters});\n Console.WriteLine(result);\n }\n}\n\n{solutionClass}";
    // the template for the solution class body when there is no stdin
    //private const string CsharpTemplateWithoutStdIn =
    //"using System;\n\nclass Program\n{\n static void Main(string[] args)\n {\n Solution solution = new Solution();\n var result = solution.{solutionMethod}();\n Console.WriteLine(result);\n }\n}\n\n{solutionClass}";


    // constructs the solution class body
    public static string ConstructCSharpSolutionBase(string mainMethodBody, string solutionClass)
    {
        var body = mainMethodBody.Replace("{solutionClass}", solutionClass);
        /*.Replace("{methodName}", methodName)
            .Replace("{methodParameters}", methodParameters).Replace("{expectedOutput}", SetTypeOfInput(expectedOutput))*/;
        return body;
    }

}