using BE.Models.Auth;
using BE.Models.Problem;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BE.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<AppUser>(options)
{
    public DbSet<Problem> Problems { get; set; }
    public DbSet<Language> Languages { get; set; }
    public DbSet<MainMethodBody> MainMethodBodies { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        SeedRoles(builder);
        SeedLanguages(builder);
        // https://www.learnentityframeworkcore.com/configuration/many-to-many-relationship-configuration#:~:text=The%20join%20table%20will,to%20map%20it%20successfully%3A
        builder.Entity<ProblemLanguage>()
            .HasKey(pl => new { pl.ProblemId, pl.LanguageId });

        builder.Entity<ProblemLanguage>()
            .HasOne(pl => pl.Problem)
            .WithMany(p => p.ProblemLanguages)
            .HasForeignKey(pl => pl.ProblemId);

        builder.Entity<ProblemLanguage>()
            .HasOne(pl => pl.Language)
            .WithMany(l => l.ProblemLanguages)
            .HasForeignKey(pl => pl.LanguageId);
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
        builder.Entity<Language>().HasData(new Language
            {
                Id = 51,
                Name = "C#"
            }
        );
    }
}