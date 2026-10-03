using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdocaoPetsApp.Models
{
    [Table("historicomedico")]
    public class HistoricoMedico
    {
        [Key]
        [Column("id_historico")]
        public int IdHistorico { get; set; }

        [Column("doencas")]
        public string Doencas { get; set; }

        [Column("medicamentos")]
        public string Medicamentos { get; set; }

        [Column("cirurgias")]
        public string Cirurgias { get; set; }

        [Column("observacoes")]
        public string Observacoes { get; set; }

        [Column("id_pet")]
        public int IdPet { get; set; }

        [ForeignKey("IdPet")]
        public Pet Pet { get; set; }
    }
}