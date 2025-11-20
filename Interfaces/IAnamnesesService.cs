using Microsoft.AspNetCore.Mvc.Rendering;
using Projeto_gerencia_treinos_musculacao.Models;
using Projeto_gerencia_treinos_musculacao.ViewModels;
using System.Security.Claims;

namespace Projeto_gerencia_treinos_musculacao.Interfaces
{
    public interface IAnamnesesService
    {
        Task<List<Anamnese>> GetAnamnesesAsync(ClaimsPrincipal user);
        Task<Anamnese> GetAnamneseDetailsAsync(string id);
        Task<AnamneseViewModel> BuildCreateViewModelAsync(ClaimsPrincipal user);
        Task CreateAnamneseAsync(AnamneseViewModel viewModel, ClaimsPrincipal user);
        Task<AnamneseViewModel> BuildEditViewModelAsync(string id);
        Task<bool> UpdateAnamneseAsync(string id, AnamneseViewModel viewModel);
        Task<Anamnese> GetAnamneseForDeleteAsync(string id);
        Task DeleteAnamneseAsync(string id);
        Task<SelectList> GetTodosUsuariosSelectListAsync(string selectedId = null);
    }
}