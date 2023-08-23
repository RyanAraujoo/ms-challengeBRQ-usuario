using Domain.Validation.UsuarioValidation;
using System.ComponentModel.DataAnnotations;

namespace Application.InputModels
{
    public class UsuarioInputModel
    {
        [Required(ErrorMessage = "O campo CPF deve estar preenchido.")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "O CPF deve ter 11 caracteres.")]
        [ValidarCpf]
        public string? Cpf { get; set; }

        [Required(ErrorMessage = "O campo EMAIL deve estar preenchido.")]
        [ValidarEmail]
        [MaxLength(50, ErrorMessage = "O EMAIL deve ter, no máximo, 50 caracteres.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "o campo DataDeNascimento deve estar preenchido.")]
        [ValidarDataDeNascimento]
        public string? DataDeNascimento { get; set; }

        [Required(ErrorMessage = "o campo SEXO deve estar preenchido.")]
        [MaxLength(2, ErrorMessage = "O SEXO deve ter, no máximo, 2 caracteres.")]
        public string? Sexo { get; set; }

        [Required(ErrorMessage = "o campo NomeCompleto deve estar preenchido.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "O NomeCompleto deve ter entre 2 e 100 caracteres.")]
        [ValidarNomeCompleto]
        public string? NomeCompleto { get; set; }

        [Required(ErrorMessage = "o campo SENHA deve estar preenchido.")]
        [MaxLength(100, ErrorMessage = "A SENHA deve ter, no máximo, 100 caracteres.")]
        public string? Senha { get; set; }
        [MaxLength(20, ErrorMessage = "O APELIDO deve ter, no máximo, 20 caracteres.")]
        public string? Apelido { get; set; }

        [Required(ErrorMessage = "o campo TELEFONE deve estar preenchido.")]
        [ValidarTelefone]
        [MaxLength(11, ErrorMessage = "O TELEFONE deve ter, no máximo, 11 caracteres.")]
        public string? Telefone { get; set; }
        public DateTime? DataCadastro { get; set; }
        public DateTime? DataAtualizacao { get; set; }

        [Required(ErrorMessage = "o campo ENDEREÇO deve estar preenchido.")]
        public EnderecoInputModel Endereco { get; set; }
    }
}
