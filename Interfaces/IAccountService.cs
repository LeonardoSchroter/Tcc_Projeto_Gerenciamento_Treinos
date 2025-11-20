using Microsoft.AspNetCore.Identity;
using Projeto_gerencia_treinos_musculacao.Models;
using Projeto_gerencia_treinos_musculacao.ViewModels.Account;

namespace Projeto_gerencia_treinos_musculacao.Interfaces
{
    public interface IAccountService
    {
        Task<SignInResult> LoginAsync(LoginViewModel model);
        Task<IdentityResult> RegisterAsync(RegisterViewModel model);
        Task<Usuario> VerifyEmailAsync(string email);
        Task LogoutAsync();
    }
}