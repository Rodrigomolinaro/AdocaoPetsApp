using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdocaoPetsApp.Models
{
    [Table("ong")]
    public class ONG : Cadastro
    {
        [Key]
        [Column("id_ong")]
        public int IdOng { get; set; }

        [Column("nome_ong")]
        public string? NomeOng { get; set; }

        [Column("cnpj")]
        public string? Cnpj { get; set; }

        [Column("senha")]
        public string? Senha { get; set; }
    }
}