using BE.DTOs.Judge;

namespace BE.Repositories;

public interface IJudgeService
{
    Task AddBatchSubmissions(SubmissionBatchDto submissions);
}