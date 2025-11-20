using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using static Projeto_gerencia_treinos_musculacao.Models.Enum;

namespace Projeto_gerencia_treinos_musculacao.ViewModels
{
    public class AnamneseViewModel
    {
        public string? Id { get; set; }

        [Required(ErrorMessage = "É obrigatório selecionar um usuário.")]
        [Display(Name = "Aluno")]
        public string? UsuarioId { get; set; }

        [Required(ErrorMessage = "A data de registro é obrigatória.")]
        [Display(Name = "Data de Registro")]
        [DataType(DataType.Date)] 
        public DateTime DataRegistro { get; set; }

        [Display(Name = "Doenças Crônicas ou Pré-existentes")]
        public string? Doencas { get; set; }

        [Display(Name = "Hábitos Alimentares")]
        public string? HabitosAlimentares { get; set; }

        [Display(Name = "Consumo de Álcool")]
        public bool ConsumoAlcool { get; set; }

        [Display(Name = "Tabagismo")]
        public bool Tabagismo { get; set; }

        [Required(ErrorMessage = "O campo Objetivos é obrigatório.")]
        [Display(Name = "Objetivos")]
        public string? Objetivos { get; set; }

        [Display(Name = "Atividade Física Atual")]
        public string? AtividadeFisicaAtual { get; set; }

        [Display(Name = "Intensidade da Atividade Física")]
        public NivelIntensidade? IntensidadeAtividadeFisica { get; set; }

        [Display(Name = "Observações")]
        public string? Observacoes { get; set; }

        [Display(Name = "Lesões")]
        public string? Lesoes { get; set; }

        [Display(Name = "Cirurgias")]
        public string? Cirurgias { get; set; }

        [Display(Name = "Medicamentos em Uso")]
        public string? Medicamentos { get; set; }

        [Required(ErrorMessage = "O peso é obrigatório.")]
        [Display(Name = "Peso (kg)")]
        [Range(1, 400, ErrorMessage = "O peso deve ser um valor válido.")]
        public float? Peso { get; set; }

        [Required(ErrorMessage = "A altura é obrigatória.")]
        [Display(Name = "Altura (cm)")]
        [Range(1, 300, ErrorMessage = "A altura deve ser um valor válido.")]
        public int? Altura { get; set; }

        public SelectList? Usuarios { get; set; }
    }
}