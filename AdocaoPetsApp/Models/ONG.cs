using SQLite;

namespace AdocaoPetsApp.Models
{
    [SQLite.Table("ONGs")] // <-- Adicionamos SQLite. aqui também
    public class ONG : Cadastro
    {
        [PrimaryKey, AutoIncrement]
        public int IdONG { get; set; }

        [Unique]
        public string CNPJ { get; set; }

        public string Senha { get; set; }
    }
}