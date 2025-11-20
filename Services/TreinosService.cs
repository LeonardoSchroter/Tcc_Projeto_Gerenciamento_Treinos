using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Projeto_gerencia_treinos_musculacao.Data;
using Projeto_gerencia_treinos_musculacao.Interfaces;
using Projeto_gerencia_treinos_musculacao.Models;
using Projeto_gerencia_treinos_musculacao.ViewModels;

namespace Projeto_gerencia_treinos_musculacao.Services
{
    public class TreinosService : ITreinosService
    {
        private readonly AuthDbContext _context;
        private readonly UserManager<Usuario> _userManager;

        public TreinosService(AuthDbContext context, UserManager<Usuario> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<List<Exercicio>> GetExerciciosDisponiveisAsync()
        {
            return await _context.Exercicios.ToListAsync();
        }

        public async Task<TreinoEditViewModel> BuildCreateViewModelAsync(string fichaId)
        {
            return new TreinoEditViewModel
            {
                FichaId = fichaId,
                ExerciciosDisponiveis = await GetExerciciosDisponiveisAsync()
            };
        }

        public async Task CreateTreinoAsync(TreinoEditViewModel viewModel)
        {
            var treino = new Treino
            {
                Id = Guid.NewGuid().ToString(),
                Nome = viewModel.Nome,
                FichaId = viewModel.FichaId,
                ItensTreino = new List<ItemTreino>()
            };

            if (viewModel.ItensTreino != null)
            {
                foreach (var itemVm in viewModel.ItensTreino)
                {
                    treino.ItensTreino.Add(new ItemTreino
                    {
                        Id = Guid.NewGuid().ToString(),
                        ExercicioId = itemVm.ExercicioId,
                        Series = itemVm.Series,
                        Repeticoes = itemVm.Repeticoes,
                        Carga = itemVm.Carga,
                        Descanso = itemVm.Descanso,
                        Ordem = itemVm.Ordem
                    });
                }
            }

            _context.Add(treino);
            await _context.SaveChangesAsync();
        }

        public async Task<TreinoEditViewModel> BuildEditViewModelAsync(string id)
        {
            var treino = await _context.Treinos
                .Include(t => t.ItensTreino)
                    .ThenInclude(it => it.Exercicio)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (treino == null) return null;

            var itensTreinoViewModel = treino.ItensTreino.Select(it => new ItemTreinoViewModel
            {
                ExercicioId = it.ExercicioId,
                ExercicioNome = it.Exercicio.Nome,
                Series = it.Series,
                Repeticoes = it.Repeticoes,
                Carga = it.Carga,
                Descanso = it.Descanso,
                Ordem = it.Ordem
            }).OrderBy(it => it.Ordem).ToList();

            var viewModel = new TreinoEditViewModel
            {
                Id = treino.Id,
                Nome = treino.Nome,
                FichaId = treino.FichaId,
                ItensTreino = itensTreinoViewModel,
                ExerciciosDisponiveis = await GetExerciciosDisponiveisAsync()
            };

            return viewModel;
        }

        public async Task<bool> UpdateTreinoAsync(string id, TreinoEditViewModel viewModel)
        {
            var treinoFromDb = await _context.Treinos
                .Include(t => t.ItensTreino)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (treinoFromDb == null) return false;

            treinoFromDb.Nome = viewModel.Nome;

            _context.ItensTreino.RemoveRange(treinoFromDb.ItensTreino);

            if (viewModel.ItensTreino != null)
            {
                foreach (var itemVm in viewModel.ItensTreino)
                {
                    treinoFromDb.ItensTreino.Add(new ItemTreino
                    {
                        ExercicioId = itemVm.ExercicioId,
                        Series = itemVm.Series,
                        Repeticoes = itemVm.Repeticoes,
                        Carga = itemVm.Carga,
                        Descanso = itemVm.Descanso,
                        Ordem = itemVm.Ordem
                    });
                }
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Treino> GetTreinoForDeleteAsync(string id)
        {
            return await _context.Treinos
                .Include(t => t.Ficha)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<string> DeleteTreinoAsync(string id)
        {
            var treino = await _context.Treinos.FindAsync(id);
            if (treino != null)
            {
                var fichaId = treino.FichaId;
                _context.Treinos.Remove(treino);
                await _context.SaveChangesAsync();
                return fichaId;
            }
            return null;
        }
    }
}