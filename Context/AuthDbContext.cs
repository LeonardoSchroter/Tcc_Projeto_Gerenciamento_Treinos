using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Projeto_gerencia_treinos_musculacao.Models;

namespace Projeto_gerencia_treinos_musculacao.Data
{
    public class AuthDbContext : IdentityDbContext<Usuario>
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options)
            : base(options)
        {
        }


        public DbSet<Anamnese> Anamneses { get; set; }
        public DbSet<Exercicio> Exercicios { get; set; }

        public DbSet<Ficha> Fichas { get; set; }
        public DbSet<Treino> Treinos { get; set; }
        public DbSet<ItemTreino> ItensTreino { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Anamnese>()
               .Property(e => e.Id)
               .ValueGeneratedOnAdd();

            modelBuilder.Entity<Exercicio>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Ficha>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Treino>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<ItemTreino>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();


            modelBuilder.Entity<Usuario>()
               .HasOne(u => u.Personal)
               .WithMany(u => u.Alunos)
               .HasForeignKey(u => u.PersonalId)
               .OnDelete(DeleteBehavior.Restrict); 

            // Relacionamentos da entidade Ficha
            modelBuilder.Entity<Ficha>(entity =>
            {
                // Ficha -> Aluno (Usuário)
                entity.HasOne(f => f.Aluno)
                      .WithMany(u => u.FichasPossuidas) // Certifique-se que essa coleção existe no seu modelo Usuario
                      .HasForeignKey(f => f.IdAluno)
                      .OnDelete(DeleteBehavior.Cascade); // Se um aluno for deletado, suas fichas também serão.

                // Ficha -> Personal (Usuário)
                entity.HasOne(f => f.Personal)
                      .WithMany(u => u.FichasCriadas) // Certifique-se que essa coleção existe no seu modelo Usuario
                      .HasForeignKey(f => f.IdPersonal)
                      .OnDelete(DeleteBehavior.Restrict); // Impede que um personal seja deletado se tiver criado fichas.
            });

            // Relacionamentos da entidade Treino
            modelBuilder.Entity<Treino>(entity =>
            {
                // Treino -> Ficha (um treino pertence a uma ficha)
                entity.HasOne(t => t.Ficha)
                      .WithMany(f => f.Treinos) // A Ficha tem uma coleção de Treinos
                      .HasForeignKey(t => t.FichaId)
                      .OnDelete(DeleteBehavior.Cascade); // Se uma ficha for deletada, seus treinos também serão.
            });

            // Relacionamentos da entidade ItemTreino
            modelBuilder.Entity<ItemTreino>(entity =>
            {
                // ItemTreino -> Treino (um item pertence a um treino)
                entity.HasOne(it => it.Treino)
                      .WithMany(t => t.ItensTreino) // O Treino tem uma coleção de ItensTreino
                      .HasForeignKey(it => it.TreinoId)
                      .OnDelete(DeleteBehavior.Cascade); // Se um treino for deletado, seus itens também serão.

                // ItemTreino -> Exercicio
                entity.HasOne(it => it.Exercicio)
                      .WithMany(e=>e.ItemTreino) // Se a classe Exercicio não tiver uma coleção de ItensTreino, deixe o WithMany() vazio.
                      .HasForeignKey(it => it.ExercicioId)
                      .OnDelete(DeleteBehavior.Restrict); // Impede que um exercício seja deletado se estiver em uso em algum treino.
            });

            modelBuilder.Entity<Anamnese>(entity =>
            {
                // Anamnese -> Aluno
                entity.HasOne(a => a.Usuario)
                      .WithMany(u => u.Anamneses)
                      .HasForeignKey(a => a.UsuarioId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Anamnese -> Personal
                entity.HasOne(a => a.Personal)
                      .WithMany(u => u.AnamnesesCriadas)
                      .HasForeignKey(a => a.PersonalId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

        }
    }
}