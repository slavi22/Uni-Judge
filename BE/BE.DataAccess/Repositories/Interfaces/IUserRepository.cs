using BE.Models.Models.Auth;

namespace BE.DataAccess.Repositories.Interfaces;

public interface IUserRepository
{
    /// <summary>
    /// Retrieves the current user.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains the current user</returns>
    Task<AppUser> GetCurrentUserAsync();

    /// <summary>
    /// Finds a user by their email. Currently, our user's email is also their username.
    /// </summary>
    /// <param name="email">The email of the user to find</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the user if found</returns>
    Task<AppUser> FindByNameAsync(string email);

    /// <summary>
    /// Gets the total count of users.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains the total count of users</returns>
    Task<int> GetUserCountAsync();

    /// <summary>
    /// Creates a new user with the specified password.
    /// </summary>
    /// <param name="user">The user to create</param>
    /// <param name="password">The password for the new user</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether the creation was successful</returns>
    Task<bool> CreateAsync(AppUser user, string password);

    /// <summary>
    /// Signs in a user with the specified email and password.
    /// </summary>
    /// <param name="email">The email of the user</param>
    /// <param name="password">The password of the user</param>
    /// <param name="isPersistent">Whether the sign-in should be persistent</param>
    /// <param name="lockoutOnFailure">Whether to lock out the user on failure</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether the sign-in was successful</returns>
    Task<bool> PasswordSignInAsync(string email, string password, bool isPersistent, bool lockoutOnFailure);

    /// <summary>
    /// Adds the specified user to a role.
    /// </summary>
    /// <param name="user">The user to add to the role</param>
    /// <param name="role">The role to add the user to</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task AddToRoleAsync(AppUser user, string role);

    /// <summary>
    /// Adds the specified user to multiple roles.
    /// </summary>
    /// <param name="user">The user to add to the roles</param>
    /// <param name="roles">The roles to add the user to</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task AddRolesAsync(AppUser user, IEnumerable<string> roles);

    /// <summary>
    /// Updates the specified user.
    /// </summary>
    /// <param name="user">The user to update</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task UpdateAsync(AppUser user);

    /// <summary>
    /// Gets the roles of the specified user.
    /// </summary>
    /// <param name="user">The user to get the roles for</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of roles</returns>
    Task<IList<string>> GetRolesAsync(AppUser user);
}