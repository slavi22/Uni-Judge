using BE.DataAccess.Data;
using BE.DataAccess.Repositories.Interfaces;
using BE.Models.Models.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BE.DataAccess.Repositories.Implementations;

public class UserRepository : IUserRepository
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly AppDbContext _dbContext;

    public UserRepository(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IHttpContextAccessor httpContextAccessor, AppDbContext dbContext)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _httpContextAccessor = httpContextAccessor;
        _dbContext = dbContext;
    }

    public async Task<AppUser> GetCurrentUserAsync()
    {
        return await _userManager.FindByEmailAsync(_httpContextAccessor.HttpContext.User.Identity.Name);
    }

    public async Task<AppUser> FindByNameAsync(string email)
    {
        return await _userManager.FindByNameAsync(email);
    }

    public async Task<int> GetUserCountAsync()
    {
        return await _userManager.Users.CountAsync();
    }

    public async Task<IdentityResult> CreateAsync(AppUser user, string password)
    {
        // if the password is null, we are creating a passwordless user (we are going through an auth provider)
        if (password == null)
        {
            var passwordlessResult = await _userManager.CreateAsync(user);
            return passwordlessResult;
        }
        var result = await _userManager.CreateAsync(user, password);
        return result;
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

    public async Task<IList<string>> GetRolesAsync(AppUser user)
    {
        var result = await _userManager.GetRolesAsync(user);
        return result;
    }

    public async Task<AppUser> GetUserByRefreshToken(string refreshToken)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(user => user.RefreshToken == refreshToken);
        return user;
    }
}