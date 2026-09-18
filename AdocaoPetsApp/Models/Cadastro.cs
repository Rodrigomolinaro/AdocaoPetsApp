using SQLite;

namespace AdocaoPetsApp.Models
{
    public abstract class Cadastro
    {
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string Endereco { get; set; }
        public string Cidade { get; set; }
        public string CEP { get; set; }
    }
}