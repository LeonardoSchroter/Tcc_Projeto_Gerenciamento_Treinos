using System.ComponentModel.DataAnnotations;

namespace Projeto_gerencia_treinos_musculacao.ViewModels.Account
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="Email é necessário.")]
        [EmailAddress]
        public string Email {  get; set; }

        [Required(ErrorMessage = "Senha é necessária")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]

        public string Password { get; set; }

        [Display(Name = "Lembrar-se")]
        public bool RememberMe { get; set; }
    }
}
