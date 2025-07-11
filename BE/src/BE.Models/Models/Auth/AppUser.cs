﻿using BE.Models.Models.Courses;
using BE.Models.Models.Problem;
using BE.Models.Models.Submissions;
using Microsoft.AspNetCore.Identity;

namespace BE.Models.Models.Auth;

public class AppUser : IdentityUser
{
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
    public ICollection<UserCourseModel> UserCourses { get; set; } = new List<UserCourseModel>();
    public ICollection<UserSubmissionModel> UserSubmissions { get; set; } = new List<UserSubmissionModel>();
    public ICollection<ProblemModel> CreatedProblems { get; set; } = new List<ProblemModel>();
    public ICollection<CoursesModel> CreatedCourses { get; set; } = new List<CoursesModel>();
}