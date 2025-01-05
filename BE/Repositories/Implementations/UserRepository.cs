using BE.Models.Auth;
using BE.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BE.Repositories.Implementations;

public class UserRepository : IUserRepository
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;

    public UserRepository(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<AppUser> FindByNameAsync(string email)
    {
        return await _userManager.FindByNameAsync(email);
    }

    public async Task<int> GetUserCountAsync()
    {
        return await _userManager.Users.CountAsync();
    }

    public async Task<bool> CreateAsync(AppUser user, string password)
    {
        var result = await _userManager.CreateAsync(user, password);
        return result.Succeeded;
    }

    public async Task<bool> PasswordSignInAsync(string email, string password, bool isPersistent, bool lockoutOnFailure)
    {
        var result = await _signInManager.PasswordSignInAsync(email, password, false, false);
        return result.Succeeded;
    }

    public async Task AddToRoleAsync(AppUser user, string role)
    {
        await _userManager.AddToRoleAsync(user, role);
    }

    public async Task AddRolesAsync(AppUser user, IEnumerable<string> roles)
    {
        await _userManager.AddToRolesAsync(user, roles);
    }

    public async Task UpdateAsync(AppUser user)
    {
        await _userManager.UpdateAsync(user);
    }

    public async Task<IList<string>>  GetRolesAsync(AppUser user)
    {
        var result = await _userManager.GetRolesAsync(user);
        return result;
    }
}