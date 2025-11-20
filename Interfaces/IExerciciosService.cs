using Projeto_gerencia_treinos_musculacao.Models;
using Projeto_gerencia_treinos_musculacao.ViewModels.Exercicio;
using System.Security.Claims;

namespace Projeto_gerencia_treinos_musculacao.Interfaces
{
    public interface IExerciciosService
    {
        Task<List<Exercicio>> GetExerciciosAsync(ClaimsPrincipal user);
        Task<Exercicio> GetExercicioDetailsAsync(string id);
        ExercicioEditViewModel BuildCreateViewModel();
        Task CreateExercicioAsync(ExercicioCreateViewModel viewModel, ClaimsPrincipal user);
        Task<ExercicioEditViewModel> BuildEditViewModelAsync(string id);
        Task<bool> UpdateExercicioAsync(string id, ExercicioEditViewModel viewModel);
        Task DeleteExercicioAsync(string id);
        Task<bool> ExercicioExistsAsync(string id);
    }
}