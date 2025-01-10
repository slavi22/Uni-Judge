using BE.Models.Submissions;

namespace BE.Repositories.Interfaces;

public interface IUserSubmissionRepository
{
    public Task AddAsync(UserSubmissionModel submissionModel);
}