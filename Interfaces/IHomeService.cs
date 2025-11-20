using Projeto_gerencia_treinos_musculacao.ViewModels;
using System.Security.Claims;

namespace Projeto_gerencia_treinos_musculacao.Interfaces
{
    public interface IHomeService
    {
        Task<PersonalDashboardViewModel> GetPersonalDashboardAsync(ClaimsPrincipal user);
        Task<AlunoDashboardViewModel> GetAlunoDashboardAsync(ClaimsPrincipal user);
    }
}