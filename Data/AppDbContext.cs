using Microsoft.EntityFrameworkCore;
using UbecSolicitacoes.Models;

namespace UbecSolicitacoes.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Aluno> Alunos { get; set; }
        public DbSet<Solicitacao> Solicitacoes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seed - 4 alunos (1 inativo)
            modelBuilder.Entity<Aluno>().HasData(
                new Aluno { Id = 1, Nome = "João Silva", Matricula = "2024001", Ativo = true },
                new Aluno { Id = 2, Nome = "Maria Oliveira", Matricula = "2024002", Ativo = true },
                new Aluno { Id = 3, Nome = "Pedro Santos", Matricula = "2024003", Ativo = true },
                new Aluno { Id = 4, Nome = "Ana Costa", Matricula = "2024004", Ativo = false }
            );

            // Seed - 5 solicitações (pelo menos 2 atrasadas)
            var hoje = DateTime.Now;
            var ontem = hoje.AddDays(-1);
            var tresDiasAtras = hoje.AddDays(-3);
            var cincoDiasAtras = hoje.AddDays(-5);

            modelBuilder.Entity<Solicitacao>().HasData(
                new Solicitacao { Id = 1, AlunoId = 1, TipoDocumento = "Declaração de Matrícula", Status = "Pendente", CriadaEm = hoje, PrazoLimite = hoje.AddDays(2) },
                new Solicitacao { Id = 2, AlunoId = 2, TipoDocumento = "Atestado de Frequência", Status = "Pendente", CriadaEm = ontem.AddDays(-3), PrazoLimite = ontem },
                new Solicitacao { Id = 3, AlunoId = 1, TipoDocumento = "Histórico Escolar", Status = "Concluída", CriadaEm = cincoDiasAtras, PrazoLimite = cincoDiasAtras.AddDays(5) },
                new Solicitacao { Id = 4, AlunoId = 3, TipoDocumento = "Declaração de Matrícula", Status = "Pendente", CriadaEm = tresDiasAtras.AddDays(-2), PrazoLimite = tresDiasAtras },
                new Solicitacao { Id = 5, AlunoId = 2, TipoDocumento = "Histórico Escolar", Status = "Cancelada", CriadaEm = hoje, PrazoLimite = hoje.AddDays(5) }
            );
        }
    }
}