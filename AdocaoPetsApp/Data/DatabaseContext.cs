using SQLite;
using AdocaoPetsApp.Models;
using System.IO;
using System.Threading.Tasks;

namespace AdocaoPetsApp.Data
{
    public class DatabaseContext
    {
        private readonly SQLiteAsyncConnection _database;

        public DatabaseContext(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);

            // Cria as tabelas assim que o banco for inicializado
            _database.CreateTableAsync<Usuario>().Wait();
            _database.CreateTableAsync<ONG>().Wait();
        }

        // Exemplo de método para salvar um Adotante (Sprint 1)
        public Task<int> SalvarUsuarioAsync(Usuario usuario)
        {
            if (usuario.IdUsuario != 0)
                return _database.UpdateAsync(usuario);
            else
                return _database.InsertAsync(usuario);
        }
        public Task<int> SalvarOngAsync(ONG ong)
        {
            if (ong.IdONG != 0)
                return _database.UpdateAsync(ong);
            else
                return _database.InsertAsync(ong);
        }

        // NOVO MÉTODO: Validação de Login
        public async Task<object> AutenticarAsync(string emailOuDocumento, string senha)
        {
            // Primeiro, procura na tabela de Adotantes (Usuarios)
            var usuario = await _database.Table<Usuario>()
                .FirstOrDefaultAsync(u => (u.Email == emailOuDocumento || u.CPF == emailOuDocumento) && u.Senha == senha);

            if (usuario != null)
                return usuario;

            // Se não encontrou, procura na tabela de ONGs
            var ong = await _database.Table<ONG>()
                .FirstOrDefaultAsync(o => (o.Email == emailOuDocumento || o.CNPJ == emailOuDocumento) && o.Senha == senha);

            return ong; // Retornará a ONG ou 'null' se as credenciais estiverem erradas
        }
    }
}