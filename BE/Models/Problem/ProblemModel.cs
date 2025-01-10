using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BE.Models.Courses;
using BE.Models.Submissions;

namespace BE.Models.Problem;

public class ProblemModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    [Range(0, 100)]
    public int RequiredPercentageToPass { get; set; }

    public string CourseId { get; set; }
    public CoursesModel Course { get; set; }
    public UserSubmissionModel UserSubmission { get; set; }

    public List<ExpectedOutputListModel> ExpectedOutputList { get; set; } = new List<ExpectedOutputListModel>();
    public List<StdInListModel> StdInList { get; set; } = new List<StdInListModel>();

    // these 2 should be initialized according to the documentation => https://learn.microsoft.com/en-us/ef/core/modeling/relationships/navigations#collection-navigations
    // "if i interacted with the navigation property list then it would give me the exception but since i go directly to the table thats why it doesnt care whether its initialized or not"
    // so i have to initialize them if i want to update the other side of the relationship =>
    // "If you reference a new entity from the navigation property of an entity that is already tracked by the context, the entity will be discovered and inserted into the database."
    // https://learn.microsoft.com/en-us/ef/core/saving/related-data#adding-a-related-entity
    public List<MainMethodBodyModel> MainMethodBodiesList { get; set; } = new List<MainMethodBodyModel>();
    public List<ProblemLanguageModel> ProblemLanguages { get; } = new List<ProblemLanguageModel>();
}