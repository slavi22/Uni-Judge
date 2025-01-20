using Microsoft.AspNetCore.Authorization;

namespace BE.Policies.Requirements;

public class StudentHasSignedUpForCourseRequirement : IAuthorizationRequirement;