namespace BE.Util.SubmissionTemplates;

using System;
using System.Collections.Generic;
using System.Linq;

public class Program
{
    public static void Mainz(string[] args)
    {
        // with normal parameters split by space
        /*var input = Console.ReadLine();
        var inputArray = input.Split(" ");
        var solution = new Solution();
        var result = solution.SolutionMethod(int.Parse(inputArray[0]), int.Parse(inputArray[1]));
        Console.WriteLine(result);*/

        // with normal parameters split by comma (for lists etc)
        var input = Console.ReadLine();
        var inputArray = input.Split(",");
        var inputList = inputArray.Select(int.Parse).ToList();
        var solution = new Solution();
        var result = solution.SolutionMethodWithListInput(inputList);
        Console.WriteLine(result);
    }

    //{solutionClass}
}



public class Solution
{
    public int SolutionMethod(int num1, int num2)
    {
        return num1 + num2;
    }

    public int SolutionMethodWithListInput(List<int> input)
    {
        return input[0];
    }
}