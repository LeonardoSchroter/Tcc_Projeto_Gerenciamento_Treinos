using Projeto_gerencia_treinos_musculacao.Models;
using Projeto_gerencia_treinos_musculacao.ViewModels;

namespace Projeto_gerencia_treinos_musculacao.Interfaces
{
    public interface ITreinosService
    {
        Task<TreinoEditViewModel> BuildCreateViewModelAsync(string fichaId);
        Task CreateTreinoAsync(TreinoEditViewModel viewModel);
        Task<TreinoEditViewModel> BuildEditViewModelAsync(string id);
        Task<bool> UpdateTreinoAsync(string id, TreinoEditViewModel viewModel);
        Task<Treino> GetTreinoForDeleteAsync(string id);
        Task<string> DeleteTreinoAsync(string id);
        Task<List<Exercicio>> GetExerciciosDisponiveisAsync();
    }
}