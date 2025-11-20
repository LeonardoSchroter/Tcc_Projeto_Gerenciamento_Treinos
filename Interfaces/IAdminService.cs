using Projeto_gerencia_treinos_musculacao.Models;
using Projeto_gerencia_treinos_musculacao.ViewModels.User;

namespace Projeto_gerencia_treinos_musculacao.Interfaces
{
    public interface IAdminService
    {
        Task<List<UserViewModel>> GetUsersAsync();
        Task<ManageUserRolesViewModel> GetManageRolesViewModelAsync(string userId);
        Task<bool> UpdateUserRolesAsync(ManageUserRolesViewModel viewModel);
    }
}