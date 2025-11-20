using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Projeto_gerencia_treinos_musculacao.Data;
using Projeto_gerencia_treinos_musculacao.Interfaces;
using Projeto_gerencia_treinos_musculacao.Models;
using Projeto_gerencia_treinos_musculacao.Services;
using System;
using System.Threading.Tasks;
using System.Web;

namespace Projeto_gerencia_treinos_musculacao
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<AuthDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddScoped<GeminiService>();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<IAdminService, AdminService>();
            builder.Services.AddScoped<IAlunosService, AlunosService>();
            builder.Services.AddScoped<IAnamnesesService, AnamnesesService>();
            builder.Services.AddScoped<IExerciciosService, ExerciciosService>();
            builder.Services.AddScoped<IFichasService, FichasService>();
            builder.Services.AddScoped<IHomeService, HomeService>();
            builder.Services.AddScoped<ITreinosService, TreinosService>();
            builder.Services.AddIdentity<Usuario, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequiredLength = 6;
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
            }).AddEntityFrameworkStores<AuthDbContext>().AddDefaultTokenProviders();


            var app = builder.Build();

            await SeedService.SeedDatabase(app.Services);

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
