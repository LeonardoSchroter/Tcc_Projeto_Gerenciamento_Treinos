using System.ComponentModel.DataAnnotations;

namespace Projeto_gerencia_treinos_musculacao.ViewModels.Account
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Nome é necessário.")]
        [Display(Name = "Nome")]
        public string Name {  get; set; }

        [Required(ErrorMessage = "Email é necessário.")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Telefone/Celular não preenchido")]
        [Phone]
        public string Telefone { get; set; }

        [Required(ErrorMessage = "Senha é necessária")]
        [StringLength(40,MinimumLength =8,ErrorMessage ="A senha deve conter 8 ou mais caracteres")]
        [DataType(DataType.Password)]
        [Compare("ConfirmPassword",ErrorMessage ="As senhas estão diferentes")]
        [Display(Name = "Senha")]
        public string Password {  get; set; }

        [Required(ErrorMessage ="É necessário confirmar a senha.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar a senha")]
        public string ConfirmPassword {  get; set; }
    }
}
