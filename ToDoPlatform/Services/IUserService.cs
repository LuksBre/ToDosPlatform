using Microsoft.AspNetCore.Identity;
using ToDoPlatform.ViewModels;

namespace ToDoPlatform.Services;

public interface IUserService
{
    Task<UserVM> GetLoggedUser();
    Task<SignInResult> Login(LoginVM login);
    Task Logout();
    Task<List<string>> Register(RegisterVM register);
}