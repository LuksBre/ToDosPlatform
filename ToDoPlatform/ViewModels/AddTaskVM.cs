using System.ComponentModel.DataAnnotations;

namespace ToDoPlatform.ViewModels;

public class AddTaskVM
{
    [StringLength(100)]
    [Required(ErrorMessage = "O Título é obrigatório")]
    [Display(Name = "Título da Tarefa", Prompt = "Digite o título da tarefa")]
    public string Title { get; set; }

    [Display(Name = "Descrição", Prompt = "Descreva os detalhes da tarefa")]
    public string Description { get; set; }
}