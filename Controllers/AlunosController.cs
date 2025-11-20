using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Projeto_gerencia_treinos_musculacao.Interfaces;
using System.Threading.Tasks;

namespace Projeto_gerencia_treinos_musculacao.Controllers
{
    public class AlunosController : Controller
    {
        private readonly IAlunosService _alunosService;

        public AlunosController(IAlunosService alunosService)
        {
            _alunosService = alunosService;
        }

        [Authorize(Roles = "Personal,Admin")]
        public async Task<IActionResult> Index()
        {
            var alunos = await _alunosService.GetAlunosDoPersonalAsync(User);
            return View(alunos);
        }
    }
}