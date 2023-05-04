using Domain.Validation.EnderecoValidation;
using System.ComponentModel.DataAnnotations;

namespace Domain.Dto
{
    public class EnderecoDto
    {
        [Required(ErrorMessage = "o campo LOGRADOURO deve estar preenchido.")]
        [MaxLength(100, ErrorMessage = "O LOGRADOURO deve ter, no máximo, 100 caracteres.")]
        public string? Logradouro { get; set; }
        [MaxLength(50, ErrorMessage = "O BAIRRO deve ter, no máximo, 50 caracteres.")]
        public string? Complemento { get; set; }

        [Required(ErrorMessage = "o campo NÚMERO deve estar preenchido.")]
        [MaxLength(10, ErrorMessage = "O NÚMERO deve ter, no máximo, 10 caracteres.")]
        public string? Numero { get; set; }

        [Required(ErrorMessage = "o campo BAIRRO deve estar preenchido.")]
        [MaxLength(20, ErrorMessage = "O BAIRRO deve ter, no máximo, 20 caracteres.")]
        public string? Bairro { get; set; }

        [Required(ErrorMessage = "o campo CIDADE deve estar preenchido.")]
        [MaxLength(20, ErrorMessage = "O CIDADE deve ter, no máximo, 20 caracteres.")]
        public string? Cidade { get; set; }

        [Required(ErrorMessage = "o campo CIDADE deve estar preenchido.")]
        [MaxLength(2, ErrorMessage = "O CIDADE deve ter, no máximo, 2 caracteres.")]
        [ValidarEstado]
        public string? Estado { get; set; }

        [Required(ErrorMessage = "o campo PAÍS deve estar preenchido.")]
        [MaxLength(2, ErrorMessage = "O PAIS deve ter, no máximo, 2 caracteres.")]
        [ValidarPais]
        public string? Pais { get; set; }

        [Required(ErrorMessage = "o campo CEP deve estar preenchido.")]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "O CEP deve ter 8 caracteres.")]
        public string? Cep { get; set; }
    }
}
