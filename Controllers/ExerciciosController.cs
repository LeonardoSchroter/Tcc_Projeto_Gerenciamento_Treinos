using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto_gerencia_treinos_musculacao.Interfaces;
using Projeto_gerencia_treinos_musculacao.ViewModels.Exercicio;
using System.Threading.Tasks;

namespace Projeto_gerencia_treinos_musculacao.Controllers
{
    public class ExerciciosController : Controller
    {
        private readonly IExerciciosService _exerciciosService;

        public ExerciciosController(IExerciciosService exerciciosService)
        {
            _exerciciosService = exerciciosService;
        }

        [Authorize(Roles = "Personal,Admin")]
        public async Task<IActionResult> Index()
        {
            var exercicios = await _exerciciosService.GetExerciciosAsync(User);
            return View(exercicios);
        }

        [Authorize(Roles = "Personal,Admin")]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exercicio = await _exerciciosService.GetExercicioDetailsAsync(id);
            if (exercicio == null)
            {
                return NotFound();
            }

            return View(exercicio);
        }

        [Authorize(Roles = "Personal,Admin")]
        public IActionResult Create()
        {
            return View(_exerciciosService.BuildCreateViewModel());
        }

        [Authorize(Roles = "Personal,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nome,Execucao,GrupoMuscular,Equipamento")] ExercicioCreateViewModel exercicioViewModel)
        {
            if (ModelState.IsValid)
            {
                await _exerciciosService.CreateExercicioAsync(exercicioViewModel, User);
                return RedirectToAction(nameof(Index));
            }
            return View(exercicioViewModel);
        }

        [Authorize(Roles = "Personal,Admin")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exercicioViewModel = await _exerciciosService.BuildEditViewModelAsync(id);
            if (exercicioViewModel == null)
            {
                return NotFound();
            }

            return View(exercicioViewModel);
        }

        [Authorize(Roles = "Personal,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Nome,Execucao,GrupoMuscular,Equipamento")] ExercicioEditViewModel exercicioViewModel)
        {
            if (id != exercicioViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    bool success = await _exerciciosService.UpdateExercicioAsync(id, exercicioViewModel);
                    if (!success)
                    {
                        return NotFound();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _exerciciosService.ExercicioExistsAsync(exercicioViewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(exercicioViewModel);
        }

        [Authorize(Roles = "Personal,Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null) return NotFound();

            var exercicio = await _exerciciosService.GetExercicioDetailsAsync(id);
            if (exercicio == null) return NotFound();

            return View(exercicio);
        }

        [Authorize(Roles = "Personal,Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                await _exerciciosService.DeleteExercicioAsync(id);
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(string.Empty,
                    "Não é possível excluir este exercício, pois ele já está sendo utilizado em uma ou mais fichas de treino.");

                var exercicio = await _exerciciosService.GetExercicioDetailsAsync(id);
                return View(exercicio);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}