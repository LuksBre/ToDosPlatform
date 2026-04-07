using Microsoft.AspNetCore.Mvc;
using ToDoPlatform.Services;
using ToDoPlatform.ViewModels;

namespace ToDoPlatform.Controllers;

public class AccountController : Controller
{
    private readonly ILogger<AccountController> _logger;
    private readonly IUserService _userService;

    public AccountController(ILogger<AccountController> logger, IUserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    [HttpGet]
    public IActionResult Login(string returnUrl)
    {
        LoginVM loginVM = new() { ReturnUrl = returnUrl ?? Url.Content("~/") };
        return View(loginVM);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginVM loginVM)
    {
        if (ModelState.IsValid)
        {
            var result = await _userService.Login(loginVM);
            if (result.Succeeded)
                TempData["Success"] = "Login realizado com sucesso! Redirecionando...";
            else if (result.IsLockedOut)
                TempData["Failure"] = "Usuário bloqueado por muitas tentativas.";
            else if (result.IsNotAllowed)
                TempData["Failure"] = "Usuário sem permissão para acessar o sistema.";
            else
                TempData["Failure"] = "E-mail ou senha incorretos. Tente novamente.";
        }
        else
            TempData["Failure"] = "Dados inválidos. Verifique os campos preenchidos.";

        return View(loginVM);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _userService.Logout();
        return RedirectToAction("Login", "Account");
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterVM registerVM)
    {
        if (ModelState.IsValid && registerVM.Terms)
        {
            var result = await _userService.Register(registerVM);
            if (result.Count == 0)
                TempData["Success"] = "Conta criada com sucesso! Redirecionando...";
            else
            {
                foreach (var error in result)
                    TempData["Failure"] += error + "\n";
            }
        }
        else
            TempData["Failure"] = "Dados inválidos. Verifique os campos preenchidos.";

        return View(registerVM);
    }

    public IActionResult Profile() => View();
}