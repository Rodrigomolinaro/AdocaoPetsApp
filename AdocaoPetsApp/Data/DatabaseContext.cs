using Microsoft.EntityFrameworkCore;
using AdocaoPetsApp.Models;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace AdocaoPetsApp.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        // Mapeamento das tabelas do banco de dados
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<ONG> ONGs { get; set; }
        public DbSet<Pet> Pets { get; set; }

        public DbSet<Adocao> Adocoes { get; set; }

        // Mapeia os modelos para os nomes exatos das tabelas no MySQL
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1. Mapeamento dos nomes das tabelas no singular
            modelBuilder.Entity<Usuario>().ToTable("usuario");
            modelBuilder.Entity<ONG>().ToTable("ong");
            modelBuilder.Entity<Pet>().ToTable("pet");

            // 2. Mapeamento das colunas essenciais
            modelBuilder.Entity<Usuario>().Property(u => u.IdUsuario).HasColumnName("id_usuario");
            modelBuilder.Entity<Usuario>().Property(u => u.DataCadastro).HasColumnName("data_cadastro");

            modelBuilder.Entity<ONG>().Property(o => o.IdOng).HasColumnName("id_ong");
            modelBuilder.Entity<ONG>().Property(o => o.Cnpj).HasColumnName("cnpj");
        }

        // Método de Autenticação (Login)
        public async Task<object> AutenticarAsync(string emailOuDocumento, string senha)
        {
            var usuario = await Usuarios.FirstOrDefaultAsync(u =>
                (u.Email == emailOuDocumento || u.CPF == emailOuDocumento) && u.Senha == senha);

            if (usuario != null)
                return usuario;

            var ong = await ONGs.FirstOrDefaultAsync(o =>
                (o.Email == emailOuDocumento || o.Cnpj == emailOuDocumento) && o.Senha == senha);

            if (ong != null)
                return ong;

            return null;
        }
        // Busca as adoções do usuário logado cruzando Dados (Adoção + Pet + ONG)
        public async Task<List<SolicitacaoDTO>> ObterMinhasAdocoesAsync(int idUsuario)
        {
            var query = from adocao in Adocoes
                        join pet in Pets on adocao.IdPet equals pet.IdPet
                        join ong in ONGs on pet.IdOng equals ong.IdOng // BUSCA CORRETA NA TABELA DE ONGS
                        where adocao.IdUsuario == idUsuario
                        select new SolicitacaoDTO
                        {
                            IdAdocao = adocao.IdAdocao,
                            IdPet = pet.IdPet,
                            NomePet = pet.Nome,
                            NomeAdotante = ong.Nome, // Se a ONG usar NomeFantasia ou RazaoSocial, mude aqui
                            EmailAdotante = ong.Email,
                            DataSolicitacao = adocao.DataSolicitacao,
                            StatusAdocao = adocao.StatusAdocao
                        };

            return await query.OrderByDescending(a => a.DataSolicitacao).ToListAsync();
        }


        // Método para registrar um novo Adotante (Pessoa Física)
        public async Task<bool> CadastrarAdotanteAsync(string nome, string cpf, string telefone, string email, string cep, string endereco, string cidade, string senha)
        {
            try
            {
                var novoUsuario = new Usuario
                {
                    Nome = nome,
                    CPF = cpf,
                    Telefone = telefone,
                    Email = email,
                    CEP = cep,
                    Endereco = endereco,
                    Cidade = cidade,
                    Senha = senha,
                    DataCadastro = DateTime.Now
                };

                Usuarios.Add(novoUsuario);
                return await SaveChangesAsync() > 0;
            }
            catch
            {
                return false;
            }
        }

        // Método para registrar uma nova ONG
        public async Task<bool> SalvarOngAsync(ONG novaOng)
        {
            try
            {
                ONGs.Add(novaOng);
                return await SaveChangesAsync() > 0;
            }
            catch
            {
                return false;
            }
        }

        // Método para buscar os animais disponíveis (que não estão adotados)
        public async Task<List<Pet>> ObterPetsDisponiveisAsync()
        {
            try
            {
                return await Pets.Where(p => p.Status == null || p.Status != "Adotado").ToListAsync();
            }
            catch
            {
                return new List<Pet>();
            }
        }

        // Retorna todos os pets cadastrados por uma ONG específica
        public async Task<List<Pet>> ObterPetsPorOngAsync(int idOng)
        {
            try
            {
                return await Pets.Where(p => p.IdOng == idOng && (p.Status == null || p.Status != "Adotado")).ToListAsync();
            }
            catch
            {
                return new List<Pet>();
            }
        }

        // Marca o pet como adotado atualizando a coluna status
        public async Task<bool> MarcarComoAdotadoAsync(int idPet)
        {
            try
            {
                var pet = await Pets.FindAsync(idPet);
                if (pet != null)
                {
                    pet.Status = "Adotado";
                    return await SaveChangesAsync() > 0;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }


        // Método para solicitar adoção
        public async Task<bool> SolicitarAdocaoAsync(int idUsuario, int idPet)
        {
            try
            {
                // Verifica se já existe um pedido pendente desta pessoa para este pet (evita cliques duplos)
                var jaExiste = await Adocoes.AnyAsync(a => a.IdUsuario == idUsuario && a.IdPet == idPet && a.StatusAdocao == "Pendente");
                if (jaExiste) return false;

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
            catch
            {
                return false;
            }
        }  // <-- ADICIONE ESTA CHAVE para fechar SolicitarAdocaoAsync

        // 1. Busca os pedidos cruzando Dados (Adoção + Pet + Adotante)
        public async Task<List<SolicitacaoDTO>> ObterSolicitacoesPendentesAsync()
        {
            // Usamos o LINQ para fazer um JOIN entre as 3 tabelas automaticamente
            var query = from adocao in Adocoes
                        join pet in Pets on adocao.IdPet equals pet.IdPet
                        join usuario in Usuarios on adocao.IdUsuario equals usuario.IdUsuario
                        where adocao.StatusAdocao == "Pendente"
                        select new SolicitacaoDTO
                        {
                            IdAdocao = adocao.IdAdocao,
                            IdPet = pet.IdPet,
                            NomePet = pet.Nome,
                            NomeAdotante = usuario.Nome,
                            EmailAdotante = usuario.Email,
                            DataSolicitacao = adocao.DataSolicitacao,
                            StatusAdocao = adocao.StatusAdocao
                        };

            return await query.ToListAsync();
        }

        // 2. Método para a ONG aprovar o pedido
        public async Task<bool> AprovarAdocaoAsync(int idAdocao, int idPet)
        {
            // Busca o pedido e o pet na base de dados
            var adocao = await Adocoes.FindAsync(idAdocao);
            var pet = await Pets.FindAsync(idPet);

            if (adocao != null && pet != null)
            {
                // Muda o status de ambos
                adocao.StatusAdocao = "Aprovada";
                pet.Status = "Adotado"; // Assim ele sai da lista pública!

                // Se houverem outros pedidos pendentes para o mesmo pet, pode rejeitá-los aqui no futuro

                return await SaveChangesAsync() > 0;
            }

            return false;
        }
    }
    
}