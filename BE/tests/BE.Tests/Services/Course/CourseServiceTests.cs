using System.Security.Claims;
using BE.Business.Services.Implementations;
using BE.Common.Exceptions;
using BE.DataAccess.Repositories.Interfaces;
using BE.DTOs.DTOs.Course.Requests;
using BE.Models.Models.Auth;
using BE.Models.Models.Courses;
using BE.Models.Models.Problem;
using Microsoft.AspNetCore.Http;
using Moq;

namespace BE.Tests.Services.Course;

public class CourseServiceTests
{
    private readonly Mock<ICourseRepository> _courseRepositoryMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
    private readonly CourseService _courseService;

    public CourseServiceTests()
    {
        _courseRepositoryMock = new Mock<ICourseRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        _courseService = new CourseService(_courseRepositoryMock.Object, _userRepositoryMock.Object,
            _httpContextAccessorMock.Object);
    }

    [Fact]
    public async Task GetProblemsForCourse_ReturnsProblemsList_WhenCourseExists()
    {
        // Arrange
        var courseId = "course123";
        var course = new CoursesModel
        {
            Id = courseId,
            Problems = new List<ProblemModel>
                { new ProblemModel { ProblemId = "problem1", Name = "Problem 1", Description = "Description 1" } }
        };
        _courseRepositoryMock.Setup(repo => repo.GetCourseAndProblemsByIdAsync(courseId)).ReturnsAsync(course);

        // Act
        var result = await _courseService.GetProblemsForCourse(courseId);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("problem1", result[0].ProblemId);
    }

    [Fact]
    public async Task GetProblemsForCourse_ReturnsNull_WhenCourseDoesNotExist()
    {
        // Arrange
        var courseId = "course123";
        _courseRepositoryMock.Setup(repo => repo.GetCourseAndProblemsByIdAsync(courseId))
            .ReturnsAsync((CoursesModel)null);

        // Act
        var result = await _courseService.GetProblemsForCourse(courseId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task SignUpForCourse_ThrowsInvalidCoursePasswordException_WhenPasswordIsIncorrect()
    {
        // Arrange
        var dto = new SignUpForCourseDto { CourseId = "course123", Password = "wrongpassword" };
        var course = new CoursesModel { Id = "course123", Password = "correctpassword" };
        _courseRepositoryMock.Setup(repo => repo.GetCourseByCourseIdAsync(dto.CourseId)).ReturnsAsync(course);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidCoursePasswordException>(() => _courseService.SignUpForCourse(dto));
    }

    [Fact]
    public async Task SignUpForCourse_ReturnsTrue_WhenSignUpIsSuccessful()
    {
        // Arrange
        var dto = new SignUpForCourseDto { CourseId = "course123", Password = "correctpassword" };
        var course = new CoursesModel { Id = "course123", Password = "correctpassword" };
        var user = new AppUser { Id = Guid.Empty.ToString() };
        var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, "user@email.com") }, "mock"));
        _courseRepositoryMock.Setup(repo => repo.GetCourseByCourseIdAsync(dto.CourseId)).ReturnsAsync(course);
        _httpContextAccessorMock.Setup(context => context.HttpContext).Returns(new DefaultHttpContext { User = userPrincipal });
        _userRepositoryMock.Setup(repo => repo.GetCurrentUserAsync(It.IsAny<string>())).ReturnsAsync(user);
        _courseRepositoryMock.Setup(repo => repo.SignUpForCourseAsync(course, It.IsAny<UserCourseModel>()))
            .ReturnsAsync(true);

        // Act
        var result = await _courseService.SignUpForCourse(dto);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task CreateNewCourse_ThrowsDuplicateCourseIdException_WhenCourseAlreadyExists()
    {
        // Arrange
        var dto = new CreateCourseDto
            { CourseId = "course123", Name = "Course 123", Description = "Description", Password = "password" };
        var course = new CoursesModel { Id = Guid.Empty.ToString() };
        _courseRepositoryMock.Setup(repo => repo.GetCourseByCourseIdAsync(dto.CourseId)).ReturnsAsync(course);

        // Act & Assert
        await Assert.ThrowsAsync<DuplicateCourseIdException>(() => _courseService.CreateNewCourse(dto));
    }

    [Fact]
    public async Task CreateNewCourse_CreatesCourse_WhenCourseDoesNotExist()
    {
        //TODO: update rider and test if it will show the exception at the correct line
        // Arrange
        var dto = new CreateCourseDto
            { CourseId = "course123", Name = "Course 123", Description = "Description", Password = "password" };
        var user = new AppUser { Id = Guid.Empty.ToString() };
        var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, "user@email.com") }, "mock"));
        _courseRepositoryMock.Setup(repo => repo.GetCourseByCourseIdAsync(dto.CourseId))
            .ReturnsAsync((CoursesModel)null);
        _httpContextAccessorMock.Setup(context => context.HttpContext).Returns(new DefaultHttpContext { User = userPrincipal });
        _userRepositoryMock.Setup(repo => repo.GetCurrentUserAsync(It.IsAny<string>())).ReturnsAsync(user);
        _courseRepositoryMock.Setup(repo => repo.CreateCourseAsync(It.IsAny<CoursesModel>()))
            .Callback<CoursesModel>(course => course.Id = Guid.Empty.ToString());

        // Act
        await _courseService.CreateNewCourse(dto);

        // Assert
        _courseRepositoryMock.Verify(repo => repo.CreateCourseAsync(It.IsAny<CoursesModel>()), Times.Once);
    }
}