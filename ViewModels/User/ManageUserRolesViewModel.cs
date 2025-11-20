using Microsoft.AspNetCore.Mvc.Rendering; // Adicione este using
using System.Collections.Generic;

namespace Projeto_gerencia_treinos_musculacao.ViewModels.User
{
    public class ManageUserRolesViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public List<RoleViewModel> Roles { get; set; }


        public string? PersonalId { get; set; }

        public SelectList? PersonalsDisponiveis { get; set; }
    }

    public class RoleViewModel
    {
        public string RoleName { get; set; }
        public bool IsSelected { get; set; }
    }
}