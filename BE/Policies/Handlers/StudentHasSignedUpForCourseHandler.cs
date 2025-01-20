using BE.Data;
using BE.Policies.Requirements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace BE.Policies.Handlers;

public class StudentHasSignedUpForCourseHandler : AuthorizationHandler<StudentHasSignedUpForCourseRequirement>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly AppDbContext _dbContext;

    public StudentHasSignedUpForCourseHandler(IHttpContextAccessor httpContextAccessor, AppDbContext dbContext)
    {
        _httpContextAccessor = httpContextAccessor;
        _dbContext = dbContext;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
        StudentHasSignedUpForCourseRequirement requirement)
    {
        var routeData = _httpContextAccessor.HttpContext.GetRouteData();
        var courseId = routeData.Values["courseId"].ToString();
        // https://stackoverflow.com/a/72832313
        var userCourse = await _dbContext.Courses.Where(c => c.Id == courseId).Include(c => c.UserCourses)
            .FirstOrDefaultAsync();
        if (userCourse == null)
        {
            return;
        }

        context.Succeed(requirement);
    }
}