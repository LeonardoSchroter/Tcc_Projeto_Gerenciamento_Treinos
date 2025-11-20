using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Projeto_gerencia_treinos_musculacao.Data;
using Projeto_gerencia_treinos_musculacao.Interfaces;
using Projeto_gerencia_treinos_musculacao.Models;
using Projeto_gerencia_treinos_musculacao.ViewModels;
using System.Security.Claims;
using System.Text.Json;

namespace Projeto_gerencia_treinos_musculacao.Services
{
    public class FichasService : IFichasService
    {
        private readonly AuthDbContext _context;
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly GeminiService _geminiService;

        public FichasService(AuthDbContext context,
                             UserManager<Usuario> userManager,
                             SignInManager<Usuario> signInManager,
                             RoleManager<IdentityRole> roleManager,
                             GeminiService geminiService)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _geminiService = geminiService;
        }

        public async Task<List<Ficha>> GetFichasAsync(ClaimsPrincipal user)
        {
            var userId = _userManager.GetUserId(user);
            if (user.IsInRole("Personal"))
            {
                return await _context.Fichas
                    .Include(f => f.Aluno)
                    .Include(f => f.Personal)
                    .Where(f => f.IdPersonal == userId)
                    .ToListAsync();
            }
            else
            {
                var dataAtual = DateTime.Now;
                return await _context.Fichas
                    .Include(f => f.Aluno)
                    .Include(f => f.Personal)
                    .Where(f => f.IdAluno == userId && dataAtual > f.DataInicio && dataAtual < f.DataFim)
                    .ToListAsync();
            }
        }

        public async Task<FichaDetailsViewModel> GetFichaDetailsAsync(string id)
        {
            var ficha = await _context.Fichas
                .Include(f => f.Aluno)
                .Include(f => f.Personal)
                .Include(f => f.Treinos)
                    .ThenInclude(t => t.ItensTreino)
                        .ThenInclude(it => it.Exercicio)
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == id);

            if (ficha == null) return null;

            return new FichaDetailsViewModel
            {
                Id = ficha.Id,
                Nome = ficha.Nome,
                Objetivo = ficha.Objetivo,
                DataInicio = ficha.DataInicio,
                DataFim = ficha.DataFim,
                AlunoNome = ficha.Aluno?.Nome,
                PersonalNome = ficha.Personal?.Nome,
                Treinos = ficha.Treinos.ToList()
            };
        }

        public async Task<FichaCreateViewModel> BuildCreateViewModelAsync(string alunoId)
        {
            return new FichaCreateViewModel
            {
                Alunos = new SelectList(await _context.Users.Where(u => u.Role == "Aluno").ToListAsync(), "Id", "Nome", alunoId)
            };
        }

        public async Task<string> CreateFichaAsync(FichaCreateViewModel viewModel, ClaimsPrincipal user)
        {
            var personalId = _userManager.GetUserId(user);

            var ficha = new Ficha
            {
                Id = Guid.NewGuid().ToString(),
                Nome = viewModel.Nome,
                IdAluno = viewModel.IdAluno,
                IdPersonal = personalId,
                DataInicio = viewModel.DataInicio,
                DataFim = viewModel.DataFim,
                Objetivo = viewModel.Objetivo
            };

            _context.Add(ficha);
            await _context.SaveChangesAsync();

            return ficha.Id;
        }

        public async Task<FichaEditViewModel> BuildEditViewModelAsync(string id)
        {
            var ficha = await _context.Fichas
                .Include(f => f.Aluno)
                .Include(f => f.Treinos)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (ficha == null) return null;

            return new FichaEditViewModel
            {
                Id = ficha.Id,
                Nome = ficha.Nome,
                NomeAluno = ficha.Aluno?.Nome,
                DataInicio = ficha.DataInicio,
                DataFim = ficha.DataFim,
                Objetivo = ficha.Objetivo,
                Treinos = ficha.Treinos.ToList()
            };
        }

        public async Task<bool> UpdateFichaAsync(string id, FichaEditViewModel viewModel)
        {
            var fichaToUpdate = await _context.Fichas.FindAsync(id);
            if (fichaToUpdate == null) return false;

            fichaToUpdate.Nome = viewModel.Nome;
            fichaToUpdate.DataInicio = viewModel.DataInicio;
            fichaToUpdate.DataFim = viewModel.DataFim;
            fichaToUpdate.Objetivo = viewModel.Objetivo;

            _context.Update(fichaToUpdate);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Ficha> GetFichaForDeleteAsync(string id)
        {
            return await _context.Fichas
                .Include(f => f.Aluno)
                .Include(f => f.Personal)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task DeleteFichaAsync(string id)
        {
            var ficha = await _context.Fichas.FindAsync(id);
            if (ficha != null)
            {
                _context.Fichas.Remove(ficha);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<(bool Success, string Message)> GerarTreinosComIAAsync(string fichaId, string instrucoes, ClaimsPrincipal user)
        {
            try
            {
                var personalId = _userManager.GetUserId(user);

                var ficha = await _context.Fichas.Include(f => f.Treinos).FirstOrDefaultAsync(f => f.Id == fichaId);
                if (ficha == null) return (false, "Ficha não encontrada.");

                var anamnese = await _context.Anamneses
                    .AsNoTracking()
                    .OrderByDescending(a => a.DataRegistro)
                    .FirstOrDefaultAsync(a => a.UsuarioId == ficha.IdAluno);

                if (anamnese == null)
                {
                    return (false, "Não foi possível gerar o treino: O aluno não possui uma anamnese cadastrada.");
                }

                var exercicios = await _context.Exercicios
                    .Where(e => e.IdUsuario == personalId || e.IdUsuario == null)
                    .AsNoTracking()
                    .ToListAsync();

                var fichaSugeridaJson = await _geminiService.GerarFichaDeTreinoAsync(anamnese, exercicios, instrucoes);

                if (string.IsNullOrEmpty(fichaSugeridaJson))
                {
                    return (false, "A IA não retornou uma resposta válida. Tente novamente.");
                }

                var fichaVmSugerida = JsonSerializer.Deserialize<FichaEditViewModel>(fichaSugeridaJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (ficha.Treinos.Any())
                {
                    _context.Treinos.RemoveRange(ficha.Treinos);
                }

                foreach (var treinoVm in fichaVmSugerida.Treinos)
                {
                    var novoTreino = new Treino
                    {
                        Nome = treinoVm.Nome,
                        ItensTreino = treinoVm.ItensTreino.Select(itemVm => new ItemTreino
                        {
                            ExercicioId = itemVm.ExercicioId,
                            Series = itemVm.Series,
                            Repeticoes = itemVm.Repeticoes,
                            Carga = itemVm.Carga,
                            Descanso = itemVm.Descanso,
                            Ordem = itemVm.Ordem
                        }).ToList()
                    };
                    ficha.Treinos.Add(novoTreino);
                }

                await _context.SaveChangesAsync();
                return (true, "Novas divisões de treino geradas com sucesso!");
            }
            catch (Exception)
            {
                return (false, "Ocorreu um erro interno ao gerar o treino.");
            }
        }

        public async Task<SelectList> GetAlunosSelectListAsync(string selectedId = null)
        {
            return new SelectList(
                await _context.Users.Where(u => u.Role == "Aluno").ToListAsync(),
                "Id", "Nome", selectedId);
        }
    }
}