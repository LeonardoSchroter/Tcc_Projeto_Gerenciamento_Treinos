using Projeto_gerencia_treinos_musculacao.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Projeto_gerencia_treinos_musculacao.ViewModels
{
    public class FichaDetailsViewModel
    {
        public string Id { get; set; }
        public string Nome { get; set; }
        public string? Objetivo { get; set; }

        [Display(Name = "Vigência")]
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }

        [Display(Name = "Aluno")]
        public string AlunoNome { get; set; }

        [Display(Name = "Personal Responsável")]
        public string PersonalNome { get; set; }

        public List<Treino> Treinos { get; set; } = new List<Treino>();
    }
}