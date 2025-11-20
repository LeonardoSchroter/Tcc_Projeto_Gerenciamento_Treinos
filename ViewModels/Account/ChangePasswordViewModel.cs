using System.ComponentModel.DataAnnotations;

namespace Projeto_gerencia_treinos_musculacao.ViewModels.Account
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Email é necessário.")]
        [EmailAddress]
        public string Email {  get; set; }

        [Required(ErrorMessage = "Nova Senha é necessária")]
        [StringLength(40, MinimumLength = 8, ErrorMessage = "A senha deve conter 8 ou mais caracteres")]
        [DataType(DataType.Password)]
        [Compare("ConfirmNewPassword", ErrorMessage = "As senhas estão diferentes")]
        [Display(Name = "Nova senha")]
        public string NewPassword {  get; set; }

        [Required(ErrorMessage = "Confirmar senha é necessário")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar nova senha")]
        public string ConfirmNewPassword { get; set; }
    }
}
