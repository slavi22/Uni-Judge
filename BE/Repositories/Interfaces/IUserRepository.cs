using BE.Models.Auth;

namespace BE.Repositories.Interfaces;

public interface IUserRepository
{
    Task<AppUser> GetCurrentUserAsync();
    Task<AppUser> FindByNameAsync(string email);
    Task<int> GetUserCountAsync();
    Task<bool> CreateAsync(AppUser user, string password);
    Task<bool> PasswordSignInAsync(string email, string password, bool isPersistent, bool lockoutOnFailure);
    Task AddToRoleAsync(AppUser user, string role);
    Task AddRolesAsync(AppUser user, IEnumerable<string> roles);
    Task UpdateAsync(AppUser user);
    Task<IList<string>>GetRolesAsync(AppUser user);
}