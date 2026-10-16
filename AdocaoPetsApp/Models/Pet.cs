using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdocaoPetsApp.Models
{
    [Table("pet")]
    public class Pet
    {
        [Key]
        [Column("id_pet")]
        public int IdPet { get; set; } // A chave primária nunca tem '?'

        [Column("id_usuario")]
        public int? IdUsuario { get; set; }

        [Column("id_ong")]
        public int? IdOng { get; set; } // <--- CORRIGIDO: Agora aceita NULL!

        [Column("nome")]
        public string? Nome { get; set; }

        [Column("especie")] // Adicionada a anotação para o banco achar a coluna
        public string? Especie { get; set; }

        [Column("idade")]
        public int? Idade { get; set; }

        [Column("raca")]
        public string? Raca { get; set; }

        [Column("adotado")] // Adicionada a anotação da coluna
        public bool? Adotado { get; set; } // <--- CORRIGIDO: Agora aceita NULL!

        [Column("porte")]
        public string? Porte { get; set; }

        [Column("sexo")]
        public string? Sexo { get; set; }

        [Column("cor")]
        public string? Cor { get; set; }

        [Column("status")]
        public string? Status { get; set; }

        [Column("imagem_url")]
        public string? ImagemUrl { get; set; }

        [Column("descricao")]
        public string? Descricao { get; set; }

        [Column("data_resgate")]
        public DateTime? DataResgate { get; set; }

        [ForeignKey("IdOng")]
        public ONG? Ong { get; set; }
    }
}