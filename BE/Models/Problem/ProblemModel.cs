using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BE.Models.Courses;
using BE.Models.Submissions;
using Microsoft.EntityFrameworkCore;

namespace BE.Models.Problem;

// https://code-maze.com/efcore-add-unique-constraints-to-a-property-code-first/
[Index(nameof(ProblemId), IsUnique = true)]
public class ProblemModel
{
    //TODO: Add the problemId field which specifies the problem name for the respective course

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string Id { get; set; }
    public string ProblemId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    [Range(0, 100)]
    public int RequiredPercentageToPass { get; set; }

    public string CourseId { get; set; }
    public CoursesModel Course { get; set; }
    public ICollection<UserSubmissionModel> UserSubmission { get; set; } = new List<UserSubmissionModel>();

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