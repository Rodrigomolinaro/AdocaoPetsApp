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
        public int IdPet { get; set; }

        [Column("especie")]
        public string Especie { get; set; }

        [Column("nome")]
        public string Nome { get; set; }

        [Column("idade")]
        public int? Idade { get; set; }

        [Column("raca")]
        public string Raca { get; set; }

        [Column("porte")]
        public string Porte { get; set; }

        [Column("sexo")]
        public string Sexo { get; set; }

        [Column("cor")]
        public string Cor { get; set; }

        [Column("status")]
        public string Status { get; set; }

        [Column("descricao")]
        public string Descricao { get; set; }

        [Column("data_resgate")]
        public DateTime? DataResgate { get; set; }

        [Column("id_ong")]
        public int IdOng { get; set; }

        [ForeignKey("IdOng")]
        public ONG Ong { get; set; }
    }
}