using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace ToDoPlatform.ViewModels;

public class LoginVM
{
    [Display(Name = "E-mail", Prompt = "seu@email.com")]
    [Required(ErrorMessage = "O e-mail de acesso é obrigatório!")]
    [EmailAddress(ErrorMessage = "Informe um e-mail válido!")]
    public string Email { get; set; }

    [DataType(DataType.Password)]
    [Required(ErrorMessage = "A senha de acesso é obrigatória")]
    [Display(Name = "Senha", Prompt = "**********")]
    public string Password { get; set; }

    [Display(Name = "Manter Conectado?")]
    public bool RememberMe { get; set; }

    [HiddenInput]
    public string ReturnUrl { get; set; }
}