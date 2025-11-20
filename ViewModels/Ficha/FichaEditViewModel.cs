// Crie este novo arquivo na pasta ViewModels
using Projeto_gerencia_treinos_musculacao.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Projeto_gerencia_treinos_musculacao.ViewModels
{
    public class FichaEditViewModel
    {
        // Campo oculto para saber qual ficha editar
        public string Id { get; set; }

        [Required(ErrorMessage = "O nome da ficha é obrigatório.")]
        [Display(Name = "Nome da Ficha")]
        public string Nome { get; set; }

        // Na edição, o aluno não pode ser alterado, será apenas exibido
        [Display(Name = "Aluno")]
        public string NomeAluno { get; set; }

        [Required]
        [Display(Name = "Data de Início")]
        [DataType(DataType.Date)]
        public DateTime DataInicio { get; set; }

        [Required]
        [Display(Name = "Data de Fim")]
        [DataType(DataType.Date)]
        public DateTime DataFim { get; set; }

        [Display(Name = "Objetivo")]
        public string? Objetivo { get; set; }

        // Lista para exibir os treinos já criados para esta ficha
        public List<Treino> Treinos { get; set; } = new List<Treino>();
    }
}