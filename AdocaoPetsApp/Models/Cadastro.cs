using System.ComponentModel.DataAnnotations.Schema;

namespace AdocaoPetsApp.Models
{
    public abstract class Cadastro
    {
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
        public string? CEP { get; set; }
    }
}