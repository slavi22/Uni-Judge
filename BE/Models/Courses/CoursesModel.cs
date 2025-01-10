using System.ComponentModel.DataAnnotations.Schema;
using BE.Models.Problem;

namespace BE.Models.Courses;

public class CoursesModel
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public ICollection<ProblemModel> Problems { get; set; } = new List<ProblemModel>();
}