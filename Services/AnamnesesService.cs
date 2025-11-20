using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Projeto_gerencia_treinos_musculacao.Data;
using Projeto_gerencia_treinos_musculacao.Interfaces;
using Projeto_gerencia_treinos_musculacao.Models;
using Projeto_gerencia_treinos_musculacao.ViewModels;
using System.Security.Claims;
using static Projeto_gerencia_treinos_musculacao.Models.Enum;

namespace Projeto_gerencia_treinos_musculacao.Services
{
    public class AnamnesesService : IAnamnesesService
    {
        private readonly AuthDbContext _context;
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AnamnesesService(AuthDbContext context,
                                UserManager<Usuario> userManager,
                                SignInManager<Usuario> signInManager,
                                RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<List<Anamnese>> GetAnamnesesAsync(ClaimsPrincipal user)
        {
            var usuarioAtual = await _userManager.GetUserAsync(user);
            return await _context.Anamneses
                .Include(a => a.Usuario)
                .Where(a => a.PersonalId == usuarioAtual.Id)
                .ToListAsync();
        }

        public async Task<Anamnese> GetAnamneseDetailsAsync(string id)
        {
            return await _context.Anamneses
                .Include(a => a.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<AnamneseViewModel> BuildCreateViewModelAsync(ClaimsPrincipal user)
        {
            var usuarioAtual = await _userManager.GetUserAsync(user);
            var viewModel = new AnamneseViewModel
            {
                Usuarios = new SelectList(_context.Users
                    .Where(u => u.PersonalId == usuarioAtual.Id), "Id", "Nome"),
                DataRegistro = DateTime.Now
            };
            return viewModel;
        }

        public async Task CreateAnamneseAsync(AnamneseViewModel viewModel, ClaimsPrincipal user)
        {
            var personalLogadoId = _userManager.GetUserId(user);

            var anamnese = new Anamnese
            {
                UsuarioId = viewModel.UsuarioId,
                DataRegistro = viewModel.DataRegistro,
                Doencas = viewModel.Doencas,
                HabitosAlimentares = viewModel.HabitosAlimentares,
                ConsumoAlcool = viewModel.ConsumoAlcool,
                Tabagismo = viewModel.Tabagismo,
                Objetivos = viewModel.Objetivos,
                AtividadeFisicaAtual = viewModel.AtividadeFisicaAtual,
                IntensidadeAtividadeFisica = viewModel.IntensidadeAtividadeFisica.ToString(),
                Observacoes = viewModel.Observacoes,
                Lesoes = viewModel.Lesoes,
                Cirurgias = viewModel.Cirurgias,
                Medicamentos = viewModel.Medicamentos,
                Peso = viewModel.Peso,
                Altura = viewModel.Altura,
                PersonalId = personalLogadoId
            };

            _context.Add(anamnese);
            await _context.SaveChangesAsync();
        }

        public async Task<AnamneseViewModel> BuildEditViewModelAsync(string id)
        {
            var anamnese = await _context.Anamneses.FindAsync(id);
            if (anamnese == null) return null;

            System.Enum.TryParse<NivelIntensidade>(anamnese.IntensidadeAtividadeFisica, out var intensidadeEnum);

            return new AnamneseViewModel
            {
                Id = anamnese.Id,
                UsuarioId = anamnese.UsuarioId,
                DataRegistro = anamnese.DataRegistro ?? DateTime.Now,
                Doencas = anamnese.Doencas,
                HabitosAlimentares = anamnese.HabitosAlimentares,
                ConsumoAlcool = anamnese.ConsumoAlcool,
                Tabagismo = anamnese.Tabagismo,
                Objetivos = anamnese.Objetivos,
                AtividadeFisicaAtual = anamnese.AtividadeFisicaAtual,
                IntensidadeAtividadeFisica = intensidadeEnum,
                Observacoes = anamnese.Observacoes,
                Lesoes = anamnese.Lesoes,
                Cirurgias = anamnese.Cirurgias,
                Medicamentos = anamnese.Medicamentos,
                Peso = anamnese.Peso,
                Altura = anamnese.Altura,
                Usuarios = new SelectList(_context.Users, "Id", "Nome", anamnese.UsuarioId)
            };
        }

        public async Task<bool> UpdateAnamneseAsync(string id, AnamneseViewModel viewModel)
        {
            var anamneseToUpdate = await _context.Anamneses.FindAsync(id);
            if (anamneseToUpdate == null) return false;

            anamneseToUpdate.DataRegistro = viewModel.DataRegistro;
            anamneseToUpdate.Doencas = viewModel.Doencas;
            anamneseToUpdate.HabitosAlimentares = viewModel.HabitosAlimentares;
            anamneseToUpdate.ConsumoAlcool = viewModel.ConsumoAlcool;
            anamneseToUpdate.Tabagismo = viewModel.Tabagismo;
            anamneseToUpdate.Objetivos = viewModel.Objetivos;
            anamneseToUpdate.AtividadeFisicaAtual = viewModel.AtividadeFisicaAtual;
            anamneseToUpdate.IntensidadeAtividadeFisica = viewModel.IntensidadeAtividadeFisica.ToString();
            anamneseToUpdate.Observacoes = viewModel.Observacoes;
            anamneseToUpdate.Lesoes = viewModel.Lesoes;
            anamneseToUpdate.Cirurgias = viewModel.Cirurgias;
            anamneseToUpdate.Medicamentos = viewModel.Medicamentos;
            anamneseToUpdate.Peso = viewModel.Peso;
            anamneseToUpdate.Altura = viewModel.Altura;

            _context.Update(anamneseToUpdate);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Anamnese> GetAnamneseForDeleteAsync(string id)
        {
            return await _context.Anamneses
                .Include(a => a.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task DeleteAnamneseAsync(string id)
        {
            var anamnese = await _context.Anamneses.FindAsync(id);
            if (anamnese != null)
            {
                _context.Anamneses.Remove(anamnese);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<SelectList> GetTodosUsuariosSelectListAsync(string selectedId = null)
        {
            return new SelectList(await _context.Users.ToListAsync(), "Id", "Nome", selectedId);
        }
    }
}