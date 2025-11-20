using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projeto_gerencia_treinos_musculacao.Models
{
    public class ItemTreino
    {
        [Key]
        public string Id { get; set; }


        [Required]
        [Display(Name = "Séries")]
        public string Series { get; set; }

        [Required]
        [Display(Name = "Repetições")]
        public string Repeticoes { get; set; } 

        [Display(Name = "Carga")]
        public string? Carga { get; set; } 

        [Display(Name = "Descanso (segundos)")]
        public string? Descanso { get; set; } 

        [Display(Name = "Ordem")] 
        public int Ordem { get; set; }


        public string TreinoId { get; set; }
        [ForeignKey("TreinoId")]
        public virtual Treino Treino { get; set; }

        public string ExercicioId { get; set; } 
        [ForeignKey("ExercicioId")]
        public virtual Exercicio Exercicio { get; set; }
    }
}