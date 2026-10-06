using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; // <-- Necessário para o [Table]

namespace AdocaoPetsApp.Models
{
    [Table("Usuarios")] // <-- Trocamos para o formato do Entity Framework
    public class Usuario : Cadastro
    {
        [Key]
        public int IdUsuario { get; set; }

        // O Entity Framework não usa [Unique] aqui. 
        // A garantia de CPF único é feita na configuração do banco de dados.
        public string CPF { get; set; }

        public string Senha { get; set; }
        public DateTime DataCadastro { get; set; }

        public Usuario()
        {
            DataCadastro = DateTime.Now;
        }
    }
}