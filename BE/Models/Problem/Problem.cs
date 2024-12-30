namespace BE.Models.Problem;

public class Problem
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<string> ExpectedOutputList { get; set; }
    public List<string>? StdInList { get; set; }
    // these 2 should be initialized according to the documentation => https://learn.microsoft.com/en-us/ef/core/modeling/relationships/navigations#collection-navigations
    // "if i interacted with the navigation property list then it would give me the exception but since i go directly to the table thats why it doesnt care whether its initialized or not"
    // so i have to initialize them if i want to update the other side of the relationship =>
    // "If you reference a new entity from the navigation property of an entity that is already tracked by the context, the entity will be discovered and inserted into the database."
    // https://learn.microsoft.com/en-us/ef/core/saving/related-data#adding-a-related-entity
    public List<MainMethodBody> MainMethodBodiesList { get; set; } = new List<MainMethodBody>();
    public List<ProblemLanguage> ProblemLanguages { get; } = new List<ProblemLanguage>();
}
