using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Projeto_gerencia_treinos_musculacao.Interfaces;
using Projeto_gerencia_treinos_musculacao.Models;
using Projeto_gerencia_treinos_musculacao.ViewModels.User;
using System.Threading.Tasks;

namespace Projeto_gerencia_treinos_musculacao.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var userViewModels = await _adminService.GetUsersAsync();
            return View(userViewModels);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ManageRoles(string userId)
        {
            var viewModel = await _adminService.GetManageRolesViewModelAsync(userId);

            if (viewModel == null)
            {
                ViewBag.ErrorMessage = $"Usuário não foi encontrado.";
                return View("NotFound");
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ManageRoles(ManageUserRolesViewModel viewModel)
        {
            var success = await _adminService.UpdateUserRolesAsync(viewModel);

            if (!success)
            {
                ViewBag.ErrorMessage = $"Usuário não foi encontrado.";
                return View("NotFound");
            }

            return RedirectToAction(nameof(Index));
        }
    }
}