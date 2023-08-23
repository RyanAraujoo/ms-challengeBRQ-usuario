
using Application.InputModels;

namespace Application.ViewModels
{
    public class UsuarioViewModel
    {
        public UsuarioViewModel(Guid id, string cpf, string email, string dataDeNascimento, string sexo, string nomeCompleto, string apelido, string telefone, DateTime dataCadastro, EnderecoInputModel endereco)
        {
            Id = id;
            Cpf = cpf;
            Email = email;
            DataDeNascimento = dataDeNascimento;
            Sexo = sexo;
            NomeCompleto = nomeCompleto;
            Apelido = apelido;
            Telefone = telefone;
            DataCadastro = dataCadastro;
            Endereco = endereco;
        }

        public Guid Id { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
        public string DataDeNascimento { get; set; }
        public string Sexo { get; set; }
        public string NomeCompleto { get; set; }
        public string Apelido { get; set; }
        public string Telefone { get; set; }
        public DateTime DataCadastro { get; set; }
        public EnderecoInputModel Endereco { get; set; }
    }
}
