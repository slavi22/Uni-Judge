﻿using System.Security.Claims;
using BE.Business.Services.Implementations;
using BE.DataAccess.Repositories.Interfaces;
using BE.DTOs.DTOs.Problem.Requests;
using BE.Models.Models.Courses;
using BE.Models.Models.Problem.Enums;
using Microsoft.AspNetCore.Http;
using Moq;

namespace BE.Tests.Services.Problem;

public class ProblemServiceTests
{
    private readonly Mock<IProblemRepository> _problemRepositoryMock;
    private readonly Mock<ICourseRepository> _courseRepositoryMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
    private readonly ProblemService _problemService;

    public ProblemServiceTests()
    {
        _problemRepositoryMock = new Mock<IProblemRepository>();
        _courseRepositoryMock = new Mock<ICourseRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        _problemService = new ProblemService(_problemRepositoryMock.Object, _courseRepositoryMock.Object,
            _userRepositoryMock.Object, _httpContextAccessorMock.Object);
    }

    [Fact]
    public async Task CreateProblem_ShouldCreateProblemSuccessfully()
    {
        // Arrange
        var clientProblemDto = new ClientProblemDto
        {
            ProblemId = "problem1",
            Name = "Sample Problem",
            Description = "Sample Description",
            RequiredPercentageToPass = 80,
            CourseId = "course1",
            MainMethodBodiesList = new List<MainMethodBodyDto>
            {
                new MainMethodBodyDto
                    { LanguageId = (LanguagesEnum)1, MainMethodBodyContent = "Content", SolutionTemplate = "Template" }
            },
            ExpectedOutputList = new List<ExpectedOutputListDto>
            {
                new ExpectedOutputListDto { IsSample = true, ExpectedOutput = "Output" }
            },
            StdInList = new List<StdInListDto> { new StdInListDto { IsSample = false, StdIn = "Input" } },
            LanguagesList = new List<LanguagesEnum> { (LanguagesEnum)1 }
        };

        var courseEntity = new CoursesModel { CourseId = "course1" };
        var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, "user@email.com") }, "mock"));
        _httpContextAccessorMock.Setup(context => context.HttpContext).Returns(new DefaultHttpContext { User = userPrincipal });
        _courseRepositoryMock.Setup(repo => repo.GetCourseByCourseIdAsync(It.IsAny<string>()))
            .ReturnsAsync(courseEntity);

        // Act
        var result = await _problemService.CreateProblemAsync(clientProblemDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(clientProblemDto.ProblemId, result.ProblemId);
        Assert.Equal(clientProblemDto.Name, result.Name);
        Assert.Equal(clientProblemDto.Description, result.Description);
        Assert.Equal(clientProblemDto.RequiredPercentageToPass, result.RequiredPercentageToPass);
        Assert.Equal(clientProblemDto.CourseId, result.CourseId);
    }

    [Fact]
    public async Task CreateProblem_ShouldThrowException_WhenCourseNotFound()
    {
        // Arrange
        var clientProblemDto = new ClientProblemDto
        {
            ProblemId = "problem1",
            Name = "Sample Problem",
            Description = "Sample Description",
            RequiredPercentageToPass = 80,
            CourseId = "course1",
            MainMethodBodiesList = new List<MainMethodBodyDto>(),
            ExpectedOutputList = new List<ExpectedOutputListDto>(),
            StdInList = new List<StdInListDto>(),
            LanguagesList = new List<LanguagesEnum>()
        };

        _courseRepositoryMock.Setup(repo => repo.GetCourseByCourseIdAsync(It.IsAny<string>()))
            .Throws<Exception>();

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _problemService.CreateProblemAsync(clientProblemDto));
    }
}