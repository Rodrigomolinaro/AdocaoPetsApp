using System;

namespace AdocaoPetsApp.Models
{
    public class MinhaAdocaoDTO
    {
        public int IdAdocao { get; set; }
        public int IdPet { get; set; }
        public string? NomePet { get; set; }
        public string? Raca { get; set; }
        public string? StatusAdocao { get; set; } // O status que estava em falta na tela
        public string? NomeOng { get; set; }
        public string? TelefoneOng { get; set; }
        public string? EmailOng { get; set; }
        public DateTime? DataSolicitacao { get; set; }
    }
}