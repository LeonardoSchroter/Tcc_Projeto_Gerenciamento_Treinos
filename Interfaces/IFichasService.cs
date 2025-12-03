using Microsoft.AspNetCore.Mvc.Rendering;
using Projeto_gerencia_treinos_musculacao.Models;
using Projeto_gerencia_treinos_musculacao.ViewModels;
using System.Security.Claims;

namespace Projeto_gerencia_treinos_musculacao.Interfaces
{
    public interface IFichasService
    {
        Task<List<Ficha>> GetFichasAsync(ClaimsPrincipal user);
        Task<FichaDetailsViewModel> GetFichaDetailsAsync(string id);
        Task<FichaCreateViewModel> BuildCreateViewModelAsync(string alunoId, ClaimsPrincipal user);
        Task<string> CreateFichaAsync(FichaCreateViewModel viewModel, ClaimsPrincipal user);
        Task<FichaEditViewModel> BuildEditViewModelAsync(string id);
        Task<bool> UpdateFichaAsync(string id, FichaEditViewModel viewModel);
        Task<Ficha> GetFichaForDeleteAsync(string id);
        Task DeleteFichaAsync(string id);
        Task<(bool Success, string Message)> GerarTreinosComIAAsync(string fichaId, string instrucoes, ClaimsPrincipal user);
        Task<SelectList> GetAlunosSelectListAsync(string selectedId = null);
    }
}