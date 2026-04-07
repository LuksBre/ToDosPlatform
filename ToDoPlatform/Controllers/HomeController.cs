using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoPlatform.Data;
using ToDoPlatform.Models;
using ToDoPlatform.Services;
using ToDoPlatform.ViewModels;

namespace ToDoPlatform.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly AppDbContext _dbContext;
    private readonly IUserService _userService;

    public HomeController(AppDbContext dbContext, IUserService userService)
    {
        _dbContext = dbContext;
        _userService = userService;
    }

    public async Task<IActionResult> Index()
    {
        var user = await _userService.GetLoggedUser();

        var openTodos = await _dbContext.ToDos
            .Where(t => t.UserId == user.Id && !t.Done)
            .OrderByDescending(t => t.CreatedAt)
            .ThenBy(t => t.Title)
            .ToListAsync();

        var totalTasks = await _dbContext.ToDos.CountAsync(t => t.UserId == user.Id);
        var endedTasks = await _dbContext.ToDos.CountAsync(t => t.UserId == user.Id && t.Done);

        HomeVM homeVM = new()
        {
            TotalTasks = totalTasks,
            OpenTasks = openTodos.Count,
            EndedTasks = endedTasks,
            ToDos = openTodos
        };
        return View(homeVM);
    }

    [HttpPost]
    public async Task<IActionResult> EndTask(int? id)
    {
        if (id == null)
        {
            return Json(new
            {
                success = false,
                message = "Problemas ao carregar a tarefa! Tente novamente mais tarde..."
            });
        }

        var todo = await _dbContext.ToDos.Where(t => t.Id == id).SingleOrDefaultAsync();
        if (todo == null)
        {
            return Json(new
            {
                success = false,
                message = "Não foi possível localizar a tarefa! Tente novamente mais tarde..."
            });
        }

        var user = await _userService.GetLoggedUser();
        if (user == null)
        {
            return Json(new
            {
                success = false,
                message = "Sua sessão expirou! Faça login novamente para continuar!",
                redirect = true
            });
        }

        if (todo.UserId != user.Id)
        {
            return Json(new
            {
                success = false,
                message = "Você não tem permissão para alterar esta tarefa!"
            });
        }

        try
        {
            todo.Done = true;
            _dbContext.ToDos.Update(todo);
            _dbContext.SaveChanges();
            return Json(new
            {
                success = true,
                message = "Tarefa finalizada com sucesso! Recarregando Lista..."
            });
        }
        catch (Exception ex)
        {
            return Json(new
            {
                success = false,
                message = "Ocorreu um problema ao finalizar a tarefa! Tente novamente mais tarde...",
                details = ex.Message
            });
        }
    }

    public IActionResult AddTask()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddTask(AddTaskVM addTask)
    {
        if (ModelState.IsValid)
        {
            var user = await _userService.GetLoggedUser();
            if (user == null)
                TempData["Failure"] = "Sua sessão expirou, faça login novamente!";
            else
            {
                ToDo toDo = new()
                {
                    Title = addTask.Title,
                    Description = addTask.Description,
                    UserId = user.Id
                };
                await _dbContext.ToDos.AddAsync(toDo);
                await _dbContext.SaveChangesAsync();
                TempData["Success"] = "Tarefa criada com sucesso! Redirecionando...";
            }
        }
        return View(addTask);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}