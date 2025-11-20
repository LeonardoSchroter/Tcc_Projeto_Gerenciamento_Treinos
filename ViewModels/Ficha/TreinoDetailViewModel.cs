using Projeto_gerencia_treinos_musculacao.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Projeto_gerencia_treinos_musculacao.ViewModels
{
    public class TreinoDetailsViewModel
    {
        public string Id { get; set; }
        public string Nome { get; set; }
        public string FichaId { get; set; }

        [Display(Name = "Nome da Ficha")]
        public string FichaNome { get; set; }

        [Display(Name = "Aluno")]
        public string AlunoNome { get; set; }

        public List<ItemTreino> ItensTreino { get; set; } = new List<ItemTreino>();
    }
}