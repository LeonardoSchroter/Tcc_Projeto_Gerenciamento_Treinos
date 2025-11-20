using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Projeto_gerencia_treinos_musculacao.Data;
using Projeto_gerencia_treinos_musculacao.Interfaces;
using Projeto_gerencia_treinos_musculacao.Models;
using Projeto_gerencia_treinos_musculacao.ViewModels.Exercicio;
using System.Security.Claims;

namespace Projeto_gerencia_treinos_musculacao.Services
{
    public class ExerciciosService : IExerciciosService
    {
        private readonly AuthDbContext _context;
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ExerciciosService(AuthDbContext context,
                                 UserManager<Usuario> userManager,
                                 SignInManager<Usuario> signInManager,
                                 RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<List<Exercicio>> GetExerciciosAsync(ClaimsPrincipal user)
        {
            var usuarioAtual = await _userManager.GetUserAsync(user);
            return await _context.Exercicios
                .Where(e => e.IdUsuario == usuarioAtual.Id || e.IdUsuario == null)
                .ToListAsync();
        }

        public async Task<Exercicio> GetExercicioDetailsAsync(string id)
        {
            return await _context.Exercicios.FirstOrDefaultAsync(m => m.Id == id);
        }

        public ExercicioEditViewModel BuildCreateViewModel()
        {
            return new ExercicioEditViewModel();
        }

        public async Task CreateExercicioAsync(ExercicioCreateViewModel viewModel, ClaimsPrincipal user)
        {
            var usuarioAtual = await _userManager.GetUserAsync(user);
            var exercicio = new Exercicio
            {
                Nome = viewModel.Nome,
                Execucao = viewModel.Execucao,
                GrupoMuscular = viewModel.GrupoMuscular.ToString(),
                Equipamento = viewModel.Equipamento,
                UsuarioCriador = usuarioAtual
            };

            _context.Add(exercicio);
            await _context.SaveChangesAsync();
        }

        public async Task<ExercicioEditViewModel> BuildEditViewModelAsync(string id)
        {
            var exercicio = await _context.Exercicios.FindAsync(id);
            if (exercicio == null)
            {
                return null;
            }

            System.Enum.TryParse(exercicio.GrupoMuscular, out Models.Enum.GrupoMuscular grupoMuscularEnum);

            return new ExercicioEditViewModel
            {
                Id = exercicio.Id,
                Nome = exercicio.Nome,
                Execucao = exercicio.Execucao,
                GrupoMuscular = grupoMuscularEnum,
                Equipamento = exercicio.Equipamento
            };
        }

        public async Task<bool> UpdateExercicioAsync(string id, ExercicioEditViewModel viewModel)
        {
            var exercicioToUpdate = await _context.Exercicios.FindAsync(id);
            if (exercicioToUpdate == null)
            {
                return false;
            }

            exercicioToUpdate.Nome = viewModel.Nome;
            exercicioToUpdate.Execucao = viewModel.Execucao;
            exercicioToUpdate.GrupoMuscular = viewModel.GrupoMuscular.ToString();
            exercicioToUpdate.Equipamento = viewModel.Equipamento;

            _context.Update(exercicioToUpdate);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task DeleteExercicioAsync(string id)
        {
            var exercicio = await _context.Exercicios.FindAsync(id);
            if (exercicio != null)
            {
                _context.Exercicios.Remove(exercicio);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExercicioExistsAsync(string id)
        {
            return await _context.Exercicios.AnyAsync(e => e.Id == id);
        }
    }
}