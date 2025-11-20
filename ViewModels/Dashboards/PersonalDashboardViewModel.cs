using Projeto_gerencia_treinos_musculacao.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Projeto_gerencia_treinos_musculacao.ViewModels
{
    public class PersonalDashboardViewModel
    {
        [Display(Name = "Total de Alunos Ativos")]
        public int TotalAlunos { get; set; }

        [Display(Name = "Fichas Ativas")]
        public int FichasAtivas { get; set; }

        [Display(Name = "Fichas a Expirar")]
        public int FichasExpirandoCount { get; set; }

        [Display(Name = "Alunos Sem Ficha")]
        public int AlunosSemFichaCount { get; set; }

        public List<Ficha> FichasExpirandoList { get; set; } = new List<Ficha>();

        public List<Usuario> AlunosSemFichaList { get; set; } = new List<Usuario>();
    }
}