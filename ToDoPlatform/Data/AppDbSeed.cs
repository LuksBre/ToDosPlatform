using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ToDoPlatform.Models;

namespace ToDoPlatform.Data;

public class AppDbSeed
{
    public AppDbSeed(ModelBuilder builder)
    {
        #region Perfis de usuário (Roles)
        List<IdentityRole> roles = new()
        {
            new IdentityRole()
            {
                Id = "22164d6a-40c4-4ee5-b408-90b41dde37c5",
                Name = "Administrador",
                NormalizedName = "ADMINISTRADOR"
            },
            new IdentityRole()
            {
                Id = "0298aec3-2f2a-479e-acb4-144422a15d7a",
                Name = "Usuário",
                NormalizedName = "USUÁRIO"
            },
        };
        builder.Entity<IdentityRole>().HasData(roles);
        #endregion

        #region Usuários
        List<AppUser> users = new()
        {
            new AppUser()
            {
                Id = "07c08a8c-48a4-4cde-b17f-7fedf5a95c79",
                Email = "admin@todoplatform.com",
                NormalizedEmail = "ADMIN@TODOPLATFORM.COM",
                UserName = "admin@todoplatform.com",
                NormalizedUserName = "ADMIN@TODOPLATFORM.COM",
                LockoutEnabled = false,
                EmailConfirmed = true,
                Name = "Administrator",
                ProfilePicture = "/img/users/default.png"
            },
            new AppUser()
            {
                Id = "41e8ef46-4c13-43cb-9b7e-7222642df441",
                Email = "usuario@todoplatform.com",
                NormalizedEmail = "USUARIO@TODOPLATFORM.COM",
                UserName = "usuario@todoplatform.com",
                NormalizedUserName = "USUARIO@TODOPLATFORM.COM",
                LockoutEnabled = true,
                EmailConfirmed = true,
                Name = "Usuario Teste",
                ProfilePicture = "/img/users/default.png"
            }
        };
        foreach (var user in users)
        {
            PasswordHasher<IdentityUser> pass = new();
            user.PasswordHash = pass.HashPassword(user, "123456");
        }
        builder.Entity<AppUser>().HasData(users);
        #endregion

        #region Atribuição de Perfis
        List<IdentityUserRole<string>> userRoles = new()
        {
            new IdentityUserRole<string>()
            {
                UserId = users[0].Id,
                RoleId = roles[0].Id
            },
            new IdentityUserRole<string>()
            {
                UserId = users[1].Id,
                RoleId = roles[1].Id
            }
        };
        builder.Entity<IdentityUserRole<string>>().HasData(userRoles);
        #endregion

        #region Tarefas
        List<ToDo> toDos = new()
        {
            new ToDo()
            {
                Id = 1,
                UserId = users[1].Id,
                Title = "Estudar .NET",
                Description = "Ler o capítulo 1 do livro",
                Done = false,
                CreatedAt = DateTime.Now
            },
            new ToDo()
            {
                Id = 2,
                UserId = users[1].Id,
                Title = "Fazer exercícios",
                Description = "Resolver os exercícios propostos",
                Done = false,
                CreatedAt = DateTime.Now
            }
        };
        builder.Entity<ToDo>().HasData(toDos);
        #endregion
    }
}