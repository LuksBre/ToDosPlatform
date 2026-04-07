using System.ComponentModel.DataAnnotations;

namespace ToDoPlatform.ViewModels;

public class RegisterVM
{
    [Display(Name = "Nome Completo", Prompt = "Informe seu Nome Completo")]
    [Required(ErrorMessage = "O Nome é obrigatório")]
    [StringLength(50, ErrorMessage = "O Nome deve possuir no máximo 50 caracteres")]
    public string Name { get; set; }

    [Display(Name = "E-mail", Prompt = "seu@email.com")]
    [Required(ErrorMessage = "O e-mail de acesso é obrigatório!")]
    [EmailAddress(ErrorMessage = "Informe um e-mail válido!")]
    [StringLength(100, ErrorMessage = "O E-mail deve possuir no máximo 100 caracteres")]
    public string Email { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Senha de Acesso", Prompt = "Informe uma Senha para Acesso")]
    [Required(ErrorMessage = "Por favor, informe sua Senha de Acesso")]
    [StringLength(50, MinimumLength = 6, ErrorMessage = "A Senha deve possuir no mínimo 6 e no máximo 50 caracteres")]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Confirmar Senha de Acesso", Prompt = "Confirmar sua Senha de Acesso")]
    [Required(ErrorMessage = "Por favor, informe a Confirmação de Senha")]
    [Compare("Password", ErrorMessage = "As Senhas não Conferem.")]
    public string ConfirmPassword { get; set; }

    public bool Terms { get; set; }
}