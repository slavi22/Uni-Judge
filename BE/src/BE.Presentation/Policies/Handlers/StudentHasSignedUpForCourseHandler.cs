using BE.DataAccess.Data;
using BE.DataAccess.Repositories.Interfaces;
using BE.Presentation.Policies.Requirements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.EntityFrameworkCore;

namespace BE.Presentation.Policies.Handlers;

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
        if (context.User.Identity.IsAuthenticated == false)
        {
            //https://github.com/dotnet/aspnetcore/issues/4656#issuecomment-605012014
            //apparently if i do "policy.RequireAuthenticatedUser();" in the requirement definition it will allow users that are not authenticated to access the requirement
            // "policy.RequireAuthenticatedUser();" is apparently bugged and doesn't work as intended
            // if i check whether the user is authenticated here and return, it will work as intended and return 401
            return;
        }

        var routeData = _httpContextAccessor.HttpContext.GetRouteData();
        var courseId = routeData.Values["courseId"].ToString();
        // https://stackoverflow.com/a/72832313
        var userCourse = await _dbContext.Courses.Where(c => c.CourseId == courseId).Include(c => c.UserCourses)
            .FirstOrDefaultAsync();
        var userEmail = _httpContextAccessor.HttpContext.User.Identity.Name;
        var currentUser = await _userRepository.GetCurrentUserAsync(userEmail);
        // if the course exists and the user is not signed up for the course, we return and it will point us to the OnForbidden event
        if (userCourse != null && userCourse.UserCourses.Any(x => x.UserId == currentUser.Id) == false)
        {
            return;
        }

        context.Succeed(requirement);
    }
}