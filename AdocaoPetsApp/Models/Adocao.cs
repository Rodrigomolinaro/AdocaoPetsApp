using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdocaoPetsApp.Models
{
    [Table("adocao")]
    public class Adocao
    {
        [Key]
        [Column("id_adocao")]
        public int IdAdocao { get; set; }

        [Column("data_solicitacao")]
        public DateTime? DataSolicitacao { get; set; }

        [Column("status_adocao")]
        public string? StatusAdocao { get; set; }

        [Column("observacoes")]
        public string? Observacoes { get; set; }

        [Column("id_usuario")]
        public int IdUsuario { get; set; }

        [Column("id_pet")]
        public int IdPet { get; set; }
    }
}