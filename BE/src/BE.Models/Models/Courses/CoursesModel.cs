using System.ComponentModel.DataAnnotations.Schema;
using BE.Models.Models.Auth;
using BE.Models.Models.Problem;
using BE.Models.Models.Submissions;
using Microsoft.EntityFrameworkCore;

namespace BE.Models.Models.Courses;

[Index(nameof(CourseId), IsUnique = true)]
public class CoursesModel
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string Id { get; set; }
    public string CourseId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string? Password { get; set; }
    // navigation property to the creator of the course
    public string UserId { get; set; }
    public AppUser User { get; set; }
    public ICollection<UserCourseModel> UserCourses { get; set; } = new List<UserCourseModel>();
    public ICollection<UserSubmissionModel> UserSubmissions { get; set; } = new List<UserSubmissionModel>();
    public ICollection<ProblemModel> Problems { get; set; } = new List<ProblemModel>();
}