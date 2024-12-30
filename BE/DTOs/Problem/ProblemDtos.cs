namespace BE.DTOs.Problem;

public class CreateProblemDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public List<MainMethodBodyDto> MainMethodBodiesList { get; set; }
    public List<string> ExpectedOutputList { get; set; }
    public List<LanguagesEnum> LanguagesList { get; set; }
    public List<string>? StdInList { get; set; }
}

public class MainMethodBodyDto
{
    public LanguagesEnum Language { get; set; }
    public string MainMethodBodyContent { get; set; }
}

public enum LanguagesEnum
{
    // we will have a select with the value set as the enum index and between the option tags we will have the enum name
    // C# language id,
    CSharp = 51
}