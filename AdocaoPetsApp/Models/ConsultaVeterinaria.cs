using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdocaoPetsApp.Models
{
    [Table("consultaveterinaria")]
    public class ConsultaVeterinaria
    {
        [Key]
        [Column("id_consulta")]
        public int IdConsulta { get; set; }

        [Column("data_consulta")]
        public DateTime? DataConsulta { get; set; }

        [Column("diagnostico")]
        public string Diagnostico { get; set; }

        [Column("tratamento")]
        public string Tratamento { get; set; }

        [Column("observacoes")]
        public string Observacoes { get; set; }

        [Column("id_clinica")]
        public int IdClinica { get; set; }

        [Column("id_pet")]
        public int IdPet { get; set; }

        [ForeignKey("IdClinica")]
        public ClinicaVeterinaria Clinica { get; set; }

        [ForeignKey("IdPet")]
        public Pet Pet { get; set; }
    }
}