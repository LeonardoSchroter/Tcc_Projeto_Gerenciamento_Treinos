using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto_gerencia_treinos_musculacao.Interfaces;
using Projeto_gerencia_treinos_musculacao.ViewModels;
using System.Threading.Tasks;

namespace Projeto_gerencia_treinos_musculacao.Controllers
{
    public class TreinosController : Controller
    {
        private readonly ITreinosService _treinosService;

        public TreinosController(ITreinosService treinosService)
        {
            _treinosService = treinosService;
        }

        [Authorize(Roles = "Personal,Admin")]
        public async Task<IActionResult> Create(string fichaId)
        {
            if (string.IsNullOrEmpty(fichaId)) return NotFound();

            var viewModel = await _treinosService.BuildCreateViewModelAsync(fichaId);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Personal,Admin")]
        public async Task<IActionResult> Create(TreinoEditViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                await _treinosService.CreateTreinoAsync(viewModel);
                return RedirectToAction("Edit", "Fichas", new { id = viewModel.FichaId });
            }

            viewModel.ExerciciosDisponiveis = await _treinosService.GetExerciciosDisponiveisAsync();
            return View(viewModel);
        }

        [Authorize(Roles = "Personal,Admin")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();

            var viewModel = await _treinosService.BuildEditViewModelAsync(id);

            if (viewModel == null) return NotFound();

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Personal,Admin")]
        public async Task<IActionResult> Edit(string id, TreinoEditViewModel viewModel)
        {
            if (id != viewModel.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    bool success = await _treinosService.UpdateTreinoAsync(id, viewModel);
                    if (!success) return NotFound();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }

                return RedirectToAction("Edit", "Fichas", new { id = viewModel.FichaId });
            }

            viewModel.ExerciciosDisponiveis = await _treinosService.GetExerciciosDisponiveisAsync();
            return View(viewModel);
        }

        [Authorize(Roles = "Personal,Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null) return NotFound();

            var treino = await _treinosService.GetTreinoForDeleteAsync(id);

            if (treino == null) return NotFound();

            return View(treino);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Personal,Admin")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var fichaId = await _treinosService.DeleteTreinoAsync(id);

            if (fichaId != null)
            {
                return RedirectToAction("Edit", "Fichas", new { id = fichaId });
            }

            return RedirectToAction("Index", "Fichas");
        }
    }
}