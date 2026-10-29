using AdocaoPetsApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdocaoPetsApp.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext()
        {
        }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        // Mapeamento de Tabelas oficiais do rodrigomolinaro_pets[cite: 9]
        public DbSet<ONG> Ongs { get; set; }
        public DbSet<ONG> ONGs { get; set; }
        public DbSet<ONG> Ong { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Pet> Pets { get; set; }
        public DbSet<Adocao> Adocoes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = "Server=mysql-rodrigomolinaro.alwaysdata.net;Port=3306;Database=rodrigomolinaro_pets;Uid=rodrigomolinaro_pets;Pwd=Rm541632;";
                optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("usuario");
                entity.HasKey(e => e.IdUsuario);
                entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
                entity.Property(e => e.Nome).HasColumnName("nome");
                entity.Property(e => e.Telefone).HasColumnName("telefone");
                entity.Property(e => e.Email).HasColumnName("email");
                entity.Property(e => e.Endereco).HasColumnName("endereco");
                entity.Property(e => e.Cidade).HasColumnName("cidade");
                entity.Property(e => e.Cep).HasColumnName("cep");
                entity.Property(e => e.CPF).HasColumnName("cpf");
                entity.Property(e => e.Senha).HasColumnName("senha");
                entity.Property(e => e.DataCadastro).HasColumnName("data_cadastro");
            });

            modelBuilder.Entity<ONG>(entity =>
            {
                entity.ToTable("ong");
                entity.HasKey(e => e.IdOng);
                entity.Property(e => e.IdOng).HasColumnName("id_ong");
            });

            modelBuilder.Entity<Pet>(entity =>
            {
                entity.ToTable("pet");
                entity.HasKey(e => e.IdPet);
                entity.Property(e => e.IdPet).HasColumnName("id_pet");
                entity.Property(e => e.IdOng).HasColumnName("id_ong");
            });

            modelBuilder.Entity<Adocao>(entity =>
            {
                entity.ToTable("adocao");
                entity.HasKey(e => e.IdAdocao);
                entity.Property(e => e.IdAdocao).HasColumnName("id_adocao");
                entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
                entity.Property(e => e.IdPet).HasColumnName("id_pet");
                entity.Property(e => e.DataSolicitacao).HasColumnName("data_solicitacao");
                entity.Property(e => e.StatusAdocao).HasColumnName("status_adocao");
                entity.Property(e => e.Observacoes).HasColumnName("observacoes");
            });
        }

        // --- MÉTODOS DE CADASTRO E LOGIN ---

        public async Task<bool> CadastrarAdotanteAsync(Usuario usuario)
        {
            Usuarios.Add(usuario);
            return await SaveChangesAsync() > 0;
        }

        public async Task<bool> CadastrarAdotanteAsync(string nome, string telefone, string email, string endereco, string cidade, string cep, string cpf, string senha)
        {
            var usuario = new Usuario
            {
                Nome = nome,
                Telefone = telefone,
                Email = email,
                Endereco = endereco,
                Cidade = cidade,
                Cep = cep,
                CPF = cpf,
                Senha = senha,
                DataCadastro = DateTime.Now
            };
            Usuarios.Add(usuario);
            return await SaveChangesAsync() > 0;
        }
        public async Task<bool> SalvarOngAsync(ONG ong)
        {
            Ongs.Add(ong);
            return await SaveChangesAsync() > 0;
        }

        public async Task<bool> CadastrarOngAsync(ONG ong) => await SalvarOngAsync(ong);
        // --- MÉTODOS DE PETS E GESTÃO ---

        public async Task<List<Pet>> ObterPetsPorOngAsync(int idOng)
        {
            return await Pets.Where(p => p.IdOng == idOng).ToListAsync();
        }

        public async Task<List<Pet>> ObterPetsDisponiveisAsync()
        {
            return await Pets.Where(p => p.Status == "Disponível" && (p.Adotado == false || p.Adotado == null)).ToListAsync();
        }

        // --- MÉTODOS DE ADOÇÃO E SOLICITAÇÕES ---

        public async Task<bool> SolicitarAdocaoAsync(int idUsuario, int idPet)
        {
            var existe = await Adocoes.AnyAsync(a => a.IdUsuario == idUsuario && a.IdPet == idPet && a.StatusAdocao == "Pendente");
            if (existe) return false;

            var novaAdocao = new Adocao
            {
                IdUsuario = idUsuario,
                IdPet = idPet,
                DataSolicitacao = DateTime.Now,
                StatusAdocao = "Pendente"
            };

            Adocoes.Add(novaAdocao);
            return await SaveChangesAsync() > 0;
        }

        public async Task<List<SolicitacaoDTO>> ObterSolicitacoesPendentesAsync()
        {
            var query = from a in Adocoes
                        join p in Pets on a.IdPet equals p.IdPet
                        join u in Usuarios on a.IdUsuario equals u.IdUsuario
                        where a.StatusAdocao == "Pendente"
                        select new SolicitacaoDTO
                        {
                            IdAdocao = a.IdAdocao,
                            IdPet = p.IdPet,
                            NomePet = p.Nome,
                            NomeAdotante = u.Nome,
                            EmailAdotante = u.Email,
                            DataSolicitacao = a.DataSolicitacao ?? DateTime.Now
                        };

            return await query.ToListAsync();
        }

        public async Task<bool> AprovarAdocaoAsync(int idAdocao, int idPet)
        {
            var adocao = await Adocoes.FindAsync(idAdocao);
            var pet = await Pets.FindAsync(idPet);

            if (adocao == null || pet == null) return false;

            adocao.StatusAdocao = "Aprovado";
            pet.Status = "Adotado";
            pet.Adotado = true;

            return await SaveChangesAsync() > 0;
        }

        public async Task<List<MinhaAdocaoDTO>> ObterMinhasAdocoesAsync(int idUsuario)
        {
            var query = from a in Adocoes
                        join p in Pets on a.IdPet equals p.IdPet
                        join o in Ongs on p.IdOng equals o.IdOng into ongGroup
                        from ong in ongGroup.DefaultIfEmpty()
                        where a.IdUsuario == idUsuario
                        select new MinhaAdocaoDTO
                        {
                            IdAdocao = a.IdAdocao,
                            IdPet = p.IdPet,
                            NomePet = p.Nome,
                            Raca = p.Raca,
                            StatusAdocao = a.StatusAdocao ?? "Pendente",
                            NomeOng = ong != null ? ong.Nome : "ONG Parceira",
                            TelefoneOng = ong != null ? ong.Telefone : "Não informado",
                            EmailOng = ong != null ? ong.Email : "Não informado",
                            DataSolicitacao = a.DataSolicitacao
                        };

            return await query.ToListAsync();
        }
    }
}