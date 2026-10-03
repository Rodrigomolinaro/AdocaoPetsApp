using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdocaoPetsApp.Models
{
    [Table("notificacao")]
    public class Notificacao
    {
        [Key]
        [Column("id_notificacao")]
        public int IdNotificacao { get; set; }

        [Column("mensagem")]
        public string Mensagem { get; set; }

        [Column("data_envio")]
        public DateTime? DataEnvio { get; set; }

        [Column("status_leitura")]
        public string StatusLeitura { get; set; }

        [Column("id_usuario")]
        public int IdUsuario { get; set; }

        [ForeignKey("IdUsuario")]
        public Usuario Usuario { get; set; }
    }
}