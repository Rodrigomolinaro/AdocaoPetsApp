using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdocaoPetsApp.Models
{
    [Table("usuario")]
    public class Usuario
    {
        [Key]
        [Column("id_usuario")]
        public int IdUsuario { get; set; }

        [Column("nome")]
        public string? Nome { get; set; }

        [Column("telefone")]
        public string? Telefone { get; set; }

        [Column("email")]
        public string? Email { get; set; }

        [Column("endereco")]
        public string? Endereco { get; set; }

        [Column("cidade")]
        public string? Cidade { get; set; }

        [Column("cep")]
        public string? Cep { get; set; }

        [Column("cpf")]
        public string? CPF { get; set; }

        [Column("senha")]
        public string? Senha { get; set; }

        [Column("data_cadastro")]
        public DateTime? DataCadastro { get; set; }

        public Usuario()
        {
            DataCadastro = DateTime.Now;
        }
    }
}