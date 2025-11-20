using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto_gerencia_treinos_musculacao.Interfaces;
using Projeto_gerencia_treinos_musculacao.ViewModels;
using System.Threading.Tasks;

namespace Projeto_gerencia_treinos_musculacao.Controllers
{
    public class FichasController : Controller
    {
        private readonly IFichasService _fichasService;

        public FichasController(IFichasService fichasService)
        {
            _fichasService = fichasService;
        }

        [Authorize(Roles = "Personal,Admin,Aluno")]
        public async Task<IActionResult> Index()
        {
            var fichas = await _fichasService.GetFichasAsync(User);
            return View(fichas);
        }

        [Authorize(Roles = "Personal,Admin,Aluno")]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null) return NotFound();

            var viewModel = await _fichasService.GetFichaDetailsAsync(id);

            if (viewModel == null) return NotFound();

            return View(viewModel);
        }

        [Authorize(Roles = "Personal,Admin")]
        public async Task<IActionResult> Create(string alunoId)
        {
            var viewModel = await _fichasService.BuildCreateViewModelAsync(alunoId);
            return View(viewModel);
        }

        [Authorize(Roles = "Personal,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FichaCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var novoId = await _fichasService.CreateFichaAsync(viewModel, User);
                return RedirectToAction(nameof(Edit), new { id = novoId });
            }

            viewModel.Alunos = await _fichasService.GetAlunosSelectListAsync(viewModel.IdAluno);
            return View(viewModel);
        }

        [Authorize(Roles = "Personal,Admin")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();

            var viewModel = await _fichasService.BuildEditViewModelAsync(id);

            if (viewModel == null) return NotFound();

            return View(viewModel);
        }

        [Authorize(Roles = "Personal,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, FichaEditViewModel viewModel)
        {
            if (id != viewModel.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    bool success = await _fichasService.UpdateFichaAsync(id, viewModel);
                    if (!success) return NotFound();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Edit), new { id = viewModel.Id });
            }
            return View(viewModel);
        }

        [Authorize(Roles = "Personal,Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null) return NotFound();

            var ficha = await _fichasService.GetFichaForDeleteAsync(id);

            if (ficha == null) return NotFound();

            return View(ficha);
        }

        [Authorize(Roles = "Personal,Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _fichasService.DeleteFichaAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GerarTreinosIA(string fichaId, string instrucoes)
        {
            if (string.IsNullOrEmpty(fichaId))
            {
                return NotFound();
            }

            var (success, message) = await _fichasService.GerarTreinosComIAAsync(fichaId, instrucoes, User);

            if (success)
            {
                TempData["Success"] = message;
            }
            else
            {
                TempData["Error"] = message;
            }

            return RedirectToAction(nameof(Edit), new { id = fichaId });
        }
    }
}