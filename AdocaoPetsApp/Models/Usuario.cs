using SQLite;
using System;

namespace AdocaoPetsApp.Models
{
    [SQLite.Table("Usuarios")] // <-- Adicionamos SQLite. aqui
    public class Usuario : Cadastro
    {
        [PrimaryKey, AutoIncrement]
        public int IdUsuario { get; set; }

        [Unique]
        public string CPF { get; set; }

        public string Senha { get; set; }
        public DateTime DataCadastro { get; set; }

        public Usuario()
        {
            DataCadastro = DateTime.Now;
        }
    }
}