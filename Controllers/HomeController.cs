using Microsoft.AspNetCore.Mvc;
using Projeto_gerencia_treinos_musculacao.Interfaces;
using Projeto_gerencia_treinos_musculacao.Models;
using Projeto_gerencia_treinos_musculacao.ViewModels;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Projeto_gerencia_treinos_musculacao.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomeService _homeService;

        public HomeController(ILogger<HomeController> logger, IHomeService homeService)
        {
            _logger = logger;
            _homeService = homeService;
        }

        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Personal") || User.IsInRole("Admin"))
            {
                var viewModel = await _homeService.GetPersonalDashboardAsync(User);
                return View("PersonalDashboard", viewModel);
            }

            if (User.IsInRole("Aluno"))
            {
                var viewModel = await _homeService.GetAlunoDashboardAsync(User);
                return View("AlunoDashboard", viewModel);
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}