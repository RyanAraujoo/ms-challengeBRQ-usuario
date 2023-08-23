using System.ComponentModel.DataAnnotations;

namespace Application.InputModels
{
    public class EnderecoInputModel
    {
        public EnderecoInputModel(string? complemento, string? numero, string? cep, string? bairro, string? logradouro)
        {
            Complemento = complemento;
            Numero = numero;
            Cep = cep;
            Bairro = bairro;
            Logradouro = logradouro;
        }

        [MaxLength(50, ErrorMessage = "O BAIRRO deve ter, no máximo, 50 caracteres.")]
        public string? Complemento { get; set; }

        [Required(ErrorMessage = "o campo NÚMERO deve estar preenchido.")]
        [MaxLength(10, ErrorMessage = "O NÚMERO deve ter, no máximo, 10 caracteres.")]
        public string? Numero { get; set; }

        [Required(ErrorMessage = "o campo CEP deve estar preenchido.")]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "O CEP deve ter 8 caracteres.")]
        public string? Cep { get; set; }

        public string? Bairro { get; set; }
        public string? Logradouro { get; set; }
    }
}
