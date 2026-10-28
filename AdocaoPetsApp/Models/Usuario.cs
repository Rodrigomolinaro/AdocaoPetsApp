using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdocaoPetsApp.Models
{
    [Table("usuario")] // O nome da tabela no banco está em minúsculas
    public class Usuario : Cadastro
    {
        [Key]
        [Column("id_usuario")]
        public int IdUsuario { get; set; }

        [Column("cpf")]
        public string CPF { get; set; }

        [Column("senha")]
        public string Senha { get; set; }

        // O ponto de interrogação (?) salva a pátria aqui, pois aceita nulos do banco!
        [Column("data_cadastro")]
        public DateTime? DataCadastro { get; set; }

        public Usuario()
        {
            DataCadastro = DateTime.Now;
        }
    }
}