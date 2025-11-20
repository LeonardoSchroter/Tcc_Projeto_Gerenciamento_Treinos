using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Projeto_gerencia_treinos_musculacao.Data;
using Projeto_gerencia_treinos_musculacao.Interfaces;
using Projeto_gerencia_treinos_musculacao.Models;
using System.Security.Claims;

namespace Projeto_gerencia_treinos_musculacao.Services
{
    public class AlunosService : IAlunosService
    {
        private readonly AuthDbContext _context;
        private readonly UserManager<Usuario> _userManager;

        private readonly SignInManager<Usuario> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AlunosService(AuthDbContext context,
                             UserManager<Usuario> userManager,
                             SignInManager<Usuario> signInManager,
                             RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<List<Usuario>> GetAlunosDoPersonalAsync(ClaimsPrincipal userPrincipal)
        {
            var usuarioAtual = await _userManager.GetUserAsync(userPrincipal);

            return await _context.Users
                .Where(u => u.PersonalId == usuarioAtual.Id)
                .ToListAsync();
        }
    }
}