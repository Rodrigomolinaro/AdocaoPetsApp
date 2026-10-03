using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AdocaoPetsApp.Models;

namespace AdocaoPetsApp.Data
{
    public class DatabaseContext : DbContext
    {
        // Declaração de todas as tabelas do sistema
       
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<ONG> ONGs { get; set; }
        public DbSet<ClinicaVeterinaria> Clinicas { get; set; }
        public DbSet<Pet> Pets { get; set; }
        public DbSet<Adocao> Adocoes { get; set; }
        public DbSet<ConsultaVeterinaria> ConsultasVeterinarias { get; set; }
        public DbSet<HistoricoMedico> HistoricosMedicos { get; set; }
        public DbSet<Vacinacao> Vacinacoes { get; set; }
        public DbSet<Notificacao> Notificacoes { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Garantir a relação de 1 para 1 (UNIQUE) no Histórico Médico, conforme definido no SQL.
            modelBuilder.Entity<HistoricoMedico>()
                .HasIndex(h => h.IdPet)
                .IsUnique();
        }

        // ==========================================================
        // Métodos da Sprint 1 reescritos para Entity Framework Core
        // ==========================================================

        public async Task<int> SalvarUsuarioAsync(Usuario usuario)
        {
            Usuarios.Add(usuario);
            return await SaveChangesAsync();
        }

        public async Task<int> SalvarOngAsync(ONG ong)
        {
            ONGs.Add(ong);
            return await SaveChangesAsync();
        }

        public async Task<object> AutenticarAsync(string email, string senha)
        {
            // 1. Tenta encontrar na tabela de Usuários (Adotantes)
            var usuario = await Usuarios.FirstOrDefaultAsync(u => u.Email == email && u.Senha == senha);
            if (usuario != null)
            {
                return usuario;
            }

            // 2. Tenta encontrar na tabela de ONGs
            // NOTA: No teu diagrama e script SQL (codSqlTCC.txt), a tabela ONG não tinha o campo "Senha".
            // Por isso, estou a procurar apenas pelo Email. 
            // Se entretanto adicionaste a propriedade "Senha" na classe ONG.cs, podes alterar a linha abaixo para:
            // var ong = await ONGs.FirstOrDefaultAsync(o => o.Email == email && o.Senha == senha);

            var ong = await ONGs.FirstOrDefaultAsync(o => o.Email == email);
            if (ong != null)
            {
                return ong;
            }

            // Se não encontrar nem Usuário nem ONG, devolve nulo (login inválido)
            return null;
        }
    }
    
}