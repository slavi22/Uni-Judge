using BE.Models.Problem;

namespace BE.Repositories.Interfaces;

public interface IMainMethodBodiesRepository
{
    Task<MainMethodBodyModel> GetMainMethodBodyByIdAsync(string problemId);
}