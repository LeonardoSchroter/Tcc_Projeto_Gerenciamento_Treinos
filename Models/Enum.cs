using System.ComponentModel.DataAnnotations;

namespace Projeto_gerencia_treinos_musculacao.Models
{
    public class Enum
    {
        public enum GrupoMuscular
        {
            Peito,
            Costas,
            Ombros,
            Biceps,
            Triceps,
            Pernas,
            Gluteos,
            Abdomen,
            Panturrilha
        }

        public enum NivelIntensidade
        {
            [Display(Name = "Baixa")]
            Baixa,
            [Display(Name = "Moderada")]
            Moderada,
            [Display(Name = "Alta")]
            Alta
        }
    }
}
