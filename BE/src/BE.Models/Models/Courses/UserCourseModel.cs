using System.ComponentModel.DataAnnotations.Schema;
using BE.Models.Models.Auth;

namespace BE.Models.Models.Courses;

[Table("UserCourse")]
public class UserCourseModel
{
    public string UserId { get; set; }
    public AppUser User { get; set; }
    public string CourseId { get; set; }
    public CoursesModel Course { get; set; }
}