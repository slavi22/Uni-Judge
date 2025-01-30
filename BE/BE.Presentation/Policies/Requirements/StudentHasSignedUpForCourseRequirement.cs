using Microsoft.AspNetCore.Authorization;

namespace BE.Presentation.Policies.Requirements;

public class StudentHasSignedUpForCourseRequirement : IAuthorizationRequirement;