using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto_gerencia_treinos_musculacao.Interfaces;
using Projeto_gerencia_treinos_musculacao.ViewModels;
using System;
using System.Threading.Tasks;

namespace Projeto_gerencia_treinos_musculacao.Controllers
{
    public class AnamnesesController : Controller
    {
        private readonly IAnamnesesService _anamnesesService;

        public AnamnesesController(IAnamnesesService anamnesesService)
        {
            _anamnesesService = anamnesesService;
        }

        [Authorize(Roles = "Personal,Admin")]
        public async Task<IActionResult> Index()
        {
            var anamneses = await _anamnesesService.GetAnamnesesAsync(User);
            return View(anamneses);
        }

        [Authorize(Roles = "Personal,Admin")]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null) return NotFound();

            var anamnese = await _anamnesesService.GetAnamneseDetailsAsync(id);

            if (anamnese == null) return NotFound();

            return View(anamnese);
        }

        [Authorize(Roles = "Personal,Admin")]
        public async Task<IActionResult> Create()
        {
            var viewModel = await _anamnesesService.BuildCreateViewModelAsync(User);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Personal,Admin")]
        public async Task<IActionResult> Create(AnamneseViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                await _anamnesesService.CreateAnamneseAsync(viewModel, User);
                return RedirectToAction(nameof(Index));
            }

            viewModel.Usuarios = await _anamnesesService.GetTodosUsuariosSelectListAsync(viewModel.UsuarioId);
            return View(viewModel);
        }

        [Authorize(Roles = "Personal,Admin")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();

            var viewModel = await _anamnesesService.BuildEditViewModelAsync(id);

            if (viewModel == null) return NotFound();

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Personal,Admin")]
        public async Task<IActionResult> Edit(string id, AnamneseViewModel viewModel)
        {
            if (id != viewModel.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    bool success = await _anamnesesService.UpdateAnamneseAsync(id, viewModel);
                    if (!success) return NotFound();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            viewModel.Usuarios = await _anamnesesService.GetTodosUsuariosSelectListAsync(viewModel.UsuarioId);
            return View(viewModel);
        }

        [Authorize(Roles = "Personal,Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null) return NotFound();

            var anamnese = await _anamnesesService.GetAnamneseForDeleteAsync(id);

            if (anamnese == null) return NotFound();

            return View(anamnese);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Personal,Admin")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _anamnesesService.DeleteAnamneseAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}