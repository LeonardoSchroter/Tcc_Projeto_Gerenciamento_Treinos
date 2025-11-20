using Projeto_gerencia_treinos_musculacao.Models;
using System.Security.Claims;

namespace Projeto_gerencia_treinos_musculacao.Interfaces
{
    public interface IAlunosService
    {
        Task<List<Usuario>> GetAlunosDoPersonalAsync(ClaimsPrincipal userPrincipal);
    }
}