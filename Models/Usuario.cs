using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projeto_gerencia_treinos_musculacao.Models
{
    // Sua classe agora herda diretamente de IdentityUser.
    public class Usuario : IdentityUser
    {
        [Display(Name = "Nome Completo")]
        public string? Nome { get; set; }

        [Display(Name = "Papel")]
        public string? Role { get; set; } 

        [Display(Name = "Gênero")]
        public string? Sexo { get; set; }

        [Display(Name = "Endereço")]
        public string? Endereco { get; set; }

        [Display(Name = "Data de Nascimento")]
        public DateTime? DataNascimento { get; set; }

        // --- Adicionando a auto-referência para Personal ---

        [Display(Name = "Personal")]
        [ForeignKey("Personal")]
        public string? PersonalId { get; set; }

        //public virtual Usuario? Personal { get; set; }

        // --- Fim da auto-referência ---

        // Propriedades de navegação para os relacionamentos
        public virtual Usuario? Personal { get; set; }
        public virtual ICollection<Anamnese>? AnamnesesCriadas { get; set; }// anamneses criadas pelo personal
        public virtual ICollection<Anamnese>? Anamneses { get; set; }
        public virtual ICollection<Ficha>? FichasCriadas { get; set; } // Criadas pelo Personal
        public virtual ICollection<Ficha>? FichasPossuidas { get; set; } // Possuídas pelo Aluno
        public virtual ICollection<Usuario>? Alunos { get; set; }


    }
}