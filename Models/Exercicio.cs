using Projeto_gerencia_treinos_musculacao.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projeto_gerencia_treinos_musculacao.Models
{
    public class Exercicio
    {
        [Key]
        public string Id { get; set; }

        [Display(Name = "Usuário Criador")]
        [ForeignKey("UsuarioCriador")]
        public string? IdUsuario { get; set; }

        [Display(Name = "Nome")]
        public string? Nome { get; set; }

        [Display(Name = "Execução")]
        public string? Execucao { get; set; }

        [Display(Name = "Grupo Muscular")]
        public string? GrupoMuscular { get; set; }

        [Display(Name = "Equipamento")]
        public string? Equipamento { get; set; }

        // Propriedade de navegação
        public virtual ICollection<ItemTreino>? ItemTreino { get; set; }
        public virtual Usuario? UsuarioCriador { get; set; }
    }
}