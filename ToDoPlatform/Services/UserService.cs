using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ToDoPlatform.Data;
using ToDoPlatform.Models;
using ToDoPlatform.ViewModels;

namespace ToDoPlatform.Services;

public class UserService : IUserService
{
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;
    private readonly ILogger<UserService> _logger;
    private readonly AppDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserService(
        AppDbContext dbContext,
        IHttpContextAccessor httpContextAccessor,
        SignInManager<AppUser> signInManager,
        UserManager<AppUser> userManager,
        ILogger<UserService> logger)
    {
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
        _signInManager = signInManager;
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<UserVM> GetLoggedUser()
    {
        var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return null;

        var user = await _dbContext.AppUsers.SingleOrDefaultAsync(u => u.Id == userId);
        if (user == null) return null;
        
        var roles = string.Join(",", await _userManager.GetRolesAsync(user));
        var isAdmin = await _userManager.IsInRoleAsync(user, "Administrador");

        return new UserVM()
        {
            Id = userId,
            Name = user.Name,
            ProfilePicture = user.ProfilePicture ?? "/img/users/default.png",
            Email = user.Email,
            UserName = user.UserName,
            Roles = roles,
            IsAdmin = isAdmin
        };
    }

    public async Task<SignInResult> Login(LoginVM login)
    {
        string userName = login.Email;
        var user = await _userManager.FindByEmailAsync(login.Email);
        if (user != null) userName = user.UserName;

        var result = await _signInManager.PasswordSignInAsync(
            userName, login.Password, login.RememberMe, lockoutOnFailure: true);

        if (result.Succeeded)
            _logger.LogInformation($"Usuário '{userName}' acessou o sistema");
        if (result.IsLockedOut)
            _logger.LogWarning($"Usuário '{userName}' está bloqueado");

        return result;
    }

    public async Task Logout()
    {
        _logger.LogInformation($"Usuário saiu do sistema");
        await _signInManager.SignOutAsync();
    }

    public async Task<List<string>> Register(RegisterVM register)
    {
        var user = new AppUser()
        {
            Name = register.Name,
            UserName = register.Email,
            NormalizedUserName = register.Email.ToUpper(),
            Email = register.Email,
            NormalizedEmail = register.Email.ToUpper(),
            EmailConfirmed = true,
            LockoutEnabled = true,
            ProfilePicture = "/img/users/default.png"  // 🔧 CORREÇÃO AQUI
        };

        var addUser = await _userManager.CreateAsync(user, register.Password);
        List<string> result = [];

        if (addUser.Succeeded)
        {
            _logger.LogInformation($"Novo usuário registrado: {register.Email}");
            await _userManager.AddToRoleAsync(user, "Usuário");
        }
        else
        {
            foreach (var error in addUser.Errors)
                result.Add(TranslateIdentityErrors.TranslateErrorMessage(error.Code));
        }
        return result;
    }
}