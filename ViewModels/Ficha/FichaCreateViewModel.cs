using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Projeto_gerencia_treinos_musculacao.ViewModels
{
    public class FichaCreateViewModel
    {
        [Required(ErrorMessage = "O nome da ficha é obrigatório.")]
        [Display(Name = "Nome da Ficha")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "É obrigatório selecionar um aluno.")]
        [Display(Name = "Aluno")]
        public string IdAluno { get; set; }

        [Required(ErrorMessage = "A data de início é obrigatória.")]
        [Display(Name = "Data de Início")]
        [DataType(DataType.Date)]
        public DateTime DataInicio { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "A data final é obrigatória.")]
        [Display(Name = "Data de Fim")]
        [DataType(DataType.Date)]
        public DateTime DataFim { get; set; } = DateTime.Today.AddDays(30);

        [Display(Name = "Objetivo")]
        public string? Objetivo { get; set; }

        // Propriedade para preencher o dropdown de alunos
        public SelectList? Alunos { get; set; }
    }
}