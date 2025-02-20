using BE.Models.Models.Problem;

namespace BE.DataAccess.Repositories.Interfaces;

public interface IMainMethodBodiesRepository
{
    /// <summary>
    /// Retrieves the main method body by its problem ID.
    /// </summary>
    /// <param name="problemId">The ID of the problem to retrieve the main method body for</param>
    /// <param name="languageId">The ID of the language used to retrieve the respective main method body</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the main method body model</returns>
    Task<MainMethodBodyModel> GetMainMethodBodyByIdAsync(string problemId, string languageId);
}