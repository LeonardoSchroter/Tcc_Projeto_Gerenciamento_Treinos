using Projeto_gerencia_treinos_musculacao.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projeto_gerencia_treinos_musculacao.Models
{
    public class Anamnese
    {
        [Key]
        public string Id { get; set; }

        [Display(Name = "ID do Usuário")]
        [ForeignKey("Usuario")]
        public string UsuarioId { get; set; }

        [Display(Name = "Data de Registro")]
        public DateTime? DataRegistro { get; set; }

        [Display(Name = "Doenças")]
        public string? Doencas { get; set; }

        [Display(Name = "Hábitos Alimentares")]
        public string? HabitosAlimentares { get; set; }

        [Display(Name = "Consumo de Álcool")]
        public bool ConsumoAlcool { get; set; }

        [Display(Name = "Tabagismo")]
        public bool Tabagismo { get; set; }

        [Display(Name = "Objetivos")]
        public string? Objetivos { get; set; }

        [Display(Name = "Atividade Física Atual")]
        public string? AtividadeFisicaAtual { get; set; }

        [Display(Name = "Intensidade da Atividade Física")]
        public string? IntensidadeAtividadeFisica { get; set; }

        [Display(Name = "Observações")]
        public string? Observacoes { get; set; }

        [Display(Name = "Lesões")]
        public string? Lesoes { get; set; }

        [Display(Name = "Cirurgias")]
        public string? Cirurgias { get; set; }

        [Display(Name = "Medicamentos")]
        public string? Medicamentos { get; set; }

        [Display(Name = "Peso (kg)")]
        public float? Peso { get; set; }

        [Display(Name = "Altura (cm)")]
        public int? Altura { get; set; }

        // Propriedade de navegação para o relacionamento 1 para 1
        public virtual Usuario? Usuario { get; set; }

        [Display(Name = "Personal Responsável")]
        [ForeignKey("Personal")]
        public string? PersonalId { get; set; } // Chave estrangeira para o Personal

        public virtual Usuario? Personal { get; set; } // Propriedade de navegação para o Personal
    }
}