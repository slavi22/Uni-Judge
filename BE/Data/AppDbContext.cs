using BE.Models.Auth;
using BE.Models.Courses;
using BE.Models.Problem;
using BE.Models.Submissions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BE.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<AppUser>(options)
{
    // "Problem" table
    public DbSet<ProblemModel> Problems { get; set; }
    public DbSet<LanguageModel> Languages { get; set; }
    public DbSet<MainMethodBodyModel> MainMethodBodies { get; set; }
    public DbSet<ExpectedOutputListModel> ExpectedOutputs { get; set; }
    public DbSet<StdInListModel> StdIns { get; set; }

    // "Courses" table
    public DbSet<CoursesModel> Courses { get; set; }

    // "Submission" table
    public DbSet<TestCaseModel> TestCases { get; set; }
    public DbSet<TestCaseStatusModel> TestCaseStatuses { get; set; }
    public DbSet<UserSubmissionModel> UserSubmissions { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        SeedRoles(builder);
        SeedLanguages(builder);
        // Problem Language many-to-many relationship
        // https://www.learnentityframeworkcore.com/configuration/many-to-many-relationship-configuration#:~:text=The%20join%20table%20will,to%20map%20it%20successfully%3A
        builder.Entity<ProblemLanguageModel>()
            .HasKey(pl => new { pl.ProblemId, pl.LanguageId });

        builder.Entity<ProblemLanguageModel>()
            .HasOne(pl => pl.Problem)
            .WithMany(p => p.ProblemLanguages)
            .HasForeignKey(pl => pl.ProblemId);

        builder.Entity<ProblemLanguageModel>()
            .HasOne(pl => pl.Language)
            .WithMany(l => l.ProblemLanguages)
            .HasForeignKey(pl => pl.LanguageId);

        // UserCourse many-to-many relationship
        builder.Entity<UserCourseModel>()
            .HasKey(uc => new { uc.UserId, uc.CourseId });
        builder.Entity<UserCourseModel>()
            .HasOne(uc => uc.User)
            .WithMany(u => u.UserCourses)
            .HasForeignKey(uc => uc.UserId);
        builder.Entity<UserCourseModel>()
            .HasOne(uc => uc.Course)
            .WithMany(c => c.UserCourses)
            .HasForeignKey(uc => uc.CourseId);
    }

    private static void SeedRoles(ModelBuilder builder)
    {
        builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = "1",
                Name = "Student",
                NormalizedName = "STUDENT"
            },
            new IdentityRole
            {
                Id = "2",
                Name = "Teacher",
                NormalizedName = "TEACHER"
            },
            new IdentityRole
            {
                Id = "3",
                Name = "Admin",
                NormalizedName = "ADMIN"
            }
        );
    }

    private static void SeedLanguages(ModelBuilder builder)
    {
        builder.Entity<LanguageModel>().HasData(new LanguageModel
            {
                Id = 51,
                Name = "C#"
            }
        );
    }
}