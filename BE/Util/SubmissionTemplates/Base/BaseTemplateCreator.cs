namespace BE.Util.SubmissionTemplates.Base;

public abstract class BaseTemplateCreator
{
    // this method iterates over the "stdInArray" and sets the character corresponding to the data type of the element
    protected static string[] SetArrayItemsTypeCharacter(string[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            // checks if the element is a char if it is it sets its value to be in single quotes
            if (array[i].Length == 1 && char.TryParse(array[i], out _))
            {
                array[i] = $"'{array[i]}'";
            }
            // checks if the element is NOT a number which means it should be a string so it sets its value to be in double quotes
            else if (!double.TryParse(array[i], out _))
            {
                array[i] = $"\"{array[i]}\"";
            }
        }

        return array;
    }

    protected static string SetTypeOfInput(string input)
    {
        // TODO: figure out how to do it for complex objects like lists, arrays, etc., since they are plain words and should have double quotes around them
        if (!double.TryParse(input, out _))
        {
            return $"\"{input}\"";
        }

        return input;
    }
}