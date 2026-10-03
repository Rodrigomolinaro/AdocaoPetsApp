using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdocaoPetsApp.Models
{
    [Table("clinicaveterinaria")]
    public class ClinicaVeterinaria : Cadastro
    {
        [Key]
        [Column("id_clinica")]
        public int IdClinica { get; set; }

        [Column("nome_clinica")]
        public string NomeClinica { get; set; }

        [Column("especialidade")]
        public string Especialidade { get; set; }
    }
}