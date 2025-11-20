using Microsoft.AspNetCore.Identity;
using Projeto_gerencia_treinos_musculacao.Interfaces; 
using Projeto_gerencia_treinos_musculacao.Models;
using Projeto_gerencia_treinos_musculacao.ViewModels.Account;

namespace Projeto_gerencia_treinos_musculacao.Services
{
    public class AccountService : IAccountService 
    {
        private readonly SignInManager<Usuario> _signInManager;
        private readonly UserManager<Usuario> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountService(SignInManager<Usuario> signInManager,
                              UserManager<Usuario> userManager,
                              RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<SignInResult> LoginAsync(LoginViewModel model)
        {
            return await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
        }

        public async Task<IdentityResult> RegisterAsync(RegisterViewModel model)
        {
            if (await _userManager.FindByEmailAsync(model.Email) != null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "EmailInUse",
                    Description = "O email já está cadastrado no sistema"
                });
            }

            var user = new Usuario
            {
                Nome = model.Name,
                UserName = model.Name,
                PhoneNumber = model.Telefone,
                NormalizedUserName = model.Email.ToUpper(),
                Email = model.Email,
                NormalizedEmail = model.Email.ToUpper()
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                var roleExist = await _roleManager.RoleExistsAsync("Usuario");

                if (!roleExist)
                {
                    var role = new IdentityRole("Usuario");
                    await _roleManager.CreateAsync(role);
                }

                await _userManager.AddToRoleAsync(user, "Usuario");
                await _signInManager.SignInAsync(user, isPersistent: false);
            }

            return result;
        }

        public async Task<Usuario> VerifyEmailAsync(string email)
        {
            return await _userManager.FindByNameAsync(email);
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}