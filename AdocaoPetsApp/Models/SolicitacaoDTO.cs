using System;

namespace AdocaoPetsApp.Models
{
    public class SolicitacaoDTO
    {
        public int IdAdocao { get; set; }
        public int IdPet { get; set; }
        public string NomePet { get; set; }
        public string NomeAdotante { get; set; }
        public string EmailAdotante { get; set; }
        public DateTime? DataSolicitacao { get; set; }
        public string StatusAdocao { get; set; }
    }
}