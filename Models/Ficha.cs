using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projeto_gerencia_treinos_musculacao.Models
{
    public class Ficha
    {
        [Key]
        public string Id { get; set; }

        [Required(ErrorMessage = "O nome da ficha é obrigatório.")]
        [Display(Name = "Nome da Ficha")] 
        public string Nome { get; set; }

        [Display(Name = "Objetivo")]
        public string? Objetivo { get; set; }

        [Required(ErrorMessage = "A data de início é obrigatória.")]
        [Display(Name = "Data de Início")]
        public DateTime DataInicio { get; set; }

        [Required(ErrorMessage = "A data final é obrigatória.")]
        [Display(Name = "Data de Fim")]
        public DateTime DataFim { get; set; }

        // --- Chaves Estrangeiras ---
        [Required]
        [Display(Name = "Aluno")]
        public string IdAluno { get; set; }

        [Required]
        [Display(Name = "Personal")]
        public string IdPersonal { get; set; }

        // --- Propriedades de Navegação ---
        [ForeignKey("IdAluno")]
        public virtual Usuario? Aluno { get; set; }

        [ForeignKey("IdPersonal")]
        public virtual Usuario? Personal { get; set; }

        // Uma Ficha agora contém uma coleção de Treinos (A, B, C...)
        public virtual ICollection<Treino> Treinos { get; set; } = new List<Treino>();
    }
}