using System.ComponentModel.DataAnnotations;
using static Projeto_gerencia_treinos_musculacao.Models.Enum;

namespace Projeto_gerencia_treinos_musculacao.ViewModels.Exercicio
{
    public class ExercicioCreateViewModel
    {

        [Required(ErrorMessage = "O nome do exercício é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo {1} caracteres.")]
        [Display(Name = "Nome do Exercício")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "A execução do exercício é obrigatória.")]
        [Display(Name = "Instruções de Execução")]
        [DataType(DataType.MultilineText)]
        public string Execucao { get; set; }

        [Required(ErrorMessage = "O grupo muscular é obrigatório.")]
        [Display(Name = "Grupo Muscular")]
        public GrupoMuscular GrupoMuscular { get; set; } // Continua como Enum

        [Required(ErrorMessage = "O equipamento é obrigatório.")]
        [Display(Name = "Equipamento Necessário")]
        // Equipamento agora é string, como você pediu.
        public string Equipamento { get; set; }
    }
}
