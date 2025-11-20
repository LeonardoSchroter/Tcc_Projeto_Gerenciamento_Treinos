using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Projeto_gerencia_treinos_musculacao.Data;
using Projeto_gerencia_treinos_musculacao.Interfaces;
using Projeto_gerencia_treinos_musculacao.Models;
using Projeto_gerencia_treinos_musculacao.ViewModels;
using System.Security.Claims;

namespace Projeto_gerencia_treinos_musculacao.Services
{
    public class HomeService : IHomeService
    {
        private readonly AuthDbContext _context;
        private readonly UserManager<Usuario> _userManager;

        public HomeService(AuthDbContext context, UserManager<Usuario> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<PersonalDashboardViewModel> GetPersonalDashboardAsync(ClaimsPrincipal user)
        {
            var personalId = _userManager.GetUserId(user);
            var hoje = DateTime.Today;

            var viewModel = new PersonalDashboardViewModel
            {
                TotalAlunos = await _context.Users.CountAsync(u => u.PersonalId == personalId),
                FichasAtivas = await _context.Fichas.CountAsync(f => f.IdPersonal == personalId && f.DataFim >= hoje),
                FichasExpirandoList = await _context.Fichas
                    .Where(f => f.IdPersonal == personalId && f.DataFim >= hoje && f.DataFim <= hoje.AddDays(7))
                    .Include(f => f.Aluno)
                    .ToListAsync(),
                FichasExpirandoCount = await _context.Fichas
                    .Where(f => f.IdPersonal == personalId && f.DataFim >= hoje && f.DataFim <= hoje.AddDays(7))
                    .Include(f => f.Aluno)
                    .CountAsync(),
                AlunosSemFichaList = await _context.Users
                    .Where(u => u.PersonalId == personalId && !_context.Fichas.Any(f => f.IdAluno == u.Id))
                    .ToListAsync(),
                AlunosSemFichaCount = await _context.Users
                    .Where(u => u.PersonalId == personalId && !_context.Fichas.Any(f => f.IdAluno == u.Id))
                    .CountAsync()
            };

            return viewModel;
        }

        public async Task<AlunoDashboardViewModel> GetAlunoDashboardAsync(ClaimsPrincipal user)
        {
            var alunoId = _userManager.GetUserId(user);
            var alunoNome = _userManager.GetUserName(user);
            var hoje = DateTime.Today;

            var fichaAtiva = await _context.Fichas
                .Include(f => f.Personal)
                .FirstOrDefaultAsync(f => f.IdAluno == alunoId && f.DataInicio <= hoje && f.DataFim >= hoje);

            var viewModel = new AlunoDashboardViewModel
            {
                NomeAluno = alunoNome,
                FichaAtiva = fichaAtiva
            };

            return viewModel;
        }
    }
}