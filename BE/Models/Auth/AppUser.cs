//using BE.Models.Submissions;

using BE.Models.Courses;
using BE.Models.Submissions;
using Microsoft.AspNetCore.Identity;

namespace BE.Models.Auth;

public class AppUser : IdentityUser
{
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
    public ICollection<UserCourseModel> UserCourses { get; set; } = new List<UserCourseModel>();
    public ICollection<UserSubmissionModel> UserSubmissions { get; set; } = new List<UserSubmissionModel>();
}