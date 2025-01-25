using System.Security.Claims;
using BE.Data;
using BE.Policies.Requirements;
using BE.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace BE.Policies.Handlers;

public class StudentHasSignedUpForCourseHandler : AuthorizationHandler<StudentHasSignedUpForCourseRequirement>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly AppDbContext _dbContext;
    private readonly IUserRepository _userRepository;

    public StudentHasSignedUpForCourseHandler(IHttpContextAccessor httpContextAccessor, AppDbContext dbContext,
        IUserRepository userRepository)
    {
        _httpContextAccessor = httpContextAccessor;
        _dbContext = dbContext;
        _userRepository = userRepository;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
        StudentHasSignedUpForCourseRequirement requirement)
    {
        var routeData = _httpContextAccessor.HttpContext.GetRouteData();
        var courseId = routeData.Values["courseId"].ToString();
        // https://stackoverflow.com/a/72832313
        var userCourse = await _dbContext.Courses.Where(c => c.Id == courseId).Include(c => c.UserCourses)
            .FirstOrDefaultAsync();
        var currentUser = await _userRepository.GetCurrentUserAsync();
        if (userCourse.UserCourses.Any(x => x.UserId == currentUser.Id) == false)
        {
            return;
        }

        context.Succeed(requirement);
    }
}