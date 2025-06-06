export const CODE_EDITOR_TEMPLATES: {
  [key: string]: {
    langName: string;
    solutionTemplate: string;
    mainMethodBodyContent: string;
  };
} = {
  "51": {
    langName: "csharp",
    solutionTemplate: `public class Solution\n{\n\t//define the solution function here\n\tpublic void SolutionFunction()\n\t{\n\n\n\t}\n}`,
    mainMethodBodyContent: `using System;\nusing System.Collections.Generic;\nusing System.Linq;\n\npublic class Program\n{\n    public static void Main(string[] args)\n    {\n        // Reade the stdIn from the system to receive the inputs\n        var input = Console.ReadLine();\n        // Instantiate the Solution class and call the corresponding method\n        var solution = new Solution();\n        var result = solution.SolutionMethod();\n        // the result of the solution should always be at the bottom\n        Console.WriteLine(result);\n    }\n}\n\n// DO NOT REMOVE THIS LINE - this is the user submitted solution function\n{solutionClass}`,
  },
  "63": {
    langName: "javascript",
    solutionTemplate: `//define the solution function here\nfunction solution() {\n\n}`,
    mainMethodBodyContent:
      "// DO NOT REMOVE THIS LINE - this is the system provided stdIn function\n{stdinFunc}\n\n// receive stdIn input from the system (if there is any)\nconst stdin = getInput();\n// print the final result. It should always be the last line of the code\nconsole.log(solution(inputArray));\n\n// DO NOT REMOVE THIS LINE - this is the user submitted solution function\n{solutionFunc}",
  },
};
