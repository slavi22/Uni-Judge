﻿// <auto-generated />
using System;
using BE.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BE.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("BE.Models.Models.Auth.AppUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("text");

                    b.Property<DateTime?>("RefreshTokenExpiryTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("BE.Models.Models.Courses.CoursesModel", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text");

                    b.Property<string>("CourseId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CourseId")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("BE.Models.Models.Courses.UserCourseModel", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("CourseId")
                        .HasColumnType("text");

                    b.HasKey("UserId", "CourseId");

                    b.HasIndex("CourseId");

                    b.ToTable("UserCourse");
                });

            modelBuilder.Entity("BE.Models.Models.Problem.ExpectedOutputListModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ExpectedOutput")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsSample")
                        .HasColumnType("boolean");

                    b.Property<string>("ProblemId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ProblemId");

                    b.ToTable("ExpectedOutputs");
                });

            modelBuilder.Entity("BE.Models.Models.Problem.LanguageModel", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Languages");

                    b.HasData(
                        new
                        {
                            Id = 51,
                            Name = "C#"
                        },
                        new
                        {
                            Id = 63,
                            Name = "JavaScript"
                        });
                });

            modelBuilder.Entity("BE.Models.Models.Problem.MainMethodBodyModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("LanguageId")
                        .HasColumnType("integer");

                    b.Property<string>("MainMethodBodyContent")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ProblemId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("SolutionTemplate")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ProblemId");

                    b.ToTable("MainMethodBodies");
                });

            modelBuilder.Entity("BE.Models.Models.Problem.ProblemLanguageModel", b =>
                {
                    b.Property<string>("ProblemId")
                        .HasColumnType("text");

                    b.Property<int>("LanguageId")
                        .HasColumnType("integer");

                    b.HasKey("ProblemId", "LanguageId");

                    b.HasIndex("LanguageId");

                    b.ToTable("ProblemLanguage");
                });

            modelBuilder.Entity("BE.Models.Models.Problem.ProblemModel", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text");

                    b.Property<string>("CourseId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ProblemId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("RequiredPercentageToPass")
                        .HasColumnType("integer");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.HasIndex("ProblemId")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("Problems");
                });

            modelBuilder.Entity("BE.Models.Models.Problem.StdInListModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool>("IsSample")
                        .HasColumnType("boolean");

                    b.Property<string>("ProblemId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("StdIn")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ProblemId");

                    b.ToTable("StdIns");
                });

            modelBuilder.Entity("BE.Models.Models.Submissions.TestCaseModel", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("CompileOutput")
                        .HasColumnType("text");

                    b.Property<string>("ExpectedOutput")
                        .HasColumnType("text");

                    b.Property<string>("HiddenExpectedOutput")
                        .HasColumnType("text");

                    b.Property<bool>("IsCorrect")
                        .HasColumnType("boolean");

                    b.Property<string>("Stderr")
                        .HasColumnType("text");

                    b.Property<string>("Stdout")
                        .HasColumnType("text");

                    b.Property<string>("UserSubmissionId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserSubmissionId");

                    b.ToTable("TestCases");
                });

            modelBuilder.Entity("BE.Models.Models.Submissions.TestCaseStatusModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("ResultId")
                        .HasColumnType("integer");

                    b.Property<string>("TestCaseId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("TestCaseId")
                        .IsUnique();

                    b.ToTable("TestCaseStatuses");
                });

            modelBuilder.Entity("BE.Models.Models.Submissions.UserSubmissionModel", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text");

                    b.Property<string>("CourseId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsPassing")
                        .HasColumnType("boolean");

                    b.Property<int>("LanguageId")
                        .HasColumnType("integer");

                    b.Property<string>("ProblemId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("SourceCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.HasIndex("LanguageId");

                    b.HasIndex("ProblemId");

                    b.HasIndex("UserId");

                    b.ToTable("UserSubmissions");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);

                    b.HasData(
                        new
                        {
                            Id = "1",
                            Name = "Student",
                            NormalizedName = "STUDENT"
                        },
                        new
                        {
                            Id = "2",
                            Name = "Teacher",
                            NormalizedName = "TEACHER"
                        },
                        new
                        {
                            Id = "3",
                            Name = "Admin",
                            NormalizedName = "ADMIN"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .HasColumnType("text");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("BE.Models.Models.Courses.CoursesModel", b =>
                {
                    b.HasOne("BE.Models.Models.Auth.AppUser", "User")
                        .WithMany("CreatedCourses")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("BE.Models.Models.Courses.UserCourseModel", b =>
                {
                    b.HasOne("BE.Models.Models.Courses.CoursesModel", "Course")
                        .WithMany("UserCourses")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BE.Models.Models.Auth.AppUser", "User")
                        .WithMany("UserCourses")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BE.Models.Models.Problem.ExpectedOutputListModel", b =>
                {
                    b.HasOne("BE.Models.Models.Problem.ProblemModel", "Problem")
                        .WithMany("ExpectedOutputList")
                        .HasForeignKey("ProblemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Problem");
                });

            modelBuilder.Entity("BE.Models.Models.Problem.MainMethodBodyModel", b =>
                {
                    b.HasOne("BE.Models.Models.Problem.ProblemModel", "Problem")
                        .WithMany("MainMethodBodiesList")
                        .HasForeignKey("ProblemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Problem");
                });

            modelBuilder.Entity("BE.Models.Models.Problem.ProblemLanguageModel", b =>
                {
                    b.HasOne("BE.Models.Models.Problem.LanguageModel", "Language")
                        .WithMany("ProblemLanguages")
                        .HasForeignKey("LanguageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BE.Models.Models.Problem.ProblemModel", "Problem")
                        .WithMany("ProblemLanguages")
                        .HasForeignKey("ProblemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Language");

                    b.Navigation("Problem");
                });

            modelBuilder.Entity("BE.Models.Models.Problem.ProblemModel", b =>
                {
                    b.HasOne("BE.Models.Models.Courses.CoursesModel", "Course")
                        .WithMany("Problems")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BE.Models.Models.Auth.AppUser", "User")
                        .WithMany("CreatedProblems")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BE.Models.Models.Problem.StdInListModel", b =>
                {
                    b.HasOne("BE.Models.Models.Problem.ProblemModel", "Problem")
                        .WithMany("StdInList")
                        .HasForeignKey("ProblemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Problem");
                });

            modelBuilder.Entity("BE.Models.Models.Submissions.TestCaseModel", b =>
                {
                    b.HasOne("BE.Models.Models.Submissions.UserSubmissionModel", "UserSubmission")
                        .WithMany("TestCases")
                        .HasForeignKey("UserSubmissionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserSubmission");
                });

            modelBuilder.Entity("BE.Models.Models.Submissions.TestCaseStatusModel", b =>
                {
                    b.HasOne("BE.Models.Models.Submissions.TestCaseModel", "TestCase")
                        .WithOne("TestCaseStatus")
                        .HasForeignKey("BE.Models.Models.Submissions.TestCaseStatusModel", "TestCaseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TestCase");
                });

            modelBuilder.Entity("BE.Models.Models.Submissions.UserSubmissionModel", b =>
                {
                    b.HasOne("BE.Models.Models.Courses.CoursesModel", "Course")
                        .WithMany("UserSubmissions")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BE.Models.Models.Problem.LanguageModel", "Language")
                        .WithMany("UserSubmissions")
                        .HasForeignKey("LanguageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BE.Models.Models.Problem.ProblemModel", "Problem")
                        .WithMany("UserSubmission")
                        .HasForeignKey("ProblemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BE.Models.Models.Auth.AppUser", "User")
                        .WithMany("UserSubmissions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("Language");

                    b.Navigation("Problem");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("BE.Models.Models.Auth.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("BE.Models.Models.Auth.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BE.Models.Models.Auth.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("BE.Models.Models.Auth.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BE.Models.Models.Auth.AppUser", b =>
                {
                    b.Navigation("CreatedCourses");

                    b.Navigation("CreatedProblems");

                    b.Navigation("UserCourses");

                    b.Navigation("UserSubmissions");
                });

            modelBuilder.Entity("BE.Models.Models.Courses.CoursesModel", b =>
                {
                    b.Navigation("Problems");

                    b.Navigation("UserCourses");

                    b.Navigation("UserSubmissions");
                });

            modelBuilder.Entity("BE.Models.Models.Problem.LanguageModel", b =>
                {
                    b.Navigation("ProblemLanguages");

                    b.Navigation("UserSubmissions");
                });

            modelBuilder.Entity("BE.Models.Models.Problem.ProblemModel", b =>
                {
                    b.Navigation("ExpectedOutputList");

                    b.Navigation("MainMethodBodiesList");

                    b.Navigation("ProblemLanguages");

                    b.Navigation("StdInList");

                    b.Navigation("UserSubmission");
                });

            modelBuilder.Entity("BE.Models.Models.Submissions.TestCaseModel", b =>
                {
                    b.Navigation("TestCaseStatus")
                        .IsRequired();
                });

            modelBuilder.Entity("BE.Models.Models.Submissions.UserSubmissionModel", b =>
                {
                    b.Navigation("TestCases");
                });
#pragma warning restore 612, 618
        }
    }
}
