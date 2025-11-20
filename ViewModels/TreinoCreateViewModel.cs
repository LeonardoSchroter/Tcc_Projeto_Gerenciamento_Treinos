using Projeto_gerencia_treinos_musculacao.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Projeto_gerencia_treinos_musculacao.ViewModels
{
    public class TreinoEditViewModel
    {
        public string? Id { get; set; } // Usado na edição

        [Required(ErrorMessage = "O nome da divisão de treino é obrigatório.")]
        [Display(Name = "Nome da Divisão (Ex: A - Peito e Tríceceps)")]
        public string Nome { get; set; }

        public string FichaId { get; set; } // Link para a ficha-mãe

        // Lista para receber os exercícios adicionados dinamicamente
        public List<ItemTreinoViewModel> ItensTreino { get; set; } = new List<ItemTreinoViewModel>();

        // Lista para popular o modal de seleção
        public List<Projeto_gerencia_treinos_musculacao.Models.Exercicio> ExerciciosDisponiveis { get; set; } = new List<Projeto_gerencia_treinos_musculacao.Models.Exercicio>();
    }
}