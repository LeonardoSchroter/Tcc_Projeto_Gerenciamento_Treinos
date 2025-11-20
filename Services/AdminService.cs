using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Projeto_gerencia_treinos_musculacao.Interfaces;
using Projeto_gerencia_treinos_musculacao.Models;
using Projeto_gerencia_treinos_musculacao.ViewModels.User;

namespace Projeto_gerencia_treinos_musculacao.Services
{
    public class AdminService : IAdminService
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminService(UserManager<Usuario> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<List<UserViewModel>> GetUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            var userViewModels = new List<UserViewModel>();

            foreach (var user in users)
            {
                userViewModels.Add(new UserViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Roles = await _userManager.GetRolesAsync(user)
                });
            }

            return userViewModels;
        }

        public async Task<ManageUserRolesViewModel> GetManageRolesViewModelAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return null;
            }

            var personals = await _userManager.GetUsersInRoleAsync("Personal");

            var viewModel = new ManageUserRolesViewModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                Roles = new List<RoleViewModel>(),
                PersonalId = user.PersonalId,
                PersonalsDisponiveis = new SelectList(personals, "Id", "Nome", user.PersonalId)
            };

            foreach (var role in _roleManager.Roles.ToList())
            {
                viewModel.Roles.Add(new RoleViewModel
                {
                    RoleName = role.Name,
                    IsSelected = await _userManager.IsInRoleAsync(user, role.Name)
                });
            }

            return viewModel;
        }

        public async Task<bool> UpdateUserRolesAsync(ManageUserRolesViewModel viewModel)
        {
            var user = await _userManager.FindByIdAsync(viewModel.UserId);
            if (user == null)
            {
                return false;
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);
            await _userManager.AddToRolesAsync(user, viewModel.Roles.Where(r => r.IsSelected).Select(r => r.RoleName));

            var isAluno = viewModel.Roles.Any(r => r.RoleName == "Aluno" && r.IsSelected);

            if (isAluno)
            {
                user.PersonalId = viewModel.PersonalId;
            }
            else
            {
                user.PersonalId = null;
            }

            await _userManager.UpdateAsync(user);

            return true;
        }
    }
}