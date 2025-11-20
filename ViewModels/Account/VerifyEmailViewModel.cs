using System.ComponentModel.DataAnnotations;

namespace Projeto_gerencia_treinos_musculacao.ViewModels.Account
{
    public class VerifyEmailViewModel
    {
        [Required(ErrorMessage = "Email é necessário.")]
        [EmailAddress]
        public string Email { get; set; }
    }
}
