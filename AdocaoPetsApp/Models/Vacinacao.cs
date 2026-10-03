using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdocaoPetsApp.Models
{
    [Table("vacinacao")]
    public class Vacinacao
    {
        [Key]
        [Column("id_vacina")]
        public int IdVacina { get; set; }

        [Column("nome_vacina")]
        public string NomeVacina { get; set; }

        [Column("data_aplicacao")]
        public DateTime? DataAplicacao { get; set; }

        [Column("dose")]
        public string Dose { get; set; }

        [Column("id_pet")]
        public int IdPet { get; set; }

        [ForeignKey("IdPet")]
        public Pet Pet { get; set; }
    }
}