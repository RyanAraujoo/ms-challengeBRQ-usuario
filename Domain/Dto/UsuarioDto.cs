using Domain.Entity;
using Domain.Enum;
using Domain.Validation;
using System.ComponentModel.DataAnnotations;


namespace Domain.Dto
{
    public class UsuarioDto
    {
        [Required(ErrorMessage = "O campo CPF deve estar preenchido.")]
        public string Cpf { get; set; }

        [Required(ErrorMessage = "O campo Email deve estar preenchido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "o campo DataDeNascimento deve estar preenchido.")]

        [ValidarDataDeNascimento]
        public string DataDeNascimento { get; set; }

        [Required(ErrorMessage = "o campo Sexo deve estar preenchido.")]
        public string Sexo { get; set; }

        [Required(ErrorMessage = "o campo NomeCompleto deve estar preenchido.")]
        public string NomeCompleto { get; set; }

        [Required(ErrorMessage = "o campo Senha deve estar preenchido.")]
        public string Senha { get; set; }
        public string? Apelido { get; set; }

        [Required(ErrorMessage = "o campo Telefone deve estar preenchido.")]
        public string Telefone { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataAtualizacao { get; set; }

        [Required(ErrorMessage = "o campo Endereço deve estar preenchido.")]
        public EnderecoDto Endereco { get; set; }

    }
}
