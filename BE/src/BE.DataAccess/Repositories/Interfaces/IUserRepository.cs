﻿using BE.Models.Models.Auth;
using Microsoft.AspNetCore.Identity;

namespace BE.DataAccess.Repositories.Interfaces;

public interface IUserRepository
{
    /// <summary>
    /// Retrieves a user by their email address.
    /// </summary>
    /// <param name="email">The email address of the user to retrieve</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the user if found</returns>
    Task<AppUser> GetCurrentUserAsync(string email);

    /// <summary>
    /// Finds a user by their email. Currently, the user's email is also their username.
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
    /// <returns>A task that represents the asynchronous operation. The task result contains an IdentityResult indicating whether the user creation was successful</returns>
    Task<IdentityResult> CreateAsync(AppUser user, string password);

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

    /// <summary>
    /// Retrieves a user by their refresh token.
    /// </summary>
    /// <param name="refreshToken">The refresh token obtained from the http-only "refreshToken" cookie in the request</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the user associated with the refresh token, if found</returns>
    Task<AppUser> GetUserByRefreshToken(string refreshToken);
}