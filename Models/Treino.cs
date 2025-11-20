using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projeto_gerencia_treinos_musculacao.Models
{
    public class Treino
    {
        [Key]
        public string Id { get; set; } // << MUDOU DE INT PARA STRING

        [Required(ErrorMessage = "O nome do treino é obrigatório.")]
        [Display(Name = "Nome do Treino")]
        public string Nome { get; set; }

        // --- Relacionamento com Ficha ---
        public string FichaId { get; set; } // << MUDOU DE INT PARA STRING
        [ForeignKey("FichaId")]
        public virtual Ficha Ficha { get; set; }

        public virtual ICollection<ItemTreino> ItensTreino { get; set; } = new List<ItemTreino>();
    }
}