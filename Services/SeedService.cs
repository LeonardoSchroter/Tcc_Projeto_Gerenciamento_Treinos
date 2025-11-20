using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Projeto_gerencia_treinos_musculacao.Data;
using Projeto_gerencia_treinos_musculacao.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Projeto_gerencia_treinos_musculacao.Services
{
    public class SeedService
    {
        public static async Task SeedDatabase(IServiceProvider service)
        {
            using var scope = service.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Usuario>>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<SeedService>>();

            try
            {
                logger.LogInformation("Garantindo que o banco de dados será criado");
                await context.Database.EnsureCreatedAsync();

                //--- 1. CRIAÇÃO DE ROLES ---
                logger.LogInformation("Criando roles");
                await AddRoleAsync(roleManager, "Admin");
                await AddRoleAsync(roleManager, "Personal"); // Nova role
                await AddRoleAsync(roleManager, "Aluno");   // Nova role

                //--- 2. CRIAÇÃO DO USUÁRIO ADMIN ---
                logger.LogInformation("Semeando usuário de admin");
                await CreateUserWithRoleAsync(userManager, logger,
                    new Usuario
                    {
                        Nome = "SysAdmin",
                        UserName = "admin@ufn.com",
                        Email = "admin@ufn.com",
                        Role = "Admin" // Preenchendo a propriedade customizada
                    }, "Admin@123", "Admin");

                //--- 3. CRIAÇÃO DO USUÁRIO PERSONAL ---
                logger.LogInformation("Semeando usuário Personal");
                var personalUser = await CreateUserWithRoleAsync(userManager, logger,
                    new Usuario
                    {
                        Nome = "Carlos Treinador",
                        UserName = "personal@ufn.com",
                        Email = "personal@ufn.com",
                        Role = "Personal" // Preenchendo a propriedade customizada
                    }, "Personal@123", "Personal");

                //--- 4. CRIAÇÃO DOS USUÁRIOS ALUNOS, VINCULADOS AO PERSONAL ---
                if (personalUser != null)
                {
                    logger.LogInformation("Semeando usuários Alunos");

                    // Aluno 1
                    await CreateUserWithRoleAsync(userManager, logger,
                        new Usuario
                        {
                            Nome = "Fernanda Aluna",
                            UserName = "aluno1@ufn.com",
                            Email = "aluno1@ufn.com",
                            PersonalId = personalUser.Id, // Vínculo com o Personal
                            Role = "Aluno" // Preenchendo a propriedade customizada
                        }, "Aluno@123", "Aluno");

                    // Aluno 2
                    await CreateUserWithRoleAsync(userManager, logger,
                        new Usuario
                        {
                            Nome = "Bruno Aluno",
                            UserName = "aluno2@ufn.com",
                            Email = "aluno2@ufn.com",
                            PersonalId = personalUser.Id, // Vínculo com o Personal
                            Role = "Aluno" // Preenchendo a propriedade customizada
                        }, "Aluno@123", "Aluno");
                }
                else
                {
                    logger.LogWarning("Não foi possível criar os alunos pois o usuário Personal não foi encontrado ou criado.");
                }
                if (context.Exercicios.Count() == 0)
                    SeedExerciciosAsync(context, logger);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Ocorreu um erro durante o processo de seed do banco de dados.");
                throw;
            }
        }

        private static async Task AddRoleAsync(RoleManager<IdentityRole> roleManager, string roleName)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                var result = await roleManager.CreateAsync(new IdentityRole(roleName));
                if (!result.Succeeded)
                {
                    throw new Exception($"Falha ao criar Role '{roleName}': {string.Join(",", result.Errors.Select(e => e.Description))}");
                }
            }
        }

        private static async Task<Usuario> CreateUserWithRoleAsync(UserManager<Usuario> userManager, ILogger logger, Usuario user, string password, string role)
        {
            if (await userManager.FindByEmailAsync(user.Email) == null)
            {
                user.NormalizedUserName = user.Email.ToUpper();
                user.NormalizedEmail = user.Email.ToUpper();
                user.EmailConfirmed = true;
                user.SecurityStamp = Guid.NewGuid().ToString();

                var result = await userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    logger.LogInformation($"Usuário '{user.Email}' criado com sucesso.");
                    await userManager.AddToRoleAsync(user, role);
                    logger.LogInformation($"Role '{role}' concedida para o usuário '{user.Email}'.");
                    return user;
                }
                else
                {
                    logger.LogError($"Falha ao criar usuário '{user.Email}': " + string.Join(", ", result.Errors.Select(e => e.Description)));
                    return null;
                }
            }
            // Se o usuário já existe, busca e retorna ele para garantir que os dependentes possam ser criados
            return await userManager.FindByEmailAsync(user.Email);
        }

        private static async Task SeedExerciciosAsync(AuthDbContext context, ILogger logger)
        {
            // Verifica se a tabela de exercícios já tem algum registro
            if (!await context.Exercicios.AnyAsync())
            {
                var exercicios = GetExerciciosPredefinidos();
                await context.Exercicios.AddRangeAsync(exercicios);
                await context.SaveChangesAsync();
                logger.LogInformation($"Semeados {exercicios.Count} exercícios com sucesso.");
            }
            else
            {
                logger.LogInformation("A tabela de exercícios já contém dados. Nenhuma ação foi tomada.");
            }
        }

        private static List<Exercicio> GetExerciciosPredefinidos()
        {
            return new List<Exercicio>
    {
        // --- PEITO ---
        new Exercicio { Nome = "Supino Reto com Barra", Execucao = "Deitado em banco reto, desça a barra até o peito e empurre para cima.", GrupoMuscular = "Peito", Equipamento = "Barra e Banco" },
        new Exercicio { Nome = "Supino Inclinado com Halteres", Execucao = "Deitado em banco inclinado, desça os halteres até a altura do peito e empurre para cima.", GrupoMuscular = "Peito", Equipamento = "Halteres e Banco Inclinado" },
        new Exercicio { Nome = "Crucifixo Reto", Execucao = "Deitado em banco reto, abra os braços com os halteres e feche-os sobre o peito.", GrupoMuscular = "Peito", Equipamento = "Halteres e Banco" },
        new Exercicio { Nome = "Flexão de Braço", Execucao = "Com as mãos no chão, na largura dos ombros, desça o corpo até o peito quase tocar o chão e suba.", GrupoMuscular = "Peito", Equipamento = "Peso do Corpo" },
        new Exercicio { Nome = "Voador (Peck Deck)", Execucao = "Sentado na máquina, junte os braços à frente do corpo controlando o movimento.", GrupoMuscular = "Peito", Equipamento = "Máquina Voador" },

        // --- COSTAS ---
        new Exercicio { Nome = "Barra Fixa", Execucao = "Pendurado na barra, puxe o corpo para cima até o queixo passar da barra.", GrupoMuscular = "Costas", Equipamento = "Barra Fixa" },
        new Exercicio { Nome = "Puxada Alta Frontal (Pulley)", Execucao = "Sentado na máquina, puxe a barra até a parte superior do peito.", GrupoMuscular = "Costas", Equipamento = "Máquina (Pulley)" },
        new Exercicio { Nome = "Remada Curvada com Barra", Execucao = "Com o tronco inclinado, puxe a barra em direção ao abdômen.", GrupoMuscular = "Costas", Equipamento = "Barra" },
        new Exercicio { Nome = "Remada Baixa (Triângulo)", Execucao = "Sentado, puxe o triângulo em direção ao abdômen, mantendo as costas retas.", GrupoMuscular = "Costas", Equipamento = "Máquina (Remada Baixa)" },
        new Exercicio { Nome = "Serrote com Halter", Execucao = "Apoiado em um banco, puxe o halter para cima, ao lado do tronco.", GrupoMuscular = "Costas", Equipamento = "Halter e Banco" },
        new Exercicio { Nome = "Crucifixo Invertido na Máquina", Execucao = "Sentado na máquina, abra os braços para trás, contraindo a parte posterior dos ombros.", GrupoMuscular = "Costas e Ombros", Equipamento = "Máquina Crucifixo Invertido" },

        // --- PERNAS (QUADRÍCEPS) ---
        new Exercicio { Nome = "Agachamento Livre com Barra", Execucao = "Com a barra nas costas, agache como se fosse sentar em uma cadeira, mantendo a coluna reta.", GrupoMuscular = "Pernas (Quadríceps e Glúteos)", Equipamento = "Barra e Suporte" },
        new Exercicio { Nome = "Leg Press 45°", Execucao = "Sentado na máquina, empurre a plataforma com os pés.", GrupoMuscular = "Pernas (Quadríceps)", Equipamento = "Máquina (Leg Press)" },
        new Exercicio { Nome = "Cadeira Extensora", Execucao = "Sentado na máquina, estenda as pernas para cima.", GrupoMuscular = "Pernas (Quadríceps)", Equipamento = "Máquina (Cadeira Extensora)" },
        new Exercicio { Nome = "Afundo com Halteres", Execucao = "Dê um passo à frente e agache, formando um ângulo de 90 graus com ambas as pernas.", GrupoMuscular = "Pernas (Quadríceps e Glúteos)", Equipamento = "Halteres" },
        new Exercicio { Nome = "Agachamento Hack", Execucao = "Utilizando a máquina Hack, agache mantendo as costas apoiadas.", GrupoMuscular = "Pernas (Quadríceps)", Equipamento = "Máquina (Hack)" },

        // --- PERNAS (POSTERIORES E GLÚTEOS) ---
        new Exercicio { Nome = "Levantamento Terra Romeno (Stiff)", Execucao = "Com as pernas semi-flexionadas, desça a barra ou halteres em direção ao chão.", GrupoMuscular = "Pernas (Posteriores)", Equipamento = "Barra ou Halteres" },
        new Exercicio { Nome = "Mesa Flexora", Execucao = "Deitado na máquina, flexione os joelhos, puxando o apoio em direção aos glúteos.", GrupoMuscular = "Pernas (Posteriores)", Equipamento = "Máquina (Mesa Flexora)" },
        new Exercicio { Nome = "Elevação Pélvica", Execucao = "Deitado com as costas apoiadas em um banco, eleve o quadril com uma barra ou anilha sobre ele.", GrupoMuscular = "Glúteos", Equipamento = "Barra ou Anilha e Banco" },
        new Exercicio { Nome = "Cadeira Flexora", Execucao = "Sentado na máquina, flexione os joelhos para baixo.", GrupoMuscular = "Pernas (Posteriores)", Equipamento = "Máquina (Cadeira Flexora)" },
        new Exercicio { Nome = "Bom dia com Barra", Execucao = "Com a barra nas costas, incline o tronco para a frente mantendo as pernas retas.", GrupoMuscular = "Pernas (Posteriores) e Lombar", Equipamento = "Barra" },

        // --- PANTURRILHAS ---
        new Exercicio { Nome = "Panturrilha em Pé (Máquina)", Execucao = "Na máquina, eleve os calcanhares o máximo possível.", GrupoMuscular = "Panturrilhas", Equipamento = "Máquina (Panturrilha em Pé)" },
        new Exercicio { Nome = "Panturrilha Sentado", Execucao = "Sentado na máquina, eleve os calcanhares.", GrupoMuscular = "Panturrilhas", Equipamento = "Máquina (Panturrilha Sentado)" },
        new Exercicio { Nome = "Panturrilha no Leg Press", Execucao = "Na máquina de leg press, empurre a plataforma apenas com a ponta dos pés.", GrupoMuscular = "Panturrilhas", Equipamento = "Máquina (Leg Press)" },

        // --- OMBROS ---
        new Exercicio { Nome = "Desenvolvimento Militar com Barra", Execucao = "Sentado ou em pé, empurre a barra para cima da cabeça.", GrupoMuscular = "Ombros", Equipamento = "Barra" },
        new Exercicio { Nome = "Elevação Lateral com Halteres", Execucao = "Em pé, eleve os halteres lateralmente até a altura dos ombros.", GrupoMuscular = "Ombros", Equipamento = "Halteres" },
        new Exercicio { Nome = "Elevação Frontal com Anilha", Execucao = "Em pé, eleve a anilha à frente do corpo até a altura dos ombros.", GrupoMuscular = "Ombros", Equipamento = "Anilha" },
        new Exercicio { Nome = "Remada Alta", Execucao = "Puxe a barra para cima em direção ao queixo, elevando os cotovelos.", GrupoMuscular = "Ombros e Trapézio", Equipamento = "Barra ou Polia" },
        new Exercicio { Nome = "Encolhimento com Halteres", Execucao = "Segurando halteres, encolha os ombros em direção às orelhas.", GrupoMuscular = "Trapézio", Equipamento = "Halteres" },

        // --- BÍCEPS ---
        new Exercicio { Nome = "Rosca Direta com Barra", Execucao = "Em pé, flexione os cotovelos, trazendo a barra para cima.", GrupoMuscular = "Bíceps", Equipamento = "Barra" },
        new Exercicio { Nome = "Rosca Alternada com Halteres", Execucao = "Sentado ou em pé, flexione um cotovelo de cada vez.", GrupoMuscular = "Bíceps", Equipamento = "Halteres" },
        new Exercicio { Nome = "Rosca Martelo", Execucao = "Com pegada neutra (palmas das mãos viradas uma para a outra), flexione os cotovelos.", GrupoMuscular = "Bíceps e Antebraço", Equipamento = "Halteres" },
        new Exercicio { Nome = "Rosca Scott na Máquina", Execucao = "Apoiado no banco Scott, flexione os cotovelos.", GrupoMuscular = "Bíceps", Equipamento = "Máquina (Rosca Scott)" },
        new Exercicio { Nome = "Rosca Concentrada", Execucao = "Sentado, com o cotovelo apoiado na parte interna da coxa, flexione o cotovelo.", GrupoMuscular = "Bíceps", Equipamento = "Halter" },

        // --- TRÍCEPS ---
        new Exercicio { Nome = "Tríceps Pulley com Barra", Execucao = "Na polia alta, empurre a barra para baixo até estender completamente os cotovelos.", GrupoMuscular = "Tríceps", Equipamento = "Máquina (Pulley)" },
        new Exercicio { Nome = "Tríceps Testa com Barra", Execucao = "Deitado, desça a barra em direção à testa e estenda os cotovelos.", GrupoMuscular = "Tríceps", Equipamento = "Barra e Banco" },
        new Exercicio { Nome = "Mergulho no Banco", Execucao = "Apoiado em um banco, flexione os cotovelos para descer o corpo.", GrupoMuscular = "Tríceps", Equipamento = "Banco e Peso do Corpo" },
        new Exercicio { Nome = "Tríceps Francês com Halter", Execucao = "Deitado ou sentado, estenda os cotovelos acima da cabeça.", GrupoMuscular = "Tríceps", Equipamento = "Halter" },
        new Exercicio { Nome = "Tríceps Coice", Execucao = "Com o tronco inclinado, estenda o cotovelo para trás.", GrupoMuscular = "Tríceps", Equipamento = "Halter" },

        // --- ABDÔMEN ---
        new Exercicio { Nome = "Abdominal Supra (Crunch)", Execucao = "Deitado, eleve o tronco em direção aos joelhos.", GrupoMuscular = "Abdômen", Equipamento = "Peso do Corpo" },
        new Exercicio { Nome = "Abdominal Infra na Paralela", Execucao = "Apoiado nas barras paralelas, eleve os joelhos em direção ao peito.", GrupoMuscular = "Abdômen", Equipamento = "Paralelas" },
        new Exercicio { Nome = "Prancha Isométrica", Execucao = "Mantenha o corpo reto, apoiado nos antebraços e pontas dos pés.", GrupoMuscular = "Abdômen", Equipamento = "Peso do Corpo" },
        new Exercicio { Nome = "Elevação de Pernas Deitado", Execucao = "Deitado, eleve as pernas retas até formarem 90 graus com o quadril.", GrupoMuscular = "Abdômen", Equipamento = "Peso do Corpo" },
        new Exercicio { Nome = "Abdominal Oblíquo (Bicicleta)", Execucao = "Deitado, simule pedaladas no ar, tocando o cotovelo no joelho oposto.", GrupoMuscular = "Abdômen", Equipamento = "Peso do Corpo" },
        
        // --- CARDIO ---
        new Exercicio { Nome = "Esteira (Corrida)", Execucao = "Correr na esteira em velocidade moderada a alta.", GrupoMuscular = "Cardio", Equipamento = "Esteira" },
        new Exercicio { Nome = "Esteira (Caminhada Inclinada)", Execucao = "Caminhar na esteira com inclinação elevada.", GrupoMuscular = "Cardio", Equipamento = "Esteira" },
        new Exercicio { Nome = "Bicicleta Ergométrica", Execucao = "Pedalar em ritmo constante.", GrupoMuscular = "Cardio", Equipamento = "Bicicleta Ergométrica" },
        new Exercicio { Nome = "Elíptico (Transport)", Execucao = "Movimentar pernas e braços no aparelho elíptico.", GrupoMuscular = "Cardio", Equipamento = "Elíptico" },
        new Exercicio { Nome = "Pular Corda", Execucao = "Pular corda em ritmo constante ou com intervalos de alta intensidade.", GrupoMuscular = "Cardio", Equipamento = "Corda" }
    };
        }
    }
}