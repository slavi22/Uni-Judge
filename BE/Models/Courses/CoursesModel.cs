using System.ComponentModel.DataAnnotations.Schema;
using BE.Models.Problem;
using BE.Models.Submissions;
using Newtonsoft.Json;

namespace BE.Models.Courses;

public class CoursesModel
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public ICollection<UserSubmissionModel> UserSubmissions { get; set; } = new List<UserSubmissionModel>();
    public ICollection<ProblemModel> Problems { get; set; } = new List<ProblemModel>();
}