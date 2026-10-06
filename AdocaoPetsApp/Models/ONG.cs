using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdocaoPetsApp.Models
{
    [Table("ong")]
    public class ONG
    {
        [Key]
        [Column("id_ong")]
        public int IdOng { get; set; }

        [Column("nome")]
        public string Nome { get; set; }

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

        [Column("nome_ong")]
        public string? NomeOng { get; set; }

        [Column("cnpj")]
        public string? Cnpj { get; set; }

        [Column("senha")] public string? Senha { get; set; }
    }
}